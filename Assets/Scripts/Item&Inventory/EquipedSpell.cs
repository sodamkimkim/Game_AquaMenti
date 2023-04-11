using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipedSpell : MonoBehaviour
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

    private void Start()
    {
        itemImgSpArr_ = Resources.LoadAll<Sprite>("Textures\\UI\\Items");
        //itemCategory = "Spell";
        //itemName = "Deg0MagicSpell";
        //description = "각도0도로 분출되는 물 마법";
        //imageFileName = "Deg0MagicSpell";

        SetEquipedItemUI();
    }
    public void SetEquipedItemUI()
    {
        equipedItemName_.text = itemName;
        equipedItemDescription_ .text = description;
        
        foreach (Sprite sprite in itemImgSpArr_)
        {
            if (sprite.name == imageFileName)
            {
                equipedItemImg.sprite = sprite;
            }
        }
    }
}
