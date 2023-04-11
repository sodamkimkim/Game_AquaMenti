using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour
{
    public delegate void ItemSelectCallback_(NowWearingInfo.NowWearingItem _selectItem);
    public ItemSelectCallback_ itemSelectCallback_;
    private Button btnItem;
    public string itemCategory { get; set; }
    public string itemName { get; set; }
    public string description { get; set; }
    public string imageFileName { get; set; }
    public string useable { get; set; }
    public void Init(ItemSelectCallback_ _callback)
    {
        itemSelectCallback_ += _callback;
    }
    private void Awake()
    {
        btnItem = this.gameObject.GetComponentInChildren<Button>();
        btnItem.onClick.AddListener(ItemClickCallback);

    }
    public void SetUI()
    {
        ItemNameUIControl itemNameUIControl = this.gameObject.GetComponentInChildren<ItemNameUIControl>();
        ItemImgUIControl itemImgUIControl = this.gameObject.GetComponentInChildren<ItemImgUIControl>();
        ItemDescriptionUIControl itemDescriptionUIControl = this.gameObject.GetComponentInChildren<ItemDescriptionUIControl>();

        itemNameUIControl.SetItemNameUI(itemName);
        itemImgUIControl.SetItemImageUI(imageFileName);
        itemDescriptionUIControl.SetItemDescriptionUI(description);
        if (useable.Equals("FALSE")) // 사용 불가능 상태
        {
            this.gameObject.GetComponentInChildren<Item_Lock>().gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.GetComponentInChildren<Item_Lock>().gameObject.SetActive(false);
        }
    }
    private void ItemClickCallback()
    {
        NowWearingInfo.NowWearingItem selectItemInfo = new NowWearingInfo.NowWearingItem(itemCategory, itemName, imageFileName, description);
        Debug.Log(selectItemInfo.ToString());
        itemSelectCallback_.Invoke(selectItemInfo);
    }
}
