using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NowWearingInfo : MonoBehaviour
{
    // 0: EquipedStaff, 1: EquipedSpell
    [SerializeField]
    private EquipedItem[] equipedItemArr = null;
    
    // 현재 착용 중인 아이템 정보 기록에 사용할 structure
    public struct NowWearingItem
    { // # staff : [0], # spell : [1]
        // item정보
        public string itemCategory_;
        public string itemName_;
        public string itemImgFileName_; // Asset에 저장된 image파일 이름
        public string itemDescription_;
        public NowWearingItem(string _itemCategory, string _itemName, string _itemImgFileName_, string _itemDescription)
        {
            this.itemCategory_ = _itemCategory;
            this.itemName_ = _itemName;
            this.itemImgFileName_ = _itemImgFileName_;
            this.itemDescription_ = _itemDescription;
        } // end of construct
        public override string ToString()
        {
            return $"itemCategory_ : {itemCategory_}, itemName_ : {itemName_}, itemImg_ : {itemName_}, itemDescription_ : {itemDescription_}";
        }
    } // end of struct

    //  현재 착용중인 아이템 정보 저장 arr -> [0] : Staff, [1] : Spell
    private NowWearingItem[] nowWearingArr_ = new NowWearingItem[2];

    private void Awake()
    {
        // # 기본 Staff와 기본 Spell로 NowWearing정보 초기화
        // TODO
        //nowWearingArr_[0] = 
        //nowWearingArr_[1] = 
        equipedItemArr = GetComponentsInChildren<EquipedItem>();

        SetDefaultItems();
    }
    private void SetDefaultItems()
    {
        NowWearingItem defaultStaff = new NowWearingItem();
        defaultStaff.itemCategory_ = "Staff";
        defaultStaff.itemName_ = "AmberStaff";
        defaultStaff.itemDescription_ = "기본 Staff";
        defaultStaff.itemImgFileName_ = "AmberStaff";
        //nowWearingArr_[0] = defaultStaff;
        SetNowWearingItem(defaultStaff);

        NowWearingItem defaultSpell = new NowWearingItem();
        defaultSpell.itemCategory_ = "Spell";
        defaultSpell.itemName_ = "Deg0MagicSpell";
        defaultSpell.itemDescription_ = "각도0도로 분출되는 물 마법";
        defaultSpell.itemImgFileName_ = "Deg0MagicSpell";
        //nowWearingArr_[1] = defaultSpell;
        SetNowWearingItem(defaultSpell);
    }
    /// <summary>
    /// : 현재 착용중인 Item정보 저장
    /// </summary>
    /// <param name="_selectItem"></param>
    public void SetNowWearingItem(NowWearingItem _selectItem)
    {
        int idx = -1;
        if (_selectItem.itemCategory_.Equals(InGameAllItemInfo.EItemCategory.Staff.ToString()))
        {
            idx = 0;
        }
        else if(_selectItem.itemCategory_.Equals(InGameAllItemInfo.EItemCategory.Spell.ToString()))
        {
            idx = 1;
        }

        nowWearingArr_[idx] = _selectItem;
     //   Debug.Log(nowWearingArr_[idx].ToString());
    //    Debug.Log(nowWearingArr_[idx].ToString());

        equipedItemArr[idx].itemCategory = nowWearingArr_[idx].itemCategory_;
        equipedItemArr[idx].itemName = nowWearingArr_[idx].itemName_;
        equipedItemArr[idx].description = nowWearingArr_[idx].itemDescription_;
        equipedItemArr[idx].imageFileName = nowWearingArr_[idx].itemImgFileName_;

        equipedItemArr[idx].SetEquipedItemUI();

    }

    public NowWearingItem GetNowWearingStaff()
    {
        return nowWearingArr_[0];
    }
    public NowWearingItem GetNowWearingSpell()
    {
        return nowWearingArr_[1];
    }
} // end of class
