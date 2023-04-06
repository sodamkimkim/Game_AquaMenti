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

    public bool isInventoryPanOpen_ { get; set; }

    private void Awake()
    {
        // # 변수 초기화
        isInventoryPanOpen_ = false;
        nowWearingInfo_ = GetComponent<NowWearingInfo>();
        selectStaffManager_ = invenUIArr[1].GetComponent<SelectStaffManager>();
        selectSpellManager_ = invenUIArr[2].GetComponent<SelectSpellManager>();

        // # 하위 메니저 게으른 초기화 => 콜백함수 전달
        selectStaffManager_.Init(CloseAllInvenUI, SelectItem);
        selectSpellManager_.Init(CloseAllInvenUI, SelectItem);
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
        }
        else if(_selectItem.itemCategory_.Equals(InGameAllItemInfo.EItemCategory.Spell.ToString()))
        { // # Spell
            nowWearingInfo_.SetNowWearingSpell(_selectItem);
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
} // end of class
