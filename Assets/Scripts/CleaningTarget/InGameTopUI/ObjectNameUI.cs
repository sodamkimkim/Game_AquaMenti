using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectNameUI : MonoBehaviour
{
    private TextMeshProUGUI tmp_objectName_ = null;

    private void Awake()
    {
        tmp_objectName_ = this.gameObject.GetComponent<TextMeshProUGUI>();  
    }

    public void SetObjectName(string _objectName)
    {
        tmp_objectName_.text = _objectName;
    }
} // end of class
