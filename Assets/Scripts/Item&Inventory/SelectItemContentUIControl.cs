using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectItemContentUIControl : MonoBehaviour
{
    [SerializeField]
    private InGameAllItemInfo inGameAllItemInfo_ = null;
    [SerializeField]
    private GameObject itemUIPrefab_ = null;
    private List<Dictionary<string, object>> itemInfoList_ = new List<Dictionary<string, object>>();
    private float xOffset_ = 20f;
    private float uiWidth_ = 550f;

    private RectTransform rtr = null;

    private void Awake()
    {
        rtr = this.gameObject.GetComponent<RectTransform>();
        SettSpellUIList();
        InstantiateUIPrefabs();
    }
    private void Update()
    {
        //Debug.Log(rtr.sizeDelta.x);
    }
    private void SettSpellUIList()
    {
        itemInfoList_.Clear();
        if (this.gameObject.CompareTag("SelectStaff"))
        {
            inGameAllItemInfo_.SetItemStaffUIList(out itemInfoList_);
        }
        else if (this.gameObject.CompareTag("SelectSpell"))
        {
            inGameAllItemInfo_.SetItemSpellUIList(out itemInfoList_);
        }
    }
    private void InstantiateUIPrefabs()
    {
        for (int i = 0; i < itemInfoList_.Count; i++)
        {
            GameObject go = Instantiate(itemUIPrefab_, this.transform);
            Vector3 newPos = go.transform.localPosition;
            newPos.x = i * (xOffset_ + uiWidth_) + xOffset_;
            go.transform.localPosition = newPos;
            // info 実特
            ItemInfo itemInfo = go.GetComponentInChildren<ItemInfo>();
            itemInfo.itemCategory = itemInfoList_[i]["ItemCategory"].ToString();
            itemInfo.itemName = itemInfoList_[i]["ItemName"].ToString();
            itemInfo.description = itemInfoList_[i]["Description"].ToString();
            itemInfo.imageFileName = itemInfoList_[i]["ImageFileName"].ToString();
            itemInfo.useable = itemInfoList_[i]["Useable"].ToString();
            // UI 実特
            itemInfo.SetUI();
        }
        if (rtr != null)
        {

            float contentWidth = 0;
            contentWidth = itemInfoList_.Count * (xOffset_ + uiWidth_) - (3 * xOffset_ + 2 * uiWidth_);
            Vector2 sizedelta = rtr.sizeDelta;
            rtr.sizeDelta = new Vector2(contentWidth, sizedelta.y);
        }
    }

} // end of class
