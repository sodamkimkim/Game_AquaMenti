using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CleaningProgressPanUI : MonoBehaviour
{
    private Image img_progressPan = null;

    private void Awake()
    {
        img_progressPan = GetComponent<Image>();    
    }
    public void SetCleaningProgressImgFillAmt(float _fillAmount)
    {
        img_progressPan.fillAmount = _fillAmount;
    }
} // end of class
