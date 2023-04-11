using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipedItem : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI equipedItemName_ = null;
    [SerializeField]
    private Image equipedItemImg_ = null;
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
    }
    private void Start()
    {
        SetEquipedItemUI();
    }
    public void SetEquipedItemUI()
    {
        equipedItemName_.text = itemName;
        equipedItemDescription_.text = description;
        if (itemImgSpArr_ != null)
        {
            foreach (Sprite sprite in itemImgSpArr_)
            {
                if (sprite.name == imageFileName)
                {
                    equipedItemImg_.sprite = sprite;
                }
            }

        }
    }
}
