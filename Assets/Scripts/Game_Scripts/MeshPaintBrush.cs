using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshPaintBrush : MonoBehaviour
{
    private Collider prevCollider_;
    private Vector2 uvPos_ = Vector2.zero;

    private MeshPaintTarget target_ = null;

    [SerializeField, Range(0.5f, 10f)]
    private float effectiveDistance_ = 1f; // 유효 사거리
    [SerializeField, Range(1f, 16f)]
    private float maxDistance_ = 2f; // 최대 사거리
    private bool effective_ = false;

    // Option
    [SerializeField, Range(1, 20)]
    private int size_ = 10;
    private Vector4 color_ = new Vector4(0f, 0f, 0f, 1f);
    private bool isPainting_ = false;

    private float drawTiming_ = 0.01f;
    private float waitTime_ = 0.1f;
    private float saveTiming_ = 1f;

    private bool drawCoroutine_ = false;
    private bool runCoroutine_ = false;
    private bool saveCoroutine_ = false;

    private Ray ray_;

    // 임시
    public enum EMagicType { Zero, One, Two }

    public struct DirtyLv // 오염타입
    {
        public int rLv { get; set; } // 표면
        public int gLv { get; set; } // 뒤덮힘
        public int bLv { get; set; } // 억셈
    }

    public struct Stick
    {
        public DirtyLv cleanLv;
        public EMagicType magicType; // 마법 => Nozzle
    }

    // 임시
    // R: 표면, G: 뒤덮힘, B: 억셈
    //private int rDirtyLv = 5;
    //private int rCleanLv = 3;
    //private int magicType = 2; // Nozzle. Maybe Enum.
    public Stick stick;
    private DirtyLv dirty;

    //public void setStartPos(Vector3 _pos)
    //{
    //    startPos_ = _pos;
    //    this.gameObject.transform.localPosition = startPos_;
    //}
    //public void SetEndPos(Vector3 _pos)
    //{
    //    endPos_ = _pos;
    //}
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
    public bool IsPainting()
    {
        return isPainting_;
    }
    public void IsPainting(bool _do)
    {
        isPainting_ = _do;
    }
    public MeshPaintTarget GetTarget()
    {
        if (target_ != null)
        {
            return target_;
        }
        else return null;
    }

    // 임시 명칭
    public void PaintToTarget(Ray _ray)
    {
#if UNITY_EDITOR
        //Debug.Log("Try");

#endif
        if (Physics.Raycast(_ray, out var hitInfo))
        {
#if UNITY_EDITOR
            //Debug.Log("In Raycast");
#endif
            if (prevCollider_ != hitInfo.collider)
            {
                prevCollider_ = hitInfo.collider;
                hitInfo.collider.TryGetComponent<MeshPaintTarget>(out target_);
            }
#if UNITY_EDITOR
            //Debug.Log("target? " + target_);
#endif
            if (target_ != null &&
                target_.IsDrawable() == true)
            {
#if UNITY_EDITOR
                //Debug.LogFormat("uvPos_: {0} | coord: {1}", uvPos_, hitInfo.textureCoord);
#endif
                if (uvPos_ != hitInfo.textureCoord)
                {
#if UNITY_EDITOR
                    //Debug.Log("Coord is not Same.");
#endif
                    uvPos_ = hitInfo.textureCoord;
                    if (maxDistance_ >= hitInfo.distance)
                        effective_ = true;
                    else if (effectiveDistance_ >= hitInfo.distance)
                        effective_ = true;
                    else
                        effective_ = false;

#if UNITY_EDITOR
                    //Debug.Log("Before Draw" + effective_);
#endif
                    // 유효한 사거리라면 DrawRender를 실행
                    if (effective_)
                    {
                        // target에서 Draw처리하게 하는데
                        // uvPos와 Paint의 Color, Size, Distance, Drawable을 넘김 (Compute Shader에서 처리할 부분)

                        // 유효사거리 내에 있다면 효과 1, 최대사거리에 근접한다면 효과 1 ~ 0
                        float distance;
                        if (hitInfo.distance <= effectiveDistance_)
                            distance = 1f;
                        else
                            distance = 1 - ((hitInfo.distance - effectiveDistance_) / (maxDistance_ - effectiveDistance_));

                        // 세척력
                        color_ = WashPower();
#if UNITY_EDITOR
                        //Debug.LogFormat("r: {0}, g: {1}, b: {2}", color_.x, color_.y, color_.z);
#endif
                        if (target_.IsClear() == false)
                            target_.DrawRender(isPainting_, uvPos_, color_, size_, distance);
                        target_.DrawWet(isPainting_, uvPos_, size_, distance);
                        CheckTargetProcess();
                        SaveTargetProcess();
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
    private float CalculatePower(float _cleanLv, float _dirtyLv, EMagicType _type)
    {
        float magicPw = GetMagicPower(_type);

        float cleanLvPw = (float)_cleanLv / _dirtyLv; // 타입이 같은 것에 대해

        float pw = cleanLvPw * magicPw; // 기본: 지팡이Pw(세척기) * 마법Pw(노즐)

        return pw;
    }

    // 마법 스펠에 따른 보정치
    private float GetMagicPower(EMagicType _type)
    {
        switch ((int)_type)
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
    /// <summary>
    /// ///////////////////////////////
    /// </summary>
    /// <param name="_ray"></param>
    public void TimingDraw(Ray _ray)
    {

        ray_ = _ray;
        if (drawCoroutine_ == false)
        {
            drawCoroutine_ = true;
            IsPainting(true);

            StartCoroutine("TimingDrawCoroutine");
        }
    }
    public void SetRayDirection(Vector3 _dir)
    {
        ray_.direction = _dir;
    }
    public void StopTimingDraw()
    {
        if (drawCoroutine_ == true)
        {
            drawCoroutine_ = false;
            IsPainting(false);
            StopCoroutine("TimingDrawCoroutine");
        }
    }

    private void CheckTargetProcess()
    {
        if (runCoroutine_ == false)
        {
            runCoroutine_ = true;
            if (target_ != null && target_.IsDrawable() && target_.IsClear() == false)
                StartCoroutine("CheckTargetProcessCoroutine");
        }
    }
    public void StopCheckTargetProcess()
    {
        if (runCoroutine_ == true)
        {
            runCoroutine_ = false;
            StopCoroutine("CheckTargetProcessCoroutine");
        }
    }

    private void SaveTargetProcess()
    {
        if (saveCoroutine_ == false)
        {
            saveCoroutine_ = true;
            if (target_ != null && target_.IsDrawable() && target_.IsClear() == false)
                StartCoroutine("SaveTargetProcessCoroutine");
        }
    }
    public void StopSaveTargetProcess()
    {
        if (saveCoroutine_ == true)
        {
            saveCoroutine_ = false;
            StopCoroutine("SaveTargetProcessCoroutine");
        }
    }


    // Brush
    private IEnumerator TimingDrawCoroutine()
    {
        while (true)
        {
            // _ray로 바꿔주기
            Ray Ray = ray_;


            Ray.origin = this.transform.position;

            //Ray.direction = _direction;
            PaintToTarget(Ray);
            // Debug.Log("in");
            Debug.DrawRay(Ray.origin, Ray.direction * effectiveDistance_, Color.green);
            yield return new WaitForSeconds(drawTiming_);
        }
    }

    private IEnumerator CheckTargetProcessCoroutine()
    {
#if UNITY_EDITOR
        //Debug.Log("[CheckTargetProcessCoroutine]");
#endif
        while (true)
        {
            if (target_ != null && target_.IsDrawable() && target_.IsClear())
                StopCheckTargetProcess();
            else if (target_ != null && target_.IsDrawable())
                target_.CheckAutoClear();
            yield return new WaitForSeconds(waitTime_);
        }
    }

    private IEnumerator SaveTargetProcessCoroutine()
    {
#if UNITY_EDITOR
        Debug.Log("[SaveTargetProcessCoroutine]");
#endif
        while (true)
        {
            // 클리어 상태가 되었다면 마지막 진행도를 저장 후 Coroutine을 멈춥니다.
            if (target_ != null && target_.IsDrawable() && target_.IsClear())
            {
                target_.SaveMask();
                StopSaveTargetProcess();
            }
            else if (target_ != null && target_.IsDrawable() && target_.IsClear() == false)
                target_.SaveMask();
            yield return new WaitForSeconds(saveTiming_);
        }
    }
    #endregion
}
