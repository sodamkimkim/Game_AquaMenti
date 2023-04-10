using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellUIContents : MonoBehaviour
{
    [SerializeField]
    private InGameAllItemInfo inGameAllItemInfo_ = null;
    [SerializeField]
    private GameObject spellUIPrefab_ = null;
    private List<Dictionary<string, object>> itemSpellInfoList_ = new List<Dictionary<string, object>>();
    private float xOffset_ = 20f;
    private float uiWidth = 550f;


    private void Awake()
    {  
        SettSpellUIList();
        InstantiateUIPrefabs();
    }
    private void SettSpellUIList()
    {
        itemSpellInfoList_.Clear();
        inGameAllItemInfo_.SetItemSpellUIList(out itemSpellInfoList_);
    }
    private void InstantiateUIPrefabs()
    {
        for (int i = 0; i < itemSpellInfoList_.Count; i++)
        {
            GameObject go = Instantiate(spellUIPrefab_, this.transform);
            Vector3 newPos = go.transform.localPosition;
            newPos.x = i * (xOffset_ + uiWidth);
            go.transform.localPosition = newPos;
            // info 実特
            ItemInfo itemInfo = go.GetComponentInChildren<ItemInfo>();
            itemInfo.itemCategory = itemSpellInfoList_[i]["ItemCategory"].ToString();
            itemInfo.itemName = itemSpellInfoList_[i]["ItemName"].ToString();
            itemInfo.description = itemSpellInfoList_[i]["Description"].ToString();
            itemInfo.imageFileName = itemSpellInfoList_[i]["ImageFileName"].ToString();
            itemInfo.useable = itemSpellInfoList_[i]["Useable"].ToString();
            // UI 実特
            itemInfo.SetUI();
        }

    }

} // end of class
