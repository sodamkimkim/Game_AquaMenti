using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [Header("Aggregation")] // # Aggregation(집합관계) : 생명주기를 달리 함. ex) 역사과목, 수강학생
    [SerializeField]
    private GameObject inventoryPanUIGo_ = null; // inventory panel

    // # item 생성 관련
    [SerializeField]
    private PlayerFocusManager playerFocusManager_ = null;
    [SerializeField]
    private InGameAllItemInfo inGameAllItemInfo = null;

    [Header("Composition")] // # Composition(구성관계) : 생명주기를 같이 함. ex) 차 & 엔진
    private NowWearingInfo nowWearingInfo_;
    private SelectItemManager selectStaffManager_;
    private SelectItemManager selectSpellManager_;
    [SerializeField]
    private GameObject[] staffArr_ = null;
    [SerializeField]
    private GameObject[] staffPartsArr_ = null; // # staff 하위 brushgroup, waterpump gameObject
    [SerializeField]
    private GameObject[] bottomImgArr_ = null; // # [0] : staffImage, [1] : spellImage

    public bool isInventoryPanOpen_ { get; set; }
    private Staff nowStaff_ = null;
    private WaterPumpActivator nowWaterPumpActivator_ = null;
    [SerializeField]
    private GameObject[] invenUIArr_ = new GameObject[3]; // Inventory panel 하위의 Inventory, SelectStaff, SelectSpell UI GameObject 저장하는 Arr

    [SerializeField]
    private WandRaySpawner wandRaySpawner_ = null;
    private void Awake()
    {
        // # 변수 초기화
        isInventoryPanOpen_ = false;
        nowWearingInfo_ = invenUIArr_[0].GetComponent<NowWearingInfo>();

        selectStaffManager_ = invenUIArr_[1].GetComponent<SelectItemManager>();
        selectSpellManager_ = invenUIArr_[2].GetComponent<SelectItemManager>();
        // # 하위 메니저 게으른 초기화 => 콜백함수 전달
        selectStaffManager_.Init(CloseAllInvenUI, SelectItem);
        selectSpellManager_.Init(CloseAllInvenUI, SelectItem);
    }
    private void Start()
    {

        SetDefaultPlayerItem();
    }
    /// <summary>
    /// 게임 시작할 때, default로 장착하는 아이템 셋팅
    /// </summary>
    private void SetDefaultPlayerItem()
    {
        SetStaff(0);
        SetBottomUIStaffImg("Staff1"); // TODO 임시, save load 들어가면 변경해야함!
       // SetSpell(0);
        SetBottomUISpellImg("Deg0MagicSpell"); // TODO 임시, save load 들어가면 변경해야함!
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
    public void SelectItem(NowWearingInfo.NowWearingItem _selectItem)
    {
        CloseAllInvenUIAndOpenDefaultUI();
        // # staff spell 구분해서 NowWearing 함수 호출
        if (_selectItem.itemCategory_.Equals(InGameAllItemInfo.EItemCategory.Staff.ToString()))
        { // # Staff 
            nowWearingInfo_.SetNowWearingItem(_selectItem);
            SetStaff(_selectItem);

        }
        else if (_selectItem.itemCategory_.Equals(InGameAllItemInfo.EItemCategory.Spell.ToString()))
        { // # Spell
            nowWearingInfo_.SetNowWearingItem(_selectItem);
            SetSpell(_selectItem);

        }
    }
    /// <summary>
    /// invenPanel 하위 inven UI를 다 꺼주는 메서드
    /// </summary>
    private void CloseAllInvenUI()
    {
        foreach (GameObject invenUIgo in invenUIArr_)
        {
            invenUIgo.SetActive(false);
        }
    }
    private void CloseAllInvenUIAndOpenDefaultUI()
    {
        CloseAllInvenUI();
        invenUIArr_[0].SetActive(true);
    }
    /// <summary>
    /// staff 오브젝트 교체하기 전, 모든 staff 다 꺼주는 메서드 
    /// </summary>
    private void CloseAllstaff()
    {
        foreach (GameObject go in staffArr_)
        {
            go.SetActive(false);
        }
    }

    private void SetStaff(NowWearingInfo.NowWearingItem _selectItem)
    {
        if (_selectItem.itemName_ == InGameAllItemInfo.EStaffName.Staff1.ToString())
        { // # AmberStaff 켜기
            SetStaff(0);
            SetBottomUIStaffImg(_selectItem.itemImgFileName_);
        }
        else if (_selectItem.itemName_ == InGameAllItemInfo.EStaffName.Staff2.ToString())
        { // # RubyStaff 켜기
            SetStaff(1);
            SetBottomUIStaffImg(_selectItem.itemImgFileName_);
        }
        else if (_selectItem.itemName_ == InGameAllItemInfo.EStaffName.Staff3.ToString())
        { // # RubyStaff 켜기
            SetStaff(2);
            SetBottomUIStaffImg(_selectItem.itemImgFileName_);
        }
    }
    /// <summary>
    /// Staff 바꿔끼워주는 메서드
    /// </summary>
    /// <param name="_idx"></param>
    private void SetStaff(int _idx)
    {
        CloseAllstaff();
        staffArr_[_idx].SetActive(true);
        foreach (GameObject go in staffPartsArr_)
        {
            go.SetActive(true);
            go.transform.SetParent(staffArr_[_idx].transform);
            //TODO
            // staff 변경할 때, parts default 셋팅 해줘야 함!

        }
        nowStaff_ = staffArr_[_idx].gameObject.GetComponent<Staff>();
        playerFocusManager_.SetStaff(nowStaff_);
        nowWaterPumpActivator_ = staffArr_[_idx].gameObject.GetComponent<WaterPumpActivator>();
        Debug.Log("waterPumpActivator change? " + nowWaterPumpActivator_.gameObject.name);
    }
    public WaterPumpActivator GetWaterPumpActivator()
    {
        return nowWaterPumpActivator_;
    }
    private void SetSpell(NowWearingInfo.NowWearingItem _selectItem)
    {
        // TODO
        if (_selectItem.itemName_ == InGameAllItemInfo.ESpellName.Deg0MagicSpell.ToString())
        { // # AmberStaff 켜기
            wandRaySpawner_.rayAngle_ = 0f;
            SetBottomUISpellImg(_selectItem.itemImgFileName_);
        }
        else if (_selectItem.itemName_ == InGameAllItemInfo.ESpellName.Deg15MagicSpell.ToString())
        { // # RubyStaff 켜기
            wandRaySpawner_.rayAngle_ = 15f;
            SetBottomUISpellImg(_selectItem.itemImgFileName_);
        }
        else if (_selectItem.itemName_ == InGameAllItemInfo.ESpellName.Deg25MagicSpell.ToString())
        { // # RubyStaff 켜기
            wandRaySpawner_.rayAngle_ = 25f;
            SetBottomUISpellImg(_selectItem.itemImgFileName_);
        }
        else if (_selectItem.itemName_ == InGameAllItemInfo.ESpellName.Deg45MagicSpell.ToString())
        { // # RubyStaff 켜기
            wandRaySpawner_.rayAngle_ = 45f;
            SetBottomUISpellImg(_selectItem.itemImgFileName_);
        }

    }
    private void SetBottomUIStaffImg(string _imgFileName)
    {
        Image img = bottomImgArr_[0].GetComponent<Image>();
        img.sprite = inGameAllItemInfo.GetItemImg(_imgFileName);
    }
    private void SetBottomUISpellImg(string _imgFileName)
    {
        Image img = bottomImgArr_[1].GetComponent<Image>();
        img.sprite = inGameAllItemInfo.GetItemImg(_imgFileName);
    }
} // end of class
