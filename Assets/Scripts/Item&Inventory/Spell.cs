using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spell : Item
{
    [Header("Aggregation")]
    [SerializeField]
    private GameObject selectSpellUIGo_ = null;
    private Item[] itemArr = null;

    public delegate void OpenInvenUICallback_();
    public delegate void CloseInvenUICallback_(NowWearingInfo.NowWearingItem _selectItem);

    private OpenInvenUICallback_ openCallback_;
    private CloseInvenUICallback_ closeCallback_;

    private bool isSelectSpellUIOpen_ { get; set; }

    private void Awake()
    {
        itemArr = selectSpellUIGo_.GetComponentsInChildren<Item>();
        isSelectSpellUIOpen_ = false;
        foreach (Item item in itemArr)
        {
            item.gameObject.GetComponent<Button>().onClick.AddListener(SelectSpellItemCallback);
        }
    }
    public void Init(OpenInvenUICallback_ _OpenCallback, CloseInvenUICallback_ _CloseCallback_)
    {
        openCallback_ += _OpenCallback;
        closeCallback_ += _CloseCallback_;
    }
    /// <summary>
    /// inventory UI에서 CurrentSpell버튼 누르면 실행되는 콜백
    /// </summary>
    public void OnCurrentSpellClickCallback()
    {
        isSelectSpellUIOpen_ = true;
        openCallback_.Invoke();
    }
    /// <summary>
    /// : selectSpell UI에서 item 누르면 실행되는 콜백
    ///  1. 클릭한 item 정보 NowWearing에 뿌려주고
    ///  2. selectSpell UI 꺼줌
    /// </summary>
    private void SelectSpellItemCallback()
    {
        NowWearingInfo.NowWearingItem selectItemInfo = new NowWearingInfo.NowWearingItem("staff", "name", "imageFileName", "description");

        isSelectSpellUIOpen_ = false;
        closeCallback_.Invoke(selectItemInfo);
    }
} // end of class
