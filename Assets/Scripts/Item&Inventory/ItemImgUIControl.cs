using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemImgUIControl : MonoBehaviour
{
    private Image img_ = null;
    private Sprite[] ItemImgSprite_ = null;
    private void Awake()
    {
        img_ = GetComponent<Image>();
        ItemImgSprite_ = Resources.LoadAll<Sprite>("Textures\\UI\\Items");
    }
    public void SetItemImageUI(string _imgFileName)
    {
        foreach (Sprite sprite in ItemImgSprite_) { 
        if(sprite.name == _imgFileName)
            {
                img_.sprite = sprite;
            }
        }
    }
} // end of class
