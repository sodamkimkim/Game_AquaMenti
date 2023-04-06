using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SelectSpellManager;
public class SelectStaffManager : MonoBehaviour
{
    [Header("Aggregation")]
    [SerializeField]
    private GameObject selectStaffUIGo_ = null;
    private Item[] itemArr = null;


    public delegate void OpenInvenUICallback_();
    public delegate void CloseInvenUICallback_(NowWearingInfo.NowWearingItem _selectItem);

    private OpenInvenUICallback_ openCallback_;
    private CloseInvenUICallback_ closeCallback_;
    private bool isSelectStaffUIOpen_ { get; set; }
    private void Awake()
    {
        isSelectStaffUIOpen_ = false;
    }
    public void Init(OpenInvenUICallback_ _OpenCallback, CloseInvenUICallback_ _CloseCallback_)
    {
        openCallback_ += _OpenCallback;
        closeCallback_ += _CloseCallback_;
    }
    /// <summary>
    /// inventory UI에서 CurrentStaff버튼 누르면 실행되는 콜백
    /// </summary>
    /// 
    public void OnCurrentStaffClickCallback()
    {

    }
    private void CloseSelectSpellCallback()
    {

    }
    public void OpenSelectStaffUI()
    {

    }
    public void CloseSelectStaffUI()
    {

    }
    /// <summary>
    /// item 선택 완료 수행해주는 메서드
    /// 해당 ui도 꺼줌
    /// </summary>
    public void SelectItem()
    {

    }
}  // end of class
