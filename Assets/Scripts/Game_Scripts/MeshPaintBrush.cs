using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshPaintBrush : MonoBehaviour
{
    private Collider prevCollider;
    private Vector2 uvPos = Vector2.zero;

    private MeshPaintTarget target = null;

    [SerializeField, Range(0.5f, 10f)]
    private float effectiveDistance = 1f; // 유효 사거리
    [SerializeField, Range(1f, 16f)]
    private float maxDistance = 2f; // 최대 사거리
    private bool effective = false;

    // Option
    [SerializeField, Range(1, 20)]
    private int size = 10;
    private Vector4 color = new Vector4(0f, 0f, 0f, 1f);
    private bool isPainting = false;

    private float drawTiming = 0.01f;
    private float waitTime = 0.1f;

    private bool drawCoroutine = false;
    private bool runCoroutine = false;


    // 임시
    private enum MagicType { Zero, One, Two, Three, Four }

    private struct DirtyLv // 오염타입
    {
        public int rLv { get; set; } // 표면
        public int gLv { get; set; } // 뒤덮힘
        public int bLv { get; set; } // 억셈
    }

    private struct Stick
    {
        public DirtyLv cleanLv;
        public MagicType magicType; // 마법 => Nozzle
    }

    // 임시
    // R: 표면, G: 뒤덮힘, B: 억셈
    //private int rDirtyLv = 5;
    //private int rCleanLv = 3;
    //private int magicType = 2; // Nozzle. Maybe Enum.
    private Stick stick;
    private DirtyLv dirty;


    private void Start()
    {
        // 지팡이 스펙 설정
        stick = new Stick();
        DirtyLv cleanLv = new DirtyLv();
        cleanLv.rLv = 5;
        cleanLv.gLv = 3;
        cleanLv.bLv = 1;
        stick.cleanLv = cleanLv;
        stick.magicType = 0;

        // 대상 오염도 설정
        dirty = new DirtyLv();
        dirty.rLv = 10;
        dirty.gLv = 0;
        dirty.bLv = 1;
    }
    // End 임시 //

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            //IsPainting(true);
            //PaintToTarget();
            TimingDraw();
            Debug.Log("마우스 좌클릭");
        }
        else if (IsPainting() == true)
        {
            IsPainting(false);
            StopCheckTargetProcess();
            StopTimingDraw();
            Debug.Log("마우스 좌클릭 해제");
        }
        //else if (drawCoroutine == true)
        //{
        //    StopTimingDraw();
        //    Debug.Log("몇 번 호출되나요?");
        //}

        // 임시 스펠 변경 //
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            stick.magicType = MagicType.Zero;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            stick.magicType = MagicType.One;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            stick.magicType = MagicType.Two;
        }
        // End 임시 스펠 변경 //

        // Utility
        // Dirty 모두 제거 (일정 %에 도달하면 사용할 부분(지금은 단일대상))
        if (target != null && Input.GetKeyDown(KeyCode.E))
        {
            target.ClearTexture();
        }
        // Dirty 초기화 (초기화 버튼을 누른다면 적용할 부분(지금은 단일대상))
        if (target != null && Input.GetKeyDown(KeyCode.R))
        {
            target.ResetTexture();
        }
        if (target != null && target.IsDrawable() && Input.GetKeyDown(KeyCode.T))
        {
            target.CompleteTwinkle();
        }
    }


    private bool IsPainting()
    {
        return isPainting;
    }
    private void IsPainting(bool _do)
    {
        isPainting = _do;
    }


    // 임시 명칭
    public void PaintToTarget(Ray _ray)
    {
#if UNITY_EDITOR
        Debug.Log("Try");
        //Debug.DrawRay(screenRay.origin, screenRay.direction, Color.green);
#endif
        if (Physics.Raycast(_ray, out var hitInfo))
        {
#if UNITY_EDITOR
            Debug.Log("In Raycast");
#endif
            if (prevCollider != hitInfo.collider)
            {
                prevCollider = hitInfo.collider;
                hitInfo.collider.TryGetComponent<MeshPaintTarget>(out target);
            }
#if UNITY_EDITOR
            Debug.Log("target? " + target);
#endif
            if (target != null &&
                target.IsDrawable() == true)
            {
#if UNITY_EDITOR
                //Debug.LogFormat("uvPos: {0} | coord: {1}", uvPos, hitInfo.textureCoord);
#endif
                if (uvPos != hitInfo.textureCoord)
                {
#if UNITY_EDITOR
                    Debug.Log("Coord is not Same.");
#endif
                    uvPos = hitInfo.textureCoord;
                    if (maxDistance >= hitInfo.distance)
                        effective = true;
                    else if (effectiveDistance >= hitInfo.distance)
                        effective = true;
                    else
                        effective = false;

#if UNITY_EDITOR
                    Debug.Log("Before Draw" + effective);
#endif
                    // 유효한 사거리라면 DrawRender를 실행
                    if (effective)
                    {
                        // target에서 Draw처리하게 하는데
                        // uvPos와 Paint의 Color, Size, Distance, Drawable을 넘김 (Compute Shader에서 처리할 부분)

                        // 유효사거리 내에 있다면 효과 1, 최대사거리에 근접한다면 효과 1 ~ 0
                        float distance;
                        if (hitInfo.distance <= effectiveDistance)
                            distance = 1f;
                        else 
                            distance = 1 - ((hitInfo.distance - effectiveDistance) / (maxDistance - effectiveDistance));

                        // 세척력
                        color = WashPower();
#if UNITY_EDITOR
                        Debug.LogFormat("r: {0}, g: {1}, b: {2}", color.x, color.y, color.z);
#endif
                        if (target.IsClear() == false)
                            target.DrawRender(isPainting, uvPos, color, size, distance);
                        target.DrawWet(isPainting, uvPos, size, distance);
                        CheckTargetProcess();
                    }
                }
            }
        }
    }


    // 종합 세척력 결정
    private Vector4 WashPower()
    {
        DirtyLv cleanLv = stick.cleanLv;

        float rPow = CalculatePower(cleanLv.rLv, dirty.rLv, stick.magicType);
        float gPow = CalculatePower(cleanLv.gLv, dirty.gLv, stick.magicType);
        float bPow = CalculatePower(cleanLv.bLv, dirty.bLv, stick.magicType);

        return new Vector4(rPow, gPow, bPow, 1);
    }

    // 세척력 계산
    private float CalculatePower(float _cleanLv, float _dirtyLv, MagicType _type)
    {
        float magicPw = GetMagicPower(_type);

        float cleanLvPw = (float)_cleanLv / _dirtyLv; // 타입이 같은 것에 대해

        float pw = cleanLvPw * magicPw; // 기본: 지팡이Pw(세척기) * 마법Pw(노즐)

        return pw;
    }

    // 마법 스펠에 따른 보정치
    private float GetMagicPower(MagicType _type)
    {
        switch((int)_type)
        {
            default:
                return 1f;
            case 1: // 15도
                return 0.85f;
            case 2: // 25도
                return 0.7f;
            case 3: // 45도
                return 0.45f;
            case 4: // 세척제
                return 1f;
        }
    }


    #region Paint Coroutine
    private void TimingDraw()
    {
        if (drawCoroutine == false)
        {
            drawCoroutine = true;
            IsPainting(true);
            StartCoroutine("TimingDrawCoroutine");
        }
    }
    private void StopTimingDraw()
    {
        if (drawCoroutine == true)
        {
            drawCoroutine = false;
            IsPainting(false);
            StopCoroutine("TimingDrawCoroutine");
        }
    }

    private void CheckTargetProcess()
    {
        if (runCoroutine == false)
        {
            runCoroutine = true;
            StartCoroutine("CheckTargetProcessCoroutine");
        }
    }
    private void StopCheckTargetProcess()
    {
        if (runCoroutine == true)
        {
            runCoroutine = false;
            StopCoroutine("CheckTargetProcessCoroutine");
        }
    }


    // Brush
    private IEnumerator TimingDrawCoroutine()
    {
        while(true)
        {
            Ray screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            screenRay.origin = this.transform.position;
            PaintToTarget(screenRay);
            yield return new WaitForSeconds(drawTiming);
        }
    }

    private IEnumerator CheckTargetProcessCoroutine()
    {
#if UNITY_EDITOR
        Debug.Log("[CheckTargetProcessCoroutine]");
#endif
        while (true)
        {
            if (target != null && target.IsDrawable())
                target.CheckAutoClear();
            yield return new WaitForSeconds(waitTime);
        }
    }
    #endregion
}
