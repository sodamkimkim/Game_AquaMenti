using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipedStaff : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI equipedItemName_ = null;
    [SerializeField]
    private Image equipedItemImg = null;
    [SerializeField]
    private TextMeshProUGUI equipedItemDescription_ = null;

    private Sprite[] itemImgSpArr_ = null;

    public string itemCategory { get; set; }
    public string itemName { get; set; }
    public string description { get; set; }
    public string imageFileName { get; set; }
    private void Awake()
    {
        itemImgSpArr_ = Resources.LoadAll<Sprite>("Textures\\UI\\Items");
        //itemCategory = "Staff";
        //itemName = "AmberStaff";
        //description = "호박보석이 박혀있는 지팡이";
        //imageFileName = "AmberStaff";
        SetEquipedItemUI();
    }
    public void SetEquipedItemUI()
    {
        equipedItemName_.text = itemName;
        equipedItemDescription_.text = description;
        foreach (Sprite sprite in itemImgSpArr_)
        {
            if (sprite.name == imageFileName)
            {
                equipedItemImg.sprite = sprite;
            }
        }
    }
}
