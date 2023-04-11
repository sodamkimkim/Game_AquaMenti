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
    private float uiWidth_ = 550f;

    private RectTransform rtr   = null;

    private void Awake()
    {  
        rtr = this.gameObject.GetComponent<RectTransform>();    
        SettSpellUIList();
        InstantiateUIPrefabs();
    }
    private void Update()
    {
        Debug.Log(rtr.sizeDelta.x);
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
            newPos.x = i * (xOffset_ + uiWidth_)+xOffset_;
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
        if(rtr!=null)
        {

        float contentWidth =0;
        contentWidth = itemSpellInfoList_.Count * (xOffset_ + uiWidth_)-(3*xOffset_ + 2*uiWidth_);
        Vector2 sizedelta = rtr.sizeDelta;
        rtr.sizeDelta = new Vector2(contentWidth, sizedelta.y);
        }
    }

} // end of class
