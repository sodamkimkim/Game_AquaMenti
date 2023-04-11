using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffUIContents : MonoBehaviour
{
    [SerializeField]
    private InGameAllItemInfo inGameAllItemInfo_ = null;
    [SerializeField]
    private GameObject staffUIPrefab_ = null;
    private List<Dictionary<string, object>> itemStaffInfoList_ = new List<Dictionary<string, object>>();
    private float xOffset_ = 20f;
    private float uiWidth_ = 550f;
    private RectTransform rtr = null;

    private void Awake()
    {
        rtr = this.gameObject.GetComponent<RectTransform>();
        SettStaffUIList();
        InstantiateUIPrefabs();
    }
    public void SettStaffUIList()
    {
        itemStaffInfoList_.Clear();
        inGameAllItemInfo_.SetItemStaffUIList(out itemStaffInfoList_);
    }
    private void InstantiateUIPrefabs()
    {
        for (int i = 0; i < itemStaffInfoList_.Count; i++)
        {
            GameObject go = Instantiate(staffUIPrefab_, this.transform);
            Vector3 newPos = go.transform.localPosition;
            newPos.x = i * (xOffset_ + uiWidth_)+xOffset_;
            go.transform.localPosition = newPos;
            // info 実特
            ItemInfo itemInfo = go.GetComponentInChildren<ItemInfo>();
            itemInfo.itemCategory = itemStaffInfoList_[i]["ItemCategory"].ToString();
            itemInfo.itemName = itemStaffInfoList_[i]["ItemName"].ToString();
            itemInfo.description = itemStaffInfoList_[i]["Description"].ToString();
            itemInfo.imageFileName = itemStaffInfoList_[i]["ImageFileName"].ToString();
            itemInfo.useable = itemStaffInfoList_[i]["Useable"].ToString();
            // UI 実特

            itemInfo.SetUI();
        }
        if (rtr != null)
        {

            float contentWidth = 0;
            contentWidth = itemStaffInfoList_.Count * (xOffset_ + uiWidth_) - 1160f;
            Vector2 sizedelta = rtr.sizeDelta;
            rtr.sizeDelta = new Vector2(contentWidth, sizedelta.y);
        }

    }
} // end of class
