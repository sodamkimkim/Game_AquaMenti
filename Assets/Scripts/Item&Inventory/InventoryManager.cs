using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("Aggregation")] // # Aggregation(집합관계) : 생명주기를 달리 함. ex) 역사과목, 수강학생
    [SerializeField]
    private GameObject inventoryPanUIGo_ = null; // inventory panel
    [SerializeField]
    private GameObject[] invenUIArr = null; // Inventory panel 하위의 Inventory, SelectStaff, SelectSpell UI GameObject 저장하는 Arr

    [Header("Composition")] // # Composition(구성관계) : 생명주기를 같이 함. ex) 차 & 엔진
    private NowWearingInfo nowWearingInfo_;
    private SelectStaffManager selectStaffManager_;
    private SelectSpellManager selectSpellManager_;

    // # item 생성 관련
    [SerializeField]
    private PlayerFocusManager playerFocusManager_ = null;   
    [SerializeField]
    private GameObject[] staffArr_= null;

    public bool isInventoryPanOpen_ { get; set; }

    private Staff nowStaff_ = null;
    private void Awake()
    {
        // # 변수 초기화
        isInventoryPanOpen_ = false;
        nowWearingInfo_ = inventoryPanUIGo_.GetComponentInChildren<NowWearingInfo>();
        selectStaffManager_ = invenUIArr[1].GetComponent<SelectStaffManager>();
        selectSpellManager_ = invenUIArr[2].GetComponent<SelectSpellManager>();

        // # 하위 메니저 게으른 초기화 => 콜백함수 전달
        selectStaffManager_.Init(CloseAllInvenUI, SelectItem);
        selectSpellManager_.Init(CloseAllInvenUI, SelectItem);

    }
    private void Start()
    {
        SetDefaultPlayerItem();
    }
    private void SetDefaultPlayerItem()
    {
        SetStaff(0);
        // TODO Spell
        // SetSpell(0);
    }
    private void SetStaff(int _idx)
    {
        CloseAllstaff();
        staffArr_[_idx].SetActive(true);
        nowStaff_ = staffArr_[_idx].gameObject.GetComponent<Staff>();
        playerFocusManager_.SetStaff(nowStaff_);
    }


    public void OpenInventoryPan()
    {
        isInventoryPanOpen_ = true;
        CloseAllInvenUIAndOpenDefaultUI();
        inventoryPanUIGo_.SetActive(true);
    }
    public void CloseInventoryPan()
    {
        isInventoryPanOpen_ = false;
        inventoryPanUIGo_.SetActive(false);
    }
    /// <summary>
    /// : 하위 매니져에서 item 선택하면 콜백으로 이 메서드 실행.
    /// - invenUI다꺼주고 invenUI켜주는 메서드
    /// </summary>
    private void SelectItem(NowWearingInfo.NowWearingItem _selectItem)
    {
        CloseAllInvenUIAndOpenDefaultUI();
        // # staff spell 구분해서 NowWearing 함수 호출
        if (_selectItem.itemCategory_.Equals(InGameAllItemInfo.EItemCategory.Staff.ToString()))
        { // # Staff 
            nowWearingInfo_.SetNowWearingStaff(_selectItem);
            if(_selectItem.itemName_==InGameAllItemInfo.EStaffName.AmberStaff.ToString())
            { // AmberStaff 켜기
                SetStaff(0);
            }
            else if(_selectItem.itemName_ == InGameAllItemInfo.EStaffName.RubyStaff.ToString())
            {
                SetStaff(1);
            }

        }
        else if (_selectItem.itemCategory_.Equals(InGameAllItemInfo.EItemCategory.Spell.ToString()))
        { // # Spell
            nowWearingInfo_.SetNowWearingSpell(_selectItem);
            // TODO
        }
    }
    /// <summary>
    /// invenPanel 하위 inven UI를 다 꺼주는 메서드
    /// </summary>
    private void CloseAllInvenUI()
    {
        foreach (GameObject invenUIgo in invenUIArr)
        {
            invenUIgo.SetActive(false);
        }
    }
    private void CloseAllInvenUIAndOpenDefaultUI()
    {
        CloseAllInvenUI();
        invenUIArr[0].SetActive(true);
    }
    private void CloseAllstaff()
    {
        foreach(GameObject go in staffArr_)
        {
            go.SetActive(false);
        }
    }
} // end of class
