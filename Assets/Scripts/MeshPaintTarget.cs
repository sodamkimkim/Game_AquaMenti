using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshPaintTarget : MonoBehaviour
{
    // 게임 중에서는 RenderTexture 그대로 사용을 하고 Save를 시도할 때 RenderTexture의 Pixel을 복사해서 복사본을 저장하는 것으로 진행.
    // 주의할 점은 Save를 할 때 대상이 되는 모든 RenderTexture에 대해 Pixel을 복사해서 복사본을 저장하는 작업을 진행할 수 있음.
    // ㄴ> 이 부분은 ComputeShader를 통해서 복사본과의 Pixel 동일성을 검증을 하고 다르다면 저장을 진행하게 하면 괜찮지 않을까 싶음.
    // ㄴ> ComputeShader를 사용한다고 했을 때 동일하지 않음을 감지했을 때 바로 작업을 중단하고 빠져나오게 하는 방법이 있을까?

    [SerializeField]
    private ComputeShader paintShader = null;
    [SerializeField]
    private ComputeShader countShader = null;

    private MeshRenderer mr = null; // 대상의 MeshRenderer
    private Texture originTex = null; // UV원본
    private RenderTexture targetTexture = null; // 오염 Mask
    [SerializeField]
    private int resolution = 512;
    private int depth = 32;

    [SerializeField]
    private bool drawable = false;
    [SerializeField]
    private bool isClear = false;
    [SerializeField, Range(50, 100)]
    private int clearPercent = 90;

    // Compute Shader
    private int kernelNoise;
    private int kernelPaint;
    private int kernelCopy;
    private int kernelClear;

    private int kernelInitCount;
    private int kernelCount;

    private readonly string kernelNoiseName = "CSNoise";
    private readonly string kernelPaintName = "CSPaint";
    private readonly string kernelCopyName = "CSCopy";
    private readonly string kernelClearName = "CSClear";

    private readonly string kernelInitCountName = "CSInitCount";
    private readonly string kernelCountName = "CSCount";

    private int threadGroupX;
    private int threadGroupY;

    private readonly string timerName = "_Timer";
    private readonly string twinkleSpeed = "_TwinkleSpeed";
    private bool isCompleteTwinkle { get; set; }
    private bool isDirtyTwinkle { get; set; }


    #region Properties Getter/Setter
    public bool IsDrawable()
    {
        return drawable;
    }
    public void IsDrawable(bool _able)
    {
        drawable = _able;
    }

    public bool IsClear()
    {
        return isClear;
    }
    public void IsClear(bool _clear)
    {
        isClear = _clear;
    }
    #endregion


    private void Awake()
    {
        // MeshRenderer일수도 SkinnedMeshRenderer일수도 있으므로 그냥 받는 변수의 타입에 구애받도록 제네릭을 쓰지 않음
        TryGetComponent(out mr);
    }

    private void Start()
    {
        if (drawable)
            Init();
    }

    private void Update()
    {
        // 그릴 수 있는 대상이고 클리어가 되지 않았다면 남은 구역을 보여줄 수 있도록 함
        if (IsDrawable() && !IsClear() && Input.GetKeyDown(KeyCode.Tab))
        {
            DirtyTwinkle();
        }

        // SaveToPNG Test (저장 네이밍 정의 필요)
        if (IsDrawable() && Input.GetKeyDown(KeyCode.Slash))
        {
            SaveToPNG(ToTexture(targetTexture));
        }
    }


    private void Init()
    {
        // _MainTex: 모델링의 Texture
        // _PaintTex: 오염 Texture
        // _PaintUv: 원본 UV Texture
        // _PaintMask: 오염 Mask (default: none). 코드상에서 추가할 예정

        originTex = mr.material.GetTexture("_PaintUv");

        targetTexture = new RenderTexture(originTex.width, originTex.height, depth);
        targetTexture.name = originTex.name;
        targetTexture.enableRandomWrite = true; // Graphics.Blit을 하기 전에 접근할 수 있게 설정해줘야 적용됨
        Graphics.Blit(originTex, targetTexture); // Texture를 RenderTexture에 복사

        threadGroupX = Mathf.CeilToInt(targetTexture.width / 8);
        threadGroupY = Mathf.CeilToInt(targetTexture.height / 8);

        // !!현재 노이즈 텍스쳐는 생성을 하지만 셰이더에서 사용하고 있지는 않음!!
        // *Compute Shader에서 생성하는데 자연스럽게 뽑아내기 전까지는 Shader Graph의 노이즈를 사용
        SetNoiseTexture();
        SetBasicTwinkleProperties();

        mr.material.SetTexture("_PaintMask", targetTexture);
    }


    // RenderTexture에 Paint Rendering을 함
    public void DrawRender(bool _drawable, Vector2 _uvPos, Color _color, float _size, float _distance/*, Texture2D _texture*/)
    {
        if (IsDrawable() == false || IsClear() == true) return;
        // Brush의 Texture를 받아서 사용하고자 하였으나 문제가 발생하여 지금은 사용하지 않음
#if UNITY_EDITOR
        Debug.Log("DrawRender");
#endif
        /*
            RWTexture2D<float4> PaintMask;
            Texture2D<float4> BrushTex;
            float2 UvPos;
            float4 Color;
            float Size;
            float Distance;
            bool Drawable;
        */

        // 1) Kernel을 가져옴
        kernelPaint = paintShader.FindKernel(kernelPaintName);

        // 2) 초기화가 필요한 경우 여기서 초기화
        // ex) computeBuffer = new ComputeBuffer[count, sizeof(typeof) * cnt]; (uint4) => cnt: 4
        // uvPos는 화면상에서의 비율이므로 해상도 값을 곱함
        Vector2 uvPos = new Vector2((uint)Mathf.CeilToInt(_uvPos.x * resolution), (uint)Mathf.CeilToInt(_uvPos.y * resolution));

        // 3) 설정을 끝마쳤다면 shader에 넘김
        paintShader.SetTexture(kernelPaint, "Result", targetTexture); // Target의 RenderTexture
        //shader.SetTexture(kernelPaint, "BrushTex", _texture); // Brush의 Texture
        paintShader.SetVector("UvPos", uvPos);
        paintShader.SetVector("Color", _color); // Brush의 색상
        paintShader.SetFloat("Size", _size); // Brush의 사이즈
        paintShader.SetFloat("Distance", _distance); // 최대사거리 / 충돌거리
        paintShader.SetBool("Paintable", _drawable); // 그릴 수 있는지 여부

        // 4) 필요한 것을 다 넘겼다면 shader 실행
        // 현재 Shader는 numthreads(8, 8, 1)이면 shader.Dispatch(kernel, width / 8, height / 8, 1);
        paintShader.Dispatch(kernelPaint, threadGroupX, threadGroupY, 1);
#if UNITY_EDITOR
        Debug.Log("Shader Dispatch");
#endif

        // 5) 처리된 정보를 가공하는 부분
        // Buffer를 보냈었다면 Data를 가져오고 Release 후 null 처리
        Graphics.Blit(targetTexture, targetTexture);

        // 6) 반환할 것이 있다면 함수의 반환형을 변경 후 반환
        // return;
    }


    public void SetNoiseTexture()
    {
        RenderTexture rTex = new RenderTexture(resolution, resolution, depth);
        rTex.enableRandomWrite = true;
        rTex.Create();

        kernelNoise = paintShader.FindKernel(kernelNoiseName);

        paintShader.SetTexture(kernelNoise, "Result", rTex);

        paintShader.Dispatch(kernelNoise, threadGroupX, threadGroupY, 1);
#if UNITY_EDITOR
        Debug.Log("Make Noise Texture");
#endif
        Graphics.Blit(rTex, rTex);
        mr.material.SetTexture("_PaintTex", rTex);
    }


    #region Pixel Counter
    public void CheckAutoClear()
    {
        if (IsClear() == true) return;

        if (clearPercent < GetProcessPercent())
        {
            ClearTexture();
            CompleteTwinkle();
        }
    }
    public float GetProcessPercent()
    {
        uint origin = PixelCount(originTex);
        uint target = PixelCount(targetTexture);

        float percent = (origin - target) / (float)origin * 100;
#if UNITY_EDITOR
        Debug.LogFormat("[CheckPercent] origin: {0}, target: {1}, percent: {2}", origin, target, percent);
#endif

        return percent;
    }

    private uint PixelCount(Texture _tex)
    {
        // 1) Kernel을 가져옴
        kernelInitCount = countShader.FindKernel(kernelInitCountName);
        kernelCount = countShader.FindKernel(kernelCountName);

        // 2) 초기화가 필요한 경우 여기서 초기화
        // ex) computeBuffer = new ComputeBuffer[count, sizeof(typeof) * cnt]; (uint4) => cnt: 4
        ComputeBuffer buffer = new ComputeBuffer(1, sizeof(uint) * 1); // size * count | uint1을 사용할 것이므로 1만 곱함
        uint[] data = new uint[1];

        // 3) 설정을 끝마쳤다면 shader에 넘김
        countShader.SetTexture(kernelCount, "InputTexture", _tex); // ComputeShader로 이미지를 보냄
        countShader.SetBuffer(kernelCount, "CountBuffer", buffer); // Buffer도 보냄
        countShader.SetBuffer(kernelInitCount, "CountBuffer", buffer);

        // 4) 필요한 것을 다 넘겼다면 shader 실행
        // 현재 Shader는 numthreads(8, 8, 1)이면 shader.Dispatch(kernel, width / 8, height / 8, 1);
        countShader.Dispatch(kernelInitCount, 1, 1, 1);
        countShader.Dispatch(kernelCount, threadGroupX, threadGroupY, 1);

        // 5) 처리된 정보를 가공하는 부분
        // Buffer를 보냈었다면 Data를 가져오고 Release 후 null 처리
        buffer.GetData(data);
        buffer.Release();
        buffer = null;

        // 6) 반환할 것이 있다면 함수의 반환형을 변경 후 반환
#if UNITY_EDITOR
        Debug.LogFormat("[CountWhiteColor] Left: {0}", data[0]);
#endif
        return data[0];
    }
    #endregion Pixel Counter


    #region Texture Function (Clear, Reset)
    public void ClearTexture()
    {
        if ((targetTexture.width != originTex.width) &&
            (targetTexture.height != originTex.height))
        {
#if UNITY_EDITOR
            Debug.LogWarning("RenderTexture is different in size from origin. Check RenderTexture width and height.");
#endif
            return;
        }

        kernelClear = paintShader.FindKernel(kernelClearName);

        paintShader.SetTexture(kernelClear, "Result", targetTexture);

        paintShader.Dispatch(kernelClear, threadGroupX, threadGroupY, 1);
#if UNITY_EDITOR
        Debug.Log("Dirty Clear.");
#endif
    }

    // 원본UV를 RenderTexture에 복사
    public void ResetTexture()
    {
        if ((targetTexture.width != originTex.width) &&
            (targetTexture.height != originTex.height))
        {
#if UNITY_EDITOR
            Debug.LogWarning("RenderTexture is different in size from origin. Check RenderTexture width and height.");
#endif
            return;
        }

        kernelCopy = paintShader.FindKernel(kernelCopyName);

        paintShader.SetTexture(kernelCopy, "Source", originTex);
        paintShader.SetTexture(kernelCopy, "Destination", targetTexture);

        paintShader.Dispatch(kernelCopy, threadGroupX, threadGroupY, 1);
        
        IsClear(false); // 초기화 했으므로 Clear -> false
#if UNITY_EDITOR
        Debug.Log("Reset Mask with Origin Texture.");
#endif
    }
    #endregion Texture Function


    #region Material Property
    private void SetBasicTwinkleProperties()
    {
        // Property 값 설정
        mr.material.SetFloat("_TwinkleIntensity", 4f); // 반짝임 색상 Intensity(세기)
        mr.material.SetFloat("_TwinkleSpeed", 4f); // 반짝임 속도
    }
    private void SetTwinkleProperties(bool _onlyDirty)
    {
        // Property 설정값 초기화
        Color color;
        if (_onlyDirty)
            color = new Color(1f, 0.6501361f, 0.2783019f, 1f);
        else
            color = new Color(0.4009433f, 0.5723213f, 1f, 1f);

        // Property 값 설정
        mr.material.SetFloat("_ActiveTwinkle", 1); // 반짝임 동작 여부
        mr.material.SetFloat("_OnlyDirty", _onlyDirty ? 1 : 0); // 오염 대상만인지 여부
        mr.material.SetColor("_TwinkleColor", color); // 색상
#if UNITY_EDITOR
        Debug.Log("[SetTwinkleProperties] Before Return");
#endif
    }
    private void StopTwinkleProperties()
    {
        // Property 값 설정
        mr.material.SetFloat("_ActiveTwinkle", 0); // 반짝임 동작 여부
        mr.material.SetFloat(timerName, 0f);
    }
    #endregion Material Property


    #region Utility
    // Mask Texture를 PNG로 저장하는 용도
    public void SaveToPNG(Texture2D _tex)
    {
        byte[] bytes = _tex.EncodeToPNG();

        StringBuilder savePath = new StringBuilder();
        savePath.Append(Application.dataPath);
        savePath.Append("/../");
        savePath.Append(_tex.name);
        savePath.Append(".png");

        File.WriteAllBytes(savePath.ToString(), bytes);
    }

    // RenderTexture를 Texture2D로 변환하여 반환
    private Texture2D ToTexture(RenderTexture _rTex)
    {
        Texture2D toTex = new Texture2D(resolution, resolution, TextureFormat.RGBA32, false);
        var oldTex = RenderTexture.active;
        RenderTexture.active = _rTex;

        toTex.ReadPixels(new Rect(0, 0, _rTex.width, _rTex.height), 0, 0);
        toTex.name = _rTex.name;
        toTex.Apply();
        RenderTexture.active = oldTex;

        return toTex;
    }
    #endregion


    #region Coroutine
    public void CompleteTwinkle()
    {
        if (isCompleteTwinkle == false)
        {
            isCompleteTwinkle = true;
            StopCoroutine("DirtyTwinkleCoroutine");
            StartCoroutine("CompleteTwinkleCoroutine");
        }
    }
    public void DirtyTwinkle()
    {
        if (isDirtyTwinkle == false && IsClear() == false)
        {
            isDirtyTwinkle = true;
            StartCoroutine("DirtyTwinkleCoroutine");
        }
    }


    private IEnumerator CompleteTwinkleCoroutine()
    {
        float time = 3f;
        float t = 0f;

        SetTwinkleProperties(false);
        mr.material.SetFloat(timerName, t);

        time /= mr.material.GetFloat(twinkleSpeed) * 0.5f;

        while (t < time)
        {
            t += Time.deltaTime;
            mr.material.SetFloat(timerName, t);
            yield return null;
        }

        StopTwinkleProperties();
        isDirtyTwinkle = false; // 안 되는게 맞긴 하지만 혹여나하여 넣음
        isCompleteTwinkle = false;
        IsClear(true);
    }
    private IEnumerator DirtyTwinkleCoroutine()
    {
        float time = 3f;
        float t = 0f;

        SetTwinkleProperties(true);
        mr.material.SetFloat(timerName, t);

        time /= mr.material.GetFloat(twinkleSpeed) * 0.5f;
        while (t < time)
        {
            t += Time.deltaTime;
            mr.material.SetFloat(timerName, t);
            yield return null;
        }

        StopTwinkleProperties();
        isDirtyTwinkle = false;
    }
    #endregion
}
