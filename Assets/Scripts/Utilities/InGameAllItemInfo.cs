using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// csv에서 게임세상 모든 아이템 정보 불러와 저장해 놓은 클래스
/// </summary>
public class InGameAllItemInfo : MonoBehaviour
{
    private List<Dictionary<string, object>> itemInfoList_ = new List<Dictionary<string, object>>();
    private List<Dictionary<string, object>> itemStaffInfoList_ = new List<Dictionary<string, object>>();
    private List<Dictionary<string, object>> itemSpellInfoList_ = new List<Dictionary<string, object>>();
    private List<NowWearingInfo.NowWearingItem> spellList_ = new List<NowWearingInfo.NowWearingItem>();
    public enum EItemCategory { Staff, Spell, Len }
    public enum EStaffName { AmberStaff, DiamondStaff, Len }
    public enum ESpellName
    {
        Deg0MagicSpell,
        Deg15MagicSpell,
        Deg25MagicSpell,
        Deg45MagicSpell
        , Len
    }
    public Sprite[] itemImgSpriteArr_ = null;
    private void Awake()
    {
        CSVReader.Read("Datas/GameInfo/ItemInfo", out itemInfoList_);
        GetAllStaffInfo();
        GetAllSpellInfo();
        itemImgSpriteArr_ = Resources.LoadAll<Sprite>("Textures\\UI\\Items");
        foreach (var item in itemImgSpriteArr_)
        {
           // Debug.Log(item);
        }

    }
    private void Start()
    {
        //GetAllItemInfo();
        // SearchItembyItemName("AmberStaff");
    }
    //public Dictionary<string, object> GetDefaultStaff()
    //{
    //    return itemStaffInfoList_[0];
    //}
    //public Dictionary<string, object> GetDefaultSpell()
    //{
    //    return itemSpellInfoList_[0];
    //}
    /// <summary>
    /// 모든 인게임 아이템 정보 가져오기
    /// </summary>
    public void GetAllItemInfo()
    {
        for (int i = 0; i < itemInfoList_.Count; i++)
        {
            Debug.Log($"ItemCategory : {itemInfoList_[i]["ItemCategory"].ToString()}" +
                $"/ ItemName : {itemInfoList_[i]["ItemName"].ToString()}" +
                $"/ Description : {itemInfoList_[i]["Description"].ToString()}" +
                $"/ Status : {itemInfoList_[i]["Status"].ToString()}");
        }
    }

    public void SearchItembyItemName(string _itemName)
    {
        for (int i = 0; i < itemInfoList_.Count; i++)
        {
            if (itemInfoList_[i]["ItemName"].ToString().Equals(_itemName))
            {
                Debug.Log($"ItemName으로 찾음: ItemName : {itemInfoList_[i]["ItemName"].ToString()}");
            }
        }
    }
    /// <summary>
    /// 모든 인게임 EItemCategory == Staff 아이템 정보 가져오기
    /// </summary>
    private void GetAllStaffInfo()
    {
        itemStaffInfoList_.Clear();
        for (int i = 0; i < itemInfoList_.Count; i++)
        {
            if (itemInfoList_[i]["ItemCategory"].ToString() == EItemCategory.Staff.ToString())
            {
                itemStaffInfoList_.Add(itemInfoList_[i]);
            }
        }
    }
    /// <summary>
    /// 모든 인게임 EItemCategory == Spell 아이템 정보 가져오기
    /// </summary>
    private void GetAllSpellInfo()
    {
        itemSpellInfoList_.Clear();
        for (int i = 0; i < itemInfoList_.Count; i++)
        {
            if (itemInfoList_[i]["ItemCategory"].ToString() == EItemCategory.Spell.ToString())
            {
                itemSpellInfoList_.Add(itemInfoList_[i]);
            }
        }
        SetSpellItemInfoList();
    }
    /// <summary>
    /// 모든 staff 정보 가져와서 UI생성하기
    /// </summary>
    public void SetItemStaffUIList(out List<Dictionary<string, object>> _itemStaffInfoList)
    {

        _itemStaffInfoList = itemStaffInfoList_;
    }
    /// <summary>
    /// 모든 spell 정보 가져와서 UI생성하기
    /// </summary>
    public void SetItemSpellUIList(out List<Dictionary<string, object>> _itemSpellInfoList)
    {
        _itemSpellInfoList = itemSpellInfoList_;
    }
    /// <summary>
    /// file name 으로 Item이미지 찾기
    /// </summary>
    public Sprite GetItemImg(string _imgFileName)
    {
        Sprite defaultSprite = itemImgSpriteArr_[0];
        foreach (Sprite sprite in itemImgSpriteArr_)
        {

            if (sprite.name == _imgFileName)
            {
                return sprite;
            }
        }
        return defaultSprite;
    }
    private void SetSpellItemInfoList()
    {
        //            itemSpellInfoList_.Clear();
        //for (int i = 0; i < itemInfoList_.Count; i++)
        //{
        //    if (itemInfoList_[i]["ItemCategory"].ToString() == EItemCategory.Spell.ToString())
        //    {
        //        itemSpellInfoList_.Add(itemInfoList_[i]);
        //    }
        //}
        //            public string itemCategory { get; set; }
        //public string itemName { get; set; }
        //public string description { get; set; }
        //public string imageFileName { get; set; }
        //public string useable { get; set; }

    //            public string itemCategory_;
    //public string itemName_;
    //public string itemImgFileName_; // Asset에 저장된 image파일 이름
    //public string itemDescription_;
    //    Debug.Log($"ItemCategory : {itemInfoList_[i]["ItemCategory"].ToString()}" +
    //$"/ ItemName : {itemInfoList_[i]["ItemName"].ToString()}" +
    //$"/ Description : {itemInfoList_[i]["Description"].ToString()}" +
    //$"/ Status : {itemInfoList_[i]["Status"].ToString()}");
    spellList_.Clear();
        for (int i = 0; i < itemSpellInfoList_.Count; i++)
        {
            NowWearingInfo.NowWearingItem spellItem = new NowWearingInfo.NowWearingItem();
            spellItem.itemCategory_ = itemSpellInfoList_[i]["ItemCategory"].ToString();
            spellItem.itemName_ = itemSpellInfoList_[i]["ItemName"].ToString();
            spellItem.itemDescription_ = itemSpellInfoList_[i]["Description"].ToString();
            spellItem.itemImgFileName_ = itemSpellInfoList_[i]["ImageFileName"].ToString();
            //spellItem.itemCategory_ = itemSpellInfoList_[i]["Useable"].ToString();
            spellList_.Add(spellItem);
       // Debug.Log("######" + spellList_[i].itemName_);
        }
    }
    public void GetSpellItemList(out List<NowWearingInfo.NowWearingItem> _spellList)
    {
        _spellList = spellList_;
    }

} // end of class
