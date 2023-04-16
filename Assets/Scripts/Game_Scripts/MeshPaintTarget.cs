using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshPaintTarget : MonoBehaviour
{
    [SerializeField]
    private ComputeShader paintShader = null;
    [SerializeField]
    private ComputeShader countShader = null;

    private MeshRenderer mr = null; // 대상의 MeshRenderer
    private Material mainMat = null; // 대상의 MainTex가 있는 Material

    private Texture originUvTex = null; // UV원본
    private RenderTexture dirtyRTex = null; // 오염 Mask
    private RenderTexture wetRTex = null; // 젖은 효과를 내기 위한 Mask
    // 그릴 때 dirtyRTex와 wetRTex 둘 다 그림. 복사 X
    [SerializeField]
    private int resolution = 512;
    private int depth = 32;

    [SerializeField]
    private bool drawable = false;
    [SerializeField]
    private bool isClear = false;
    [SerializeField, Range(10, 100)]
    private int clearPercent = 90;
    [SerializeField]
    private float dryTimeMultiply = 1.5f;

    // Compute Shader //
    private int kernelNoise;
    private int kernelPaint;
    private int kernelWet;
    private int kernelDry;
    private int kernelCopy;
    private int kernelClear;

    private int kernelInitCount;
    private int kernelCount;

    private readonly string kernelNoiseName = "CSNoise";
    private readonly string kernelPaintName = "CSPaint";
    private readonly string kernelWetName = "CSWet";
    private readonly string kernelDryName = "CSDry";
    private readonly string kernelCopyName = "CSCopy";
    private readonly string kernelClearName = "CSClear";

    private readonly string kernelInitCountName = "CSInitCount";
    private readonly string kernelCountName = "CSCount";

    private int threadGroupX = -1;
    private int threadGroupY = -1;
    // End Compute Shader //

    // Material Properties //
    // -MeshPaint
    private readonly string dirtyUv = "_PaintUv";
    private readonly string dirtyMask = "_PaintMask";
    // -Twinkle
    private readonly string timerName = "_Timer";
    private readonly string twinkleSpeed = "_TwinkleSpeed";
    private bool isCompleteTwinkle { get; set; }
    private bool isDirtyTwinkle { get; set; }
    // End Material Properties //


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

    public Texture2D GetPaintMask()
    {
        return ToTexture(dirtyRTex);
    }
    private void SetPaintMask(Texture2D _tex)
    {
        if (dirtyRTex == null)
        {
            dirtyRTex = GenerateRenderTexture(resolution, resolution);
            dirtyRTex.enableRandomWrite = true;
        }

        Graphics.Blit(_tex, dirtyRTex);
        mainMat.SetTexture(dirtyMask, dirtyRTex);
    }
    #endregion


    private void Awake()
    {

        // MeshRenderer일수도 SkinnedMeshRenderer일수도 있으므로 그냥 받는 변수의 타입에 구애받도록 제네릭을 쓰지 않음
        if (TryGetComponent(out mr))
        {
            mainMat = mr.material;
        }
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        // 그릴 수 있는 대상이고 클리어가 되지 않았다면 남은 구역을 보여줄 수 있도록 함
        if (IsDrawable() && !IsClear() && Input.GetKeyDown(KeyCode.Tab))
        {
            DirtyTwinkle();
        }
    }


    private void Init()
    {
        // _MainTex: 모델링의 Texture
        // _WetMask: 젖은 효과를 위한 Mask (default: none). 코드상에서 추가

        // _PaintTex: 오염 Texture. 코드상에서 추가
        // _PaintUv: 원본 UV Texture
        // _PaintMask: 오염 Mask (default: none). 코드상에서 추가

        if (mainMat != null)
        {
            originUvTex = mainMat.GetTexture(dirtyUv);

            if (threadGroupX == -1)
                threadGroupX = Mathf.CeilToInt(originUvTex.width / 8);
            if (threadGroupY == -1)
                threadGroupY = Mathf.CeilToInt(originUvTex.height / 8);

            // 임시 //
            Texture sampleTex = null;
            sampleTex = mainMat.GetTexture(dirtyMask);
            // Texture가 없다면 생성해서 넣어줍니다.
            if (sampleTex == null)
            {
#if UNITY_EDITOR
                //Debug.Log("[MeshPainterTarget] Object Name: " + gameObject.name);
#endif
                // 오염 텍스쳐를 생성하고 원본 UV를 복사함
                dirtyRTex = GenerateRenderTexture(originUvTex.width, originUvTex.height);
                dirtyRTex.name = gameObject.name;
                dirtyRTex.enableRandomWrite = true; // Graphics.Blit을 하기 전에 접근할 수 있게 설정해줘야 적용됨
                Graphics.Blit(originUvTex, dirtyRTex); // Texture를 RenderTexture에 복사

                mainMat.SetTexture(dirtyMask, dirtyRTex);
            }
            // End 임시 //

            // !!현재 노이즈 텍스쳐는 생성을 하지만 셰이더에서 사용하고 있지는 않음!!
            // *Compute Shader에서 생성하는데 자연스럽게 뽑아내기 전까지는 Shader Graph의 노이즈를 사용
            SetNoiseTexture(mainMat);
            SetBasicTwinkleProperties(mainMat);


            // 젖은 텍스쳐를 위한 알파값이 0인 빈 텍스쳐를 가져와서 복사함
            Texture2D tex = GenerateTexture2D(resolution, resolution);
            tex.LoadImage(Resources.Load<Texture2D>("Textures/Utility/Empty").EncodeToPNG());
            tex.Apply();

            wetRTex = GenerateRenderTexture(originUvTex.width, originUvTex.height);
            wetRTex.name = "WetMask";
            wetRTex.enableRandomWrite = true;
            Graphics.Blit(tex, wetRTex);

            mainMat.SetTexture("_WetMask", wetRTex);
        }
    }


    #region Draw Function
    // RenderTexture에 Paint Rendering을 함
    /// <summary>
    /// Dirty Texture를 지우는 Mask에 그립니다.
    /// </summary>
    /// <param name="_drawable">bool | 그리는 중인지 여부</param>
    /// <param name="_uvPos">Vector2 | UV상 위치 값</param>
    /// <param name="_color">Color(Vector4) | RGB로 구분 적용할 값</param>
    /// <param name="_size">float | 그리는 크기</param>
    /// <param name="_distance">float | 대상과의 거리 차이</param>
    public void DrawRender(bool _drawable, Vector2 _uvPos, Color _color, float _size, float _distance)
    {
        if (IsDrawable() == false || IsClear() == true) return;
        // Brush의 Texture를 받아서 사용하고자 하였으나 문제가 발생하여 지금은 사용하지 않음
#if UNITY_EDITOR
        Debug.Log("DrawRender");
#endif
        /*
            RWTexture2D<float4> PaintMask;
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
        // 초과 값에 대해 보정
        Vector4 color = new Vector4(
            _color.r > 1f ? 1f : _color.r,
            _color.g > 1f ? 1f : _color.g,
            _color.b > 1f ? 1f : _color.b,
            0f);

        // 3) 설정을 끝마쳤다면 shader에 넘김
        paintShader.SetTexture(kernelPaint, "Result", dirtyRTex); // Target의 Dirty RenderTexture
        paintShader.SetVector("UvPos", uvPos);
        paintShader.SetVector("Color", color); // Brush의 처리값
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
        Graphics.Blit(dirtyRTex, dirtyRTex);

        // 6) 반환할 것이 있다면 함수의 반환형을 변경 후 반환
        // return;
    }


    /// <summary>
    /// WetMask에 그려서 젖은 효과를 나타냅니다.
    /// </summary>
    /// <param name="_drawable">bool | 그리는 중인지 여부</param>
    /// <param name="_uvPos">Vector2 | UV상 위치 값</param>
    /// <param name="_size">float | 그리는 크기</param>
    /// <param name="_distance">float | 대상과의 거리 차이</param>
    public void DrawWet(bool _drawable, Vector2 _uvPos, float _size, float _distance)
    {
        // 그릴 수 있는 대상에만 젖는 효과 발생
        if (IsDrawable() == false) return;

#if UNITY_EDITOR
        Debug.Log("DrawWet");
#endif
        /*
            RWTexture2D<float4> WetMask;
            float2 UvPos;
            float Size;
            float Distance;
            bool Drawable;
        */

        // 1) Kernel을 가져옴
        kernelWet = paintShader.FindKernel(kernelWetName);

        // 2) 초기화가 필요한 경우 여기서 초기화
        // ex) computeBuffer = new ComputeBuffer[count, sizeof(typeof) * cnt]; (uint4) => cnt: 4
        // uvPos는 화면상에서의 비율이므로 해상도 값을 곱함
        Vector2 uvPos = new Vector2((uint)Mathf.CeilToInt(_uvPos.x * resolution), (uint)Mathf.CeilToInt(_uvPos.y * resolution));

        // 3) 설정을 끝마쳤다면 shader에 넘김
        paintShader.SetTexture(kernelWet, "WetMask", wetRTex); // Target의 Wet RenderTexture
        paintShader.SetVector("UvPos", uvPos);
        paintShader.SetFloat("Size", _size); // Brush의 사이즈
        paintShader.SetFloat("Distance", _distance); // 최대사거리 / 충돌거리
        paintShader.SetBool("Paintable", _drawable); // 그릴 수 있는지 여부

        // 4) 필요한 것을 다 넘겼다면 shader 실행
        // 현재 Shader는 numthreads(8, 8, 1)이면 shader.Dispatch(kernel, width / 8, height / 8, 1);
        paintShader.Dispatch(kernelWet, threadGroupX, threadGroupY, 1);
#if UNITY_EDITOR
        Debug.Log("Shader Dispatch");
#endif

        // 5) 처리된 정보를 가공하는 부분
        // Buffer를 보냈었다면 Data를 가져오고 Release 후 null 처리
        Graphics.Blit(wetRTex, wetRTex);

        // 6) 반환할 것이 있다면 함수의 반환형을 변경 후 반환
        // return;
        DryWetting();
    }
    #endregion Draw Function


    #region Pixel Counter
    /// <summary>
    /// 현재 진행도와 clearPercent를 비교하여 진행도가 clearPercent에 도달하면 Clear처리를 합니다.
    /// </summary>
    public void CheckAutoClear()
    {
        if (IsClear() == true) return;

#if UNITY_EDITOR
        //Debug.LogFormat("current: {0}, Percent: {1}", GetProcessPercent(), GetPercent());
#endif
        if (clearPercent < GetProcessPercent())
        {
            ClearTexture();
            CompleteTwinkle();
        }
    }


    /// <summary>
    /// clearPercent를 기초로 하여 진행 백분율을 제공합니다.
    /// </summary>
    /// <returns>float | ex) 50.12446...</returns>
    public float GetPercent()
    {
        float current = GetProcessPercent();
        return current * 100 / clearPercent;
    }


    /// <summary>
    /// 원본 UV를 이용해서 총 픽셀 수를 구하고 오염 Texture에서 남은 픽셀 수를 계산하여 백분율을 반환합니다.
    /// </summary>
    /// <returns>float | ex) 50.12446...</returns>
    public float GetProcessPercent()
    {
        if (originUvTex == null || dirtyRTex == null) return 0f;

        uint origin = PixelCount(originUvTex);
        uint target = PixelCount(dirtyRTex);

        float percent = (origin - target) / (float)origin * 100;
#if UNITY_EDITOR
        //Debug.LogFormat("[CheckPercent] origin: {0}, target_: {1}, percent: {2}", origin, target_, percent);
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
        ComputeBuffer buffer = new ComputeBuffer(1, sizeof(uint) * 1); // size_ * count | uint1을 사용할 것이므로 1만 곱함
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
        return data[0];
    }
    #endregion Pixel Counter


    #region Texture Function (Clear, Reset)
    public void ClearTexture()
    {
        if ((dirtyRTex.width != originUvTex.width) &&
            (dirtyRTex.height != originUvTex.height))
        {
#if UNITY_EDITOR
            Debug.LogWarning("RenderTexture is different in size from origin. Check RenderTexture width and height.");
#endif
            return;
        }

        kernelClear = paintShader.FindKernel(kernelClearName);

        paintShader.SetTexture(kernelClear, "Result", dirtyRTex);

        paintShader.Dispatch(kernelClear, threadGroupX, threadGroupY, 1);
#if UNITY_EDITOR
        Debug.Log("Dirty Clear.");
#endif
    }

    // 원본UV를 RenderTexture에 복사
    public void ResetTexture()
    {
        if ((dirtyRTex.width != originUvTex.width) &&
            (dirtyRTex.height != originUvTex.height))
        {
#if UNITY_EDITOR
            Debug.LogWarning("RenderTexture is different in size from origin. Check RenderTexture width and height.");
#endif
            return;
        }

        kernelCopy = paintShader.FindKernel(kernelCopyName);

        paintShader.SetTexture(kernelCopy, "Source", originUvTex);
        paintShader.SetTexture(kernelCopy, "Destination", dirtyRTex);

        paintShader.Dispatch(kernelCopy, threadGroupX, threadGroupY, 1);

        IsClear(false); // 초기화 했으므로 Clear -> false
#if UNITY_EDITOR
        Debug.Log("Reset Mask with Origin Texture.");
#endif
    }

    private void DryWetting(Material _mat, float _t = 0f)
    {
#if UNITY_EDITOR
        //Debug.LogFormat("_t : {0}", _t);
#endif
        kernelDry = paintShader.FindKernel(kernelDryName);

        paintShader.SetFloat("Dry", _t);
        paintShader.SetTexture(kernelDry, "WetMask", wetRTex);

        paintShader.Dispatch(kernelDry, resolution / 8, resolution / 8, 1);

        Graphics.Blit(wetRTex, wetRTex); // 갱신을 하기 위함
    }
    #endregion Texture Function


    #region Material Property
    private void SetBasicTwinkleProperties(Material _mat)
    {
        // Property 값 설정
        _mat.SetFloat("_TwinkleIntensity", 4f); // 반짝임 색상 Intensity(세기)
    }
    private void SetTwinkleProperties(bool _onlyDirty, Material _mat)
    {
        // Property 설정값 초기화
        Color color;
        if (_onlyDirty)
            color = new Color(1f, 0.6501361f, 0.2783019f, 1f);
        else
            color = new Color(0.4009433f, 0.5723213f, 1f, 1f);

        // Property 값 설정
        _mat.SetFloat("_ActiveTwinkle", 1); // 반짝임 동작 여부
        _mat.SetFloat("_OnlyDirty", _onlyDirty ? 1 : 0); // 오염 대상만인지 여부
        _mat.SetFloat("_TwinkleSpeed", _onlyDirty ? 3f : 4f); // 반짝임 속도
        _mat.SetColor("_TwinkleColor", color); // 색상
#if UNITY_EDITOR
        Debug.Log("[SetTwinkleProperties] Before Return");
#endif
    }
    private void StopTwinkleProperties(Material _mat)
    {
        // Property 값 설정
        _mat.SetFloat("_ActiveTwinkle", 0); // 반짝임 동작 여부
        _mat.SetFloat(timerName, 0f);
    }
    #endregion Material Property


    #region Utility
    /// <summary>
    /// 진행중인 Mask를 png 형식으로 저장합니다.
    /// </summary>
    public void SaveMask()
    {
        if (dirtyRTex == null) return;

        SaveToPNG(ToTexture(dirtyRTex), GetPath());
    }

    /// <summary>
    /// 진행되었던 Mask를 가져옵니다.
    /// </summary>
    /// <returns>bool | 가져왔는지 여부</returns>
    public bool LoadMask()
    {
        bool load = true;
        string path = GetPath();

        StringBuilder fileName = new StringBuilder();
        fileName.Append(gameObject.name);
        fileName.Append(".png");

        byte[] bytes = FileIO.GetFileBinary(path, fileName.ToString());


        if (bytes == null || bytes.Length <= 0) return load = false;

        Texture2D tex = GenerateTexture2D(resolution, resolution);
        tex.LoadImage(bytes);
        tex.Apply();

        SetPaintMask(tex);

        return load;
    }

    // Mask Texture를 PNG로 저장하는 용도
    private void SaveToPNG(Texture2D _tex, string _path)
    {
        byte[] bytes = _tex.EncodeToPNG();

        StringBuilder fileName = new StringBuilder();
        fileName.Append(gameObject.name);
        fileName.Append(".png");
        string savePath = Path.Combine(_path, fileName.ToString());

        File.WriteAllBytes(savePath.ToString(), bytes);
    }

    // RenderTexture를 Texture2D로 변환하여 반환
    private Texture2D ToTexture(RenderTexture _rTex)
    {
        Texture2D toTex = GenerateTexture2D(resolution, resolution);
        var oldTex = RenderTexture.active;
        RenderTexture.active = _rTex;

        toTex.ReadPixels(new Rect(0, 0, _rTex.width, _rTex.height), 0, 0);
        toTex.name = _rTex.name;
        toTex.Apply();
        RenderTexture.active = oldTex;

        return toTex;
    }

    /// <summary>
    /// gameObject의 name에 있는 정보를 이용하여 Path를 가져옵니다.
    /// </summary>
    /// <returns></returns>
    private string GetPath()
    {
        // gameObject Name을 이용하여 불러오는 방식
        // ex) Fence_1_2_3 | 1: Map, 2: Section, 3: Numbering
        string[] split = gameObject.name.Split('_');
        int len = split.Length;
        int mapNum = int.Parse(split[len - 3]) - 1;
        int sectionNum = int.Parse(split[len - 2]) - 1;

        string path = FilePath.GetPath(FilePath.EPathType.EXTERNAL, (FilePath.EMapType)mapNum, (FilePath.ESection)sectionNum);

        return path;
    }

    /// <summary>
    /// 오염Texture를 생성하고 Material Property의 _PaintTex에 탑재합니다.
    /// </summary>
    /// <param name="_mat">생성한 Texture를 담을 Material</param>
    private void SetNoiseTexture(Material _mat)
    {
        RenderTexture rTex = new RenderTexture(resolution, resolution, depth, RenderTextureFormat.ARGB64, RenderTextureReadWrite.Linear);
        rTex.enableRandomWrite = true;
        rTex.Create();

        kernelNoise = paintShader.FindKernel(kernelNoiseName);

        paintShader.SetTexture(kernelNoise, "Result", rTex);

        paintShader.Dispatch(kernelNoise, threadGroupX, threadGroupY, 1);
#if UNITY_EDITOR
        Debug.Log("Make Noise Texture");
#endif
        Graphics.Blit(rTex, rTex);

        _mat.SetTexture("_PaintTex", rTex);
    }

    private Texture2D GenerateTexture2D(int _width, int _height)
    {
        return new Texture2D(_width, _height, TextureFormat.RGBA32, false);
    }
    private RenderTexture GenerateRenderTexture(int _width, int _height)
    {
        return new RenderTexture(_width, _height, depth, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
    }
    #endregion


    #region Coroutine
    private void DryWetting()
    {
        StopDryWetting();
        StartCoroutine("DryWettingCoroutine");
    }
    private void StopDryWetting()
    {
        StopCoroutine("DryWettingCoroutine");
    }

    public void CompleteTwinkle()
    {
        if (isCompleteTwinkle == false)
        {
            isCompleteTwinkle = true;
            StopCoroutine("DirtyTwinkleCoroutine");
            if (mainMat != null)
                StartCoroutine("CompleteTwinkleCoroutine", mainMat);
        }
    }
    public void DirtyTwinkle()
    {
        if (isDirtyTwinkle == false && IsClear() == false)
        {
            isDirtyTwinkle = true;
            if (mainMat != null)
                StartCoroutine("DirtyTwinkleCoroutine", mainMat);
        }
    }

    private IEnumerator DryWettingCoroutine()
    {
        yield return new WaitForSeconds(0.01f);
#if UNITY_EDITOR
        Debug.Log("StartCoroutine");
#endif
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            DryWetting(mainMat, Time.deltaTime * dryTimeMultiply);
            yield return new WaitForSeconds(0.05f);
        }
#if UNITY_EDITOR
        Debug.Log("EndCoroutine");
#endif
        StopDryWetting();
    }

    private IEnumerator CompleteTwinkleCoroutine(Material _mat)
    {
        float time = _mat.GetFloat(twinkleSpeed);
        float t = 0f;

        SetTwinkleProperties(_onlyDirty: false, _mat);
        _mat.SetFloat(timerName, t);

        time /= time;

        while (t < time)
        {
            t += Time.deltaTime;
            _mat.SetFloat(timerName, t);
            yield return null;
        }

        StopTwinkleProperties(_mat);
        isDirtyTwinkle = false; // 안 되는게 맞긴 하지만 혹여나하여 넣음
        isCompleteTwinkle = false;
        IsClear(true);
    }
    private IEnumerator DirtyTwinkleCoroutine(Material _mat)
    {
        float time = _mat.GetFloat(twinkleSpeed);
        float t = -0.1f;

        SetTwinkleProperties(_onlyDirty: true, _mat);
        _mat.SetFloat(timerName, t);

        time /= time;

        while (t < time)
        {
            t += Time.deltaTime * 0.66f;
            _mat.SetFloat(timerName, t);
            yield return null;
        }

        StopTwinkleProperties(_mat);
        isDirtyTwinkle = false;
    }
    #endregion
}
