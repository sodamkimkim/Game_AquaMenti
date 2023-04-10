using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemDescriptionUIControl : MonoBehaviour
{
    public void SetItemDescriptionUI(string _itemDescription)
    {
        this.gameObject.GetComponent<TextMeshProUGUI>().text = _itemDescription;
    }
} // end of class

