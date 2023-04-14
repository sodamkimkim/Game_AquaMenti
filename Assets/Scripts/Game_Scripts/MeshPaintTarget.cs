using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MeshPaintTarget : MonoBehaviour
{
    [SerializeField]
    private ComputeShader paintShader = null;
    [SerializeField]
    private ComputeShader countShader = null;

    private MeshRenderer mr = null; // ����� MeshRenderer
    private Material mainMat = null; // ����� MainTex�� �ִ� Material

    private Texture originUvTex = null; // UV����
    private RenderTexture dirtyRTex = null; // ���� Mask
    private RenderTexture wetRTex = null; // ���� ȿ���� ���� ���� Mask
    // �׸� �� dirtyRTex�� wetRTex �� �� �׸�. ���� X
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

        // MeshRenderer�ϼ��� SkinnedMeshRenderer�ϼ��� �����Ƿ� �׳� �޴� ������ Ÿ�Կ� ���ֹ޵��� ���׸��� ���� ����
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
        // �׸� �� �ִ� ����̰� Ŭ��� ���� �ʾҴٸ� ���� ������ ������ �� �ֵ��� ��
        if (IsDrawable() && !IsClear() && Input.GetKeyDown(KeyCode.Tab))
        {
            DirtyTwinkle();
        }
    }


    private void Init()
    {
        // _MainTex: �𵨸��� Texture
        // _WetMask: ���� ȿ���� ���� Mask (default: none). �ڵ�󿡼� �߰�

        // _PaintTex: ���� Texture. �ڵ�󿡼� �߰�
        // _PaintUv: ���� UV Texture
        // _PaintMask: ���� Mask (default: none). �ڵ�󿡼� �߰�

        if (mainMat != null)
        {
            originUvTex = mainMat.GetTexture(dirtyUv);

            if (threadGroupX == -1)
                threadGroupX = Mathf.CeilToInt(originUvTex.width / 8);
            if (threadGroupY == -1)
                threadGroupY = Mathf.CeilToInt(originUvTex.height / 8);

            // �ӽ� //
            Texture sampleTex = null;
            sampleTex = mainMat.GetTexture(dirtyMask);
            // Texture�� ���ٸ� �����ؼ� �־��ݴϴ�.
            if (sampleTex == null)
            {
#if UNITY_EDITOR
                //Debug.Log("[MeshPainterTarget] Object Name: " + gameObject.name);
#endif
                // ���� �ؽ��ĸ� �����ϰ� ���� UV�� ������
                dirtyRTex = GenerateRenderTexture(originUvTex.width, originUvTex.height);
                dirtyRTex.name = gameObject.name;
                dirtyRTex.enableRandomWrite = true; // Graphics.Blit�� �ϱ� ���� ������ �� �ְ� ��������� �����
                Graphics.Blit(originUvTex, dirtyRTex); // Texture�� RenderTexture�� ����

                mainMat.SetTexture(dirtyMask, dirtyRTex);
            }
            // End �ӽ� //

            // !!���� ������ �ؽ��Ĵ� ������ ������ ���̴����� ����ϰ� ������ ����!!
            // *Compute Shader���� �����ϴµ� �ڿ������� �̾Ƴ��� �������� Shader Graph�� ����� ���
            SetNoiseTexture(mainMat);
            SetBasicTwinkleProperties(mainMat);


            // ���� �ؽ��ĸ� ���� ���İ��� 0�� �� �ؽ��ĸ� �����ͼ� ������
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
    // RenderTexture�� Paint Rendering�� ��
    /// <summary>
    /// Dirty Texture�� ����� Mask�� �׸��ϴ�.
    /// </summary>
    /// <param name="_drawable">bool | �׸��� ������ ����</param>
    /// <param name="_uvPos">Vector2 | UV�� ��ġ ��</param>
    /// <param name="_color">Color(Vector4) | RGB�� ���� ������ ��</param>
    /// <param name="_size">float | �׸��� ũ��</param>
    /// <param name="_distance">float | ������ �Ÿ� ����</param>
    public void DrawRender(bool _drawable, Vector2 _uvPos, Color _color, float _size, float _distance)
    {
        if (IsDrawable() == false || IsClear() == true) return;
        // Brush�� Texture�� �޾Ƽ� ����ϰ��� �Ͽ����� ������ �߻��Ͽ� ������ ������� ����
#if UNITY_EDITOR
        //Debug.Log("DrawRender");
#endif
        /*
            RWTexture2D<float4> PaintMask;
            float2 UvPos;
            float4 Color;
            float Size;
            float Distance;
            bool Drawable;
        */

        // 1) Kernel�� ������
        kernelPaint = paintShader.FindKernel(kernelPaintName);

        // 2) �ʱ�ȭ�� �ʿ��� ��� ���⼭ �ʱ�ȭ
        // ex) computeBuffer = new ComputeBuffer[count, sizeof(typeof) * cnt]; (uint4) => cnt: 4
        // uvPos�� ȭ��󿡼��� �����̹Ƿ� �ػ� ���� ����
        Vector2 uvPos = new Vector2((uint)Mathf.CeilToInt(_uvPos.x * resolution), (uint)Mathf.CeilToInt(_uvPos.y * resolution));
        // �ʰ� ���� ���� ����
        Vector4 color = new Vector4(
            _color.r > 1f ? 1f : _color.r,
            _color.g > 1f ? 1f : _color.g,
            _color.b > 1f ? 1f : _color.b,
            0f);

        // 3) ������ �����ƴٸ� shader�� �ѱ�
        paintShader.SetTexture(kernelPaint, "Result", dirtyRTex); // Target�� Dirty RenderTexture
        paintShader.SetVector("UvPos", uvPos);
        paintShader.SetVector("Color", color); // Brush�� ó����
        paintShader.SetFloat("Size", _size); // Brush�� ������
        paintShader.SetFloat("Distance", _distance); // �ִ��Ÿ� / �浹�Ÿ�
        paintShader.SetBool("Paintable", _drawable); // �׸� �� �ִ��� ����

        // 4) �ʿ��� ���� �� �Ѱ�ٸ� shader ����
        // ���� Shader�� numthreads(8, 8, 1)�̸� shader.Dispatch(kernel, width / 8, height / 8, 1);
        paintShader.Dispatch(kernelPaint, threadGroupX, threadGroupY, 1);
#if UNITY_EDITOR
        //Debug.Log("Shader Dispatch");
#endif

        // 5) ó���� ������ �����ϴ� �κ�
        // Buffer�� ���¾��ٸ� Data�� �������� Release �� null ó��
        Graphics.Blit(dirtyRTex, dirtyRTex);

        // 6) ��ȯ�� ���� �ִٸ� �Լ��� ��ȯ���� ���� �� ��ȯ
        // return;
    }


    /// <summary>
    /// WetMask�� �׷��� ���� ȿ���� ��Ÿ���ϴ�.
    /// </summary>
    /// <param name="_drawable">bool | �׸��� ������ ����</param>
    /// <param name="_uvPos">Vector2 | UV�� ��ġ ��</param>
    /// <param name="_size">float | �׸��� ũ��</param>
    /// <param name="_distance">float | ������ �Ÿ� ����</param>
    public void DrawWet(bool _drawable, Vector2 _uvPos, float _size, float _distance)
    {
        // �׸� �� �ִ� ��󿡸� ���� ȿ�� �߻�
        if (IsDrawable() == false) return;

#if UNITY_EDITOR
        //Debug.Log("DrawWet");
#endif
        /*
            RWTexture2D<float4> WetMask;
            float2 UvPos;
            float Size;
            float Distance;
            bool Drawable;
        */

        // 1) Kernel�� ������
        kernelWet = paintShader.FindKernel(kernelWetName);

        // 2) �ʱ�ȭ�� �ʿ��� ��� ���⼭ �ʱ�ȭ
        // ex) computeBuffer = new ComputeBuffer[count, sizeof(typeof) * cnt]; (uint4) => cnt: 4
        // uvPos�� ȭ��󿡼��� �����̹Ƿ� �ػ� ���� ����
        Vector2 uvPos = new Vector2((uint)Mathf.CeilToInt(_uvPos.x * resolution), (uint)Mathf.CeilToInt(_uvPos.y * resolution));

        // 3) ������ �����ƴٸ� shader�� �ѱ�
        paintShader.SetTexture(kernelWet, "WetMask", wetRTex); // Target�� Wet RenderTexture
        paintShader.SetVector("UvPos", uvPos);
        paintShader.SetFloat("Size", _size); // Brush�� ������
        paintShader.SetFloat("Distance", _distance); // �ִ��Ÿ� / �浹�Ÿ�
        paintShader.SetBool("Paintable", _drawable); // �׸� �� �ִ��� ����

        // 4) �ʿ��� ���� �� �Ѱ�ٸ� shader ����
        // ���� Shader�� numthreads(8, 8, 1)�̸� shader.Dispatch(kernel, width / 8, height / 8, 1);
        paintShader.Dispatch(kernelWet, threadGroupX, threadGroupY, 1);
#if UNITY_EDITOR
        //Debug.Log("Shader Dispatch");
#endif

        // 5) ó���� ������ �����ϴ� �κ�
        // Buffer�� ���¾��ٸ� Data�� �������� Release �� null ó��
        Graphics.Blit(wetRTex, wetRTex);

        // 6) ��ȯ�� ���� �ִٸ� �Լ��� ��ȯ���� ���� �� ��ȯ
        // return;
        DryWetting();
    }

    #endregion Draw Function

    #region Pixel Counter
    /// <summary>
    /// ���� ���൵�� clearPercent�� ���Ͽ� ���൵�� clearPercent�� �����ϸ� Clearó���� �մϴ�.
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
    /// clearPercent�� ���ʷ� �Ͽ� ���� ������� �����մϴ�.
    /// </summary>
    /// <returns>float | ex) 50.12446...</returns>
    public float GetPercent()
    {
        float current = GetProcessPercent();
        return current * 100 / clearPercent;
    }


    /// <summary>
    /// ���� UV�� �̿��ؼ� �� �ȼ� ���� ���ϰ� ���� Texture���� ���� �ȼ� ���� ����Ͽ� ������� ��ȯ�մϴ�.
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
        // 1) Kernel�� ������
        kernelInitCount = countShader.FindKernel(kernelInitCountName);
        kernelCount = countShader.FindKernel(kernelCountName);

        // 2) �ʱ�ȭ�� �ʿ��� ��� ���⼭ �ʱ�ȭ
        // ex) computeBuffer = new ComputeBuffer[count, sizeof(typeof) * cnt]; (uint4) => cnt: 4
        ComputeBuffer buffer = new ComputeBuffer(1, sizeof(uint) * 1); // size_ * count | uint1�� ����� ���̹Ƿ� 1�� ����
        uint[] data = new uint[1];

        // 3) ������ �����ƴٸ� shader�� �ѱ�
        countShader.SetTexture(kernelCount, "InputTexture", _tex); // ComputeShader�� �̹����� ����
        countShader.SetBuffer(kernelCount, "CountBuffer", buffer); // Buffer�� ����
        countShader.SetBuffer(kernelInitCount, "CountBuffer", buffer);

        // 4) �ʿ��� ���� �� �Ѱ�ٸ� shader ����
        // ���� Shader�� numthreads(8, 8, 1)�̸� shader.Dispatch(kernel, width / 8, height / 8, 1);
        countShader.Dispatch(kernelInitCount, 1, 1, 1);
        countShader.Dispatch(kernelCount, threadGroupX, threadGroupY, 1);

        // 5) ó���� ������ �����ϴ� �κ�
        // Buffer�� ���¾��ٸ� Data�� �������� Release �� null ó��
        buffer.GetData(data);
        buffer.Release();
        buffer = null;

        // 6) ��ȯ�� ���� �ִٸ� �Լ��� ��ȯ���� ���� �� ��ȯ
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
        //Debug.Log("Dirty Clear.");
#endif
    }

    // ����UV�� RenderTexture�� ����
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

        IsClear(false); // �ʱ�ȭ �����Ƿ� Clear -> false
#if UNITY_EDITOR
        //Debug.Log("Reset Mask with Origin Texture.");
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

        Graphics.Blit(wetRTex, wetRTex); // ������ �ϱ� ����
    }
    #endregion Texture Function


    #region Material Property
    private void SetBasicTwinkleProperties(Material _mat)
    {
        // Property �� ����
        _mat.SetFloat("_TwinkleIntensity", 4f); // ��¦�� ���� Intensity(����)
    }
    private void SetTwinkleProperties(bool _onlyDirty, Material _mat)
    {
        // Property ������ �ʱ�ȭ
        Color color;
        if (_onlyDirty)
            color = new Color(1f, 0.6501361f, 0.2783019f, 1f);
        else
            color = new Color(0.4009433f, 0.5723213f, 1f, 1f);

        // Property �� ����
        _mat.SetFloat("_ActiveTwinkle", 1); // ��¦�� ���� ����
        _mat.SetFloat("_OnlyDirty", _onlyDirty ? 1 : 0); // ���� ������� ����
        _mat.SetFloat("_TwinkleSpeed", _onlyDirty ? 3f : 4f); // ��¦�� �ӵ�
        _mat.SetColor("_TwinkleColor", color); // ����
#if UNITY_EDITOR
        //Debug.Log("[SetTwinkleProperties] Before Return");
#endif
    }
    private void StopTwinkleProperties(Material _mat)
    {
        // Property �� ����
        _mat.SetFloat("_ActiveTwinkle", 0); // ��¦�� ���� ����
        _mat.SetFloat(timerName, 0f);
    }
    #endregion Material Property


    #region Utility
    /// <summary>
    /// �������� Mask�� png �������� �����մϴ�.
    /// </summary>
    public void SaveMask()
    {
        if (dirtyRTex == null) return;

        SaveToPNG(ToTexture(dirtyRTex), GetPath());
    }

    /// <summary>
    /// ����Ǿ��� Mask�� �����ɴϴ�.
    /// </summary>
    /// <returns>bool | �����Դ��� ����</returns>
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

    // Mask Texture�� PNG�� �����ϴ� �뵵
    private void SaveToPNG(Texture2D _tex, string _path)
    {
        byte[] bytes = _tex.EncodeToPNG();

        StringBuilder fileName = new StringBuilder();
        fileName.Append(gameObject.name);
        fileName.Append(".png");
        string savePath = Path.Combine(_path, fileName.ToString());

        File.WriteAllBytes(savePath.ToString(), bytes);
    }

    // RenderTexture�� Texture2D�� ��ȯ�Ͽ� ��ȯ
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
    /// gameObject�� name�� �ִ� ������ �̿��Ͽ� Path�� �����ɴϴ�.
    /// </summary>
    /// <returns></returns>
    private string GetPath()
    {
        // gameObject Name�� �̿��Ͽ� �ҷ����� ���
        // ex) Fence_1_2_3 | 1: Map, 2: Section, 3: Numbering
        string[] split = gameObject.name.Split('_');
        int len = split.Length;
        int mapNum = int.Parse(split[len - 3]) - 1;
        int sectionNum = int.Parse(split[len - 2]) - 1;

        string path = FilePath.GetPath(FilePath.EPathType.EXTERNAL, (FilePath.EMapType)mapNum, (FilePath.ESection)sectionNum);

        return path;
    }

    /// <summary>
    /// ����Texture�� �����ϰ� Material Property�� _PaintTex�� ž���մϴ�.
    /// </summary>
    /// <param name="_mat">������ Texture�� ���� Material</param>
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
        //Debug.Log("StartCoroutine");
