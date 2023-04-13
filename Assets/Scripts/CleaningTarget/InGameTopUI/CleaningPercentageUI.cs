using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CleaningPercentageUI : MonoBehaviour
{
    private TextMeshProUGUI tmp_cleanimgPercentage = null;

    private void Awake()
    {
        tmp_cleanimgPercentage = GetComponent<TextMeshProUGUI>();   
    }
    public void SetCleaningPercentageUI(float _percentage)
    {
        tmp_cleanimgPercentage.text = _percentage*100f + "%";
    }
    public void SetActive(bool _para)
    {
        this.gameObject.SetActive(_para);
    }
} // end of class
