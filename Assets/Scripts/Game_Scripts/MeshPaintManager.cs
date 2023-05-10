using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MeshPaintManager : MonoBehaviour
{
    public delegate void LoadingStartUICallback_(string _text);
    public delegate void LoadingEndUICallback_();

    private LoadingStartUICallback_ loadingStartCallback_;
    private LoadingEndUICallback_ loadingEndCallback_;

    [SerializeField] GameManager gameManager_;
    [SerializeField] PlayerKeyInput keyInput_;
    [SerializeField] PlayerFocusManager focusManager_;

    private List<MeshPaintTarget> meshTargetList_ = null;


    private void Awake()
    {
        // Scene에 존재하는 Object를 대상으로 MeshPaintTarget을 가져옵니다.
        MeshPaintTarget[] targets = FindObjectsOfType<MeshPaintTarget>();
        meshTargetList_ = new List<MeshPaintTarget>(targets);
#if UNITY_EDITOR
        Debug.Log("[MeshPaintManager] target Count: " + meshTargetList_.Count);
#endif
    }

    private void Update()
    {
        if (!gameManager_.isInGame_) return;
        /*        // 임시 Save 버튼
                if (Input.GetKeyDown(KeyCode.O))
                {
                    SaveTargetMask();
                }*/
        // 임시 Reset 버튼
        if (Input.GetKeyDown(KeyCode.P))
        {
            ResetTargetMask();
        }
    }


    public void Init(LoadingStartUICallback_ _loadStartCallback, LoadingEndUICallback_ _loadEndCallback)
    {
        if (meshTargetList_.Count <= 0) return;

        loadingStartCallback_ = _loadStartCallback;
        loadingEndCallback_ = _loadEndCallback;

        InitTarget();
        LoadTargetMask();
    }


    private void InitTarget()
    {
        //foreach (MeshPaintTarget target_ in meshTargetList_)
        //{
        //    target_.Init();
        //}
        StopCoroutine("InitTargetCoroutine");
        StartCoroutine("InitTargetCoroutine");
    }


    public void SaveTargetMask()
    {
        // 현재 Section의 그리는 대상만이 저장 대상이 됨
        foreach (MeshPaintTarget target_ in meshTargetList_)
        {
            if (target_.IsDrawable() && target_.IsClear() == false)
            {
                target_.SaveMask();
            }
        }
    }

    public void LoadTargetMask()
    {
        //// 그리는 대상이 아니어도 다른 세션의 진행도도 볼 수 있도록 하기 위해 조건 해제
        //foreach (MeshPaintTarget target_ in meshTargetList_)
        //{
        //    if (target_.LoadMask() == false)
        //    {
        //        Debug.LogWarning("일부 대상의 Mask를 불러오는데 실패하였습니다." + target_.name);
        //    }
        //}
        StopCoroutine("LoadTargetMaskCoroutine");
        StartCoroutine("LoadTargetMaskCoroutine");
    }

    public void ResetTargetMask()
    {
        //foreach (MeshPaintTarget target_ in meshTargetList_)
        //{
        //    if (target_.IsDrawable() && target_.IsClear() == false && target_.GetProcessPercent() > 0.0001f)
        //    {
        //        if (target_.ResetMask() == false)
        //        {
        //            Debug.LogWarning("일부 대상의 Mask를 초기화하는데 실패하였습니다.");
        //        }
        //    }
        //}
        StopCoroutine("ResetTargetMaskCoroutine");
        StartCoroutine("ResetTargetMaskCoroutine");
    }


    private void SetLoadScreen(string _type, int _cnt, int _total)
    {
        StringBuilder sb = new StringBuilder();
        int percent = Mathf.FloorToInt(_cnt / float.Parse(_total.ToString()) * 100);

        string text = string.Empty;
        switch (_type)
        {
            case "init":
                sb.Append("초기화하는 중");
                sb.Append("...");
                break;
            case "load":
                sb.Append("불러오는 중");
                sb.Append("...");
                break;
            case "reset":
                sb.Append("리셋하는 중");
                sb.Append("...");
                loadingStartCallback_?.Invoke(sb.ToString());
                return;
            default:
                sb.Append("...");
                loadingStartCallback_?.Invoke(sb.ToString());
                return;
        }
        //sb.Append(_cnt);
        //sb.Append("/");
        //sb.Append(_total);
        sb.Append("(");
        sb.Append(percent);
        sb.Append("%)");

        loadingStartCallback_?.Invoke(sb.ToString());
    }
    
    private void IsPlayerInputKeyLock(bool _lock)
    {
        if (_lock)
        {
            keyInput_.useKey = false; // 리셋 중에는 키 입력 방지
            focusManager_.isFocusLock_ = true;
        }
        else
        {
            keyInput_.useKey = true;
            focusManager_.isFocusLock_ = false;
        }
    }

    private IEnumerator InitTargetCoroutine()
    {
        int count = 0;
        string type = "init";
        SetLoadScreen(type, count, meshTargetList_.Count);
        foreach (MeshPaintTarget target_ in meshTargetList_)
        {
            target_.Init();
            ++count;
            SetLoadScreen(type, count, meshTargetList_.Count);
            yield return null;
        }
        loadingEndCallback_?.Invoke();
    }
    private IEnumerator LoadTargetMaskCoroutine()
    {
        int count = 0;
        string type = "load";
        SetLoadScreen(type, count, meshTargetList_.Count);
        foreach (MeshPaintTarget target_ in meshTargetList_)
        {
            if (target_.LoadMask() == false)
            {
                Debug.LogWarning("일부 대상의 Mask를 불러오는데 실패하였습니다." + target_.name);
            }
            else
            {
                ++count;
                SetLoadScreen(type, count, meshTargetList_.Count);
            }
            yield return null;
        }
        loadingEndCallback_?.Invoke();
        SoundManager.Instance.Play("BirdSound");
    }
    private IEnumerator ResetTargetMaskCoroutine()
    {
        int count = 0;
        string type = "reset";
        IsPlayerInputKeyLock(true);
        SetLoadScreen(type, count, meshTargetList_.Count);
        foreach (MeshPaintTarget target_ in meshTargetList_)
        {
            if (target_.IsDrawable() && target_.IsClear() == false && target_.GetProcessPercent() > 0.0001f)
            {
                if (target_.ResetMask() == false)
                {
                    Debug.LogWarning("일부 대상의 Mask를 초기화하는데 실패하였습니다.");
                }
                SetLoadScreen(type, count, meshTargetList_.Count);
            }
            yield return null;
        }
        loadingEndCallback_?.Invoke();
        IsPlayerInputKeyLock(false);
    }
}