#endif
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            DryWetting(mainMat, Time.deltaTime * dryTimeMultiply);
            yield return new WaitForSeconds(0.05f);
        }
#if UNITY_EDITOR
        //Debug.Log("EndCoroutine");
#endif
        StopDryWetting();
    }

    private IEnumerator CompleteTwinkleCoroutine(Material _mat)
    {
        float time = 3f;
        float t = 0f;

        SetTwinkleProperties(_onlyDirty: false, _mat);
        _mat.SetFloat(timerName, t);

        time /= _mat.GetFloat(twinkleSpeed) * 0.5f;

        while (t < time)
        {
            t += Time.deltaTime;
            _mat.SetFloat(timerName, t);
            yield return null;
        }

        StopTwinkleProperties(_mat);
        isDirtyTwinkle = false; // �� �Ǵ°� �±� ������ Ȥ�����Ͽ� ����
        isCompleteTwinkle = false;
        IsClear(true);
    }
    private IEnumerator DirtyTwinkleCoroutine(Material _mat)
    {
        float time = 3f;
        float t = 0f;

        SetTwinkleProperties(_onlyDirty: true, _mat);
        _mat.SetFloat(timerName, t);

        time /= _mat.GetFloat(twinkleSpeed) * 0.5f;

        while (t < time)
        {
            t += Time.deltaTime;
            _mat.SetFloat(timerName, t);
            yield return null;
        }

        StopTwinkleProperties(_mat);
        isDirtyTwinkle = false;
    }
    #endregion
}
