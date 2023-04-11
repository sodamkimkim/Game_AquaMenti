using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemNameUIControl : MonoBehaviour
{
    public void SetItemNameUI(string _itemName)
    {
        this.gameObject.GetComponent<TextMeshProUGUI>().text = _itemName;   
    }
} // end of class
