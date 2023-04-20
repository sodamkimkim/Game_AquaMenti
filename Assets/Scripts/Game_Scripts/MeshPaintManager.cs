using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshPaintManager : MonoBehaviour
{
    private List<MeshPaintTarget> meshTargetList_ = null;
    [SerializeField] GameManager gameManager_;


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


    public void Init()
    {
        if (meshTargetList_.Count <= 0) return;

        InitTarget();
        LoadTargetMask();
    }


    private void InitTarget()
    {
        foreach (MeshPaintTarget target_ in meshTargetList_)
        {
            target_.Init();
        }
    }


    public void SaveTargetMask()
    {
        // 현재 Section의 그리는 대상만이 저장 대상이 됨
        foreach (MeshPaintTarget target_ in meshTargetList_)
        {
            if (target_.IsDrawable())
            {
                target_.SaveMask();
            }
        }
    }

    public void LoadTargetMask()
    {
        // 그리는 대상이 아니어도 다른 세션의 진행도도 볼 수 있도록 하기 위해 조건 해제
        foreach (MeshPaintTarget target_ in meshTargetList_)
        {
            if (target_.LoadMask() == false)
            {
                Debug.LogWarning("일부 대상의 Mask를 불러오는데 실패하였습니다." + target_.name);
            }
        }
    }

    public void ResetTargetMask()
    {
        foreach (MeshPaintTarget target_ in meshTargetList_)
        {
            if (target_.IsDrawable() && target_.IsClear() == false && target_.GetProcessPercent() > 0.0001f)
            {
                if (target_.ResetMask() == false)
                {
                    Debug.LogWarning("일부 대상의 Mask를 초기화하는데 실패하였습니다.");
                }
            }
        }
    }

}
