using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// csv에서 게임세상 모든 아이템 정보 불러와 저장해 놓은 클래스
/// </summary>
public class InGameAllItemInfo : MonoBehaviour
{
    private List<Dictionary<string, object>> itemInfoList_ = new List<Dictionary<string, object>>();
    public enum EItemCategory { Staff, Spell, Len}

    private void Start()
    {
        CSVReader.Read("Datas/GameInfo/ItemInfo", out itemInfoList_);
        GetAllItemInfo();
    }

    /// <summary>
    /// 모든 인게임 아이템 정보 가져오기
    /// </summary>
    public void GetAllItemInfo()
    {
        for (int i = 0; i < itemInfoList_.Count; i++)
        {
            Debug.Log($"ItemCategory : {itemInfoList_[i]["ItemCategory"].ToString()}"+
                $"/ ItemName : {itemInfoList_[i]["ItemName"].ToString()}" +
                $"/ Description : {itemInfoList_[i]["Description"].ToString()}" +
                $"/ Status : {itemInfoList_[i]["Status"].ToString()}");
        }
    }
    /// <summary>
    /// 모든 인게임 EItemCategory == Spell 아이템 정보 가져오기
    /// </summary>
    public void GetAllStaffInfo()
    {
        for (int i = 0; i<itemInfoList_.Count; i++)
        {
            if (itemInfoList_[i]["ItemCategory"].ToString() == EItemCategory.Staff.ToString())
            {
                Debug.Log($"ItemName : {itemInfoList_[i]["ItemName"].ToString()}" +
                    $"/ Description : {itemInfoList_[i]["Description"].ToString()}" +
                    $"/ Status : {itemInfoList_[i]["Status"].ToString()}");
            }
        }
    }
    /// <summary>
    /// 모든 인게임 EItemCategory == Spell 아이템 정보 가져오기
    /// </summary>
    public void GetAllMagicSpellInfo()
    {
        for(int i = 0; i<itemInfoList_.Count; i++)
        {
            if (itemInfoList_[i]["ItemCategory"].ToString() == EItemCategory.Spell.ToString())
            {
                Debug.Log($"ItemName : {itemInfoList_[i]["ItemName"].ToString()}" +
                    $"/ Description : {itemInfoList_[i]["Description"].ToString()}" +
                    $"/ Status : {itemInfoList_[i]["Status"].ToString()}");
            }
        }
    }

} // end of class
