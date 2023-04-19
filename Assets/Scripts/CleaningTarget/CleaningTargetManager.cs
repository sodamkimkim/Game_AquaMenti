using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// mainRay와 충돌한 cleaning target object이름 가져와서
/// ingame ui에 이름, 청소 상태뿌려주고
/// 상세정보에 이름, 청소상태, 가격, 획득 돈 계산해서 돈이랑 별 얻을 떄마다 data클래스에 뿌려줌
/// </summary>
public class CleaningTargetManager : MonoBehaviour
{
    [SerializeField]
    private InGameAllItemInfo inGameAllItemInfo_ = null;
    private TargetObjectData targetObjectData_ = null;
    [SerializeField]
    private GameObject playerGo_ = null;
    private WandRaySpawner wandRaySpawner_ = null;

    [SerializeField]
    private GameObject washInfoGO_ = null;

    private ObjectNameUI objectNameUI_ = null;
    private CleaningProgressPanUI cleaningProgressPanUI = null;
    private CleaningPercentageUI cleaningPercentageUI = null;
    //private 
    private void Awake()
    {
        targetObjectData_ = GetComponent<TargetObjectData>();
        wandRaySpawner_ = Camera.main.GetComponentInChildren<WandRaySpawner>();

        // # UI
        objectNameUI_ = washInfoGO_.GetComponentInChildren<ObjectNameUI>();
        cleaningProgressPanUI = washInfoGO_.GetComponentInChildren<CleaningProgressPanUI>();
        cleaningPercentageUI = washInfoGO_.GetComponentInChildren<CleaningPercentageUI>();
    }
    // # wandrayspawner의 targetname_을 가져와서, 
    // 이름, 청소상태 뿌려주고, 아니면 이름 : "", percentage "", progress : 0
    private void Update()
    {
        SetCleaningTargetStatusUI();

    }
    private void SetCleaningTargetStatusUI()
    {
        // ex) object Name: Barrel_1_1
        if (wandRaySpawner_.cleaningTargetName_ != "")
        {
            string koreanName = targetObjectData_.GetKoreanName(wandRaySpawner_.cleaningTargetName_);
            objectNameUI_.SetObjectName(koreanName);

            //Debug.Log(1f - wandRaySpawner_.cleaningPercent_ * 0.01f);
            cleaningProgressPanUI.SetCleaningProgressImgFillAmt(1f - wandRaySpawner_.cleaningPercent_ * 0.01f);
            //cleaningPercentageUI.SetCleaningPercentageUI(Mathf.Round(0.776345f*100)*0.01f);

            cleaningPercentageUI.SetActive(true);
        }
        else
        {
            objectNameUI_.SetObjectName(wandRaySpawner_.cleaningTargetName_);
            cleaningProgressPanUI.SetCleaningProgressImgFillAmt(0f);
            //cleaningPercentageUI.SetCleaningPercentageUI(100f);
            cleaningPercentageUI.SetActive(false);
        }
    }
} // end of class
