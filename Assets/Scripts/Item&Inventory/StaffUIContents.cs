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
    private float uiWidth = 550f;

    private void Start()
    {
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
            newPos.x = i * (xOffset_ + uiWidth);
            go.transform.localPosition = newPos;

            // UI 셋팅
            ItemNameUIControl itemNameUIControl = go.GetComponentInChildren<ItemNameUIControl>();
            ItemImgUIControl itemImgUIControl = go.GetComponentInChildren<ItemImgUIControl>();
            ItemDescriptionUIControl itemDescriptionUIControl = go.GetComponentInChildren<ItemDescriptionUIControl>();

            itemNameUIControl.SetItemNameUI(itemStaffInfoList_[i]["ItemName"].ToString());
            itemImgUIControl.SetItemImageUI(itemStaffInfoList_[i]["ImageFileName"].ToString());
            itemDescriptionUIControl.SetItemDescriptionUI(itemStaffInfoList_[i]["Description"].ToString());

            if (itemStaffInfoList_[i]["Useable"].Equals("FALSE")) // 사용 불가능 상태
            {
                go.GetComponentInChildren<Item_Lock>().gameObject.SetActive(true);
            }
            else
            {
                go.GetComponentInChildren<Item_Lock>().gameObject.SetActive(false);
            }
        }

    }
} // end of class
