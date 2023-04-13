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
        tmp_cleanimgPercentage.text = (int)_percentage*100f + "%";
    }
} // end of class
