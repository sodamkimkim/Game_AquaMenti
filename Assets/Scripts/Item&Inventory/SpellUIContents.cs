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


    private void Start()
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

            // UI 셋팅
            ItemNameUIControl itemNameUIControl = go.GetComponentInChildren<ItemNameUIControl>();
            ItemImgUIControl itemImgUIControl = go.GetComponentInChildren<ItemImgUIControl>();
            ItemDescriptionUIControl itemDescriptionUIControl = go.GetComponentInChildren<ItemDescriptionUIControl>();

            itemNameUIControl.SetItemNameUI(itemSpellInfoList_[i]["ItemName"].ToString());
            itemImgUIControl.SetItemImageUI(itemSpellInfoList_[i]["ImageFileName"].ToString());
            itemDescriptionUIControl.SetItemDescriptionUI(itemSpellInfoList_[i]["Description"].ToString());

            if (itemSpellInfoList_[i]["Useable"].Equals("FALSE")) // 사용 불가능 상태
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
