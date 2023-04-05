using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// csv에서 게임세상 모든 아이템 정보 불러와 저장해 놓은 클래스
/// </summary>
public class InGameAllItemInfo : MonoBehaviour
{
    private List<Dictionary<string, object>> itemInfoList = new List<Dictionary<string, object>>();
    public enum EItemCategory { Staff, MagicSpell, Len}

    private void Start()
    {
        itemInfoList = CSVReader.Read("Datas/GameInfo/ItemInfo");
        GetAllItemInfo();
    }

    /// <summary>
    /// 모든 인게임 아이템 정보 가져오기
    /// </summary>
    public void GetAllItemInfo()
    {
        for (int i = 0; i < itemInfoList.Count; i++)
        {
            Debug.Log($"ItemCategory : {itemInfoList[i]["ItemCategory"].ToString()}"+
                $"/ ItemName : {itemInfoList[i]["ItemName"].ToString()}" +
                $"/ Description : {itemInfoList[i]["Description"].ToString()}" +
                $"/ Status : {itemInfoList[i]["Status"].ToString()}");
        }
    }
    /// <summary>
    /// 모든 인게임 EItemCategory == MagicSpell 아이템 정보 가져오기
    /// </summary>
    public void GetAllStaffInfo()
    {
        for (int i = 0; i<itemInfoList.Count; i++)
        {
            if (itemInfoList[i]["ItemCategory"].ToString() == EItemCategory.Staff.ToString())
            {
                Debug.Log($"ItemName : {itemInfoList[i]["ItemName"].ToString()}" +
                    $"/ Description : {itemInfoList[i]["Description"].ToString()}" +
                    $"/ Status : {itemInfoList[i]["Status"].ToString()}");
            }
        }
    }
    /// <summary>
    /// 모든 인게임 EItemCategory == MagicSpell 아이템 정보 가져오기
    /// </summary>
    public void GetAllMagicSpellInfo()
    {
        for(int i = 0; i<itemInfoList.Count; i++)
        {
            if (itemInfoList[i]["ItemCategory"].ToString() == EItemCategory.MagicSpell.ToString())
            {
                Debug.Log($"ItemName : {itemInfoList[i]["ItemName"].ToString()}" +
                    $"/ Description : {itemInfoList[i]["Description"].ToString()}" +
                    $"/ Status : {itemInfoList[i]["Status"].ToString()}");
            }
        }
    }

} // end of class
