using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CheckScreenSideMouseHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private ScreenSideManager screenSideManager_;
    private void Awake()
    {
        screenSideManager_ = GetComponentInParent<ScreenSideManager>();
    }
    public void OnPointerEnter(PointerEventData _eventData)
    {
        if (this.gameObject.CompareTag("ScreenSideTop")) { screenSideManager_.isScreenSideTop = true; Debug.Log("isScreenSideTop == true"); }
        if (this.gameObject.CompareTag("ScreenSideBottom")) { screenSideManager_.isScreenSideBottom = true; Debug.Log("isScreenSideBottom == true"); }
        if (this.gameObject.CompareTag("ScreenSideLeft")) { screenSideManager_.isScreenSideLeft = true; Debug.Log("isScreenSideLeft == true"); }
        if (this.gameObject.CompareTag("ScreenSideRight")) { screenSideManager_.isScreenSideRight = true; Debug.Log("isScreenSideRight == true"); }
    }
    public void OnPointerExit(PointerEventData _eventData)
    {
        if (this.gameObject.CompareTag("ScreenSideTop")) { screenSideManager_.isScreenSideTop = false; Debug.Log("isScreenSideTop == false"); }
        if (this.gameObject.CompareTag("ScreenSideBottom")) { screenSideManager_.isScreenSideBottom = false; Debug.Log("isScreenSideBottom == false"); }
        if (this.gameObject.CompareTag("ScreenSideLeft")) { screenSideManager_.isScreenSideLeft = false; Debug.Log("isScreenSideLeft == false"); }
        if (this.gameObject.CompareTag("ScreenSideRight")) { screenSideManager_.isScreenSideRight = false; Debug.Log("isScreenSideRight == false"); }
    }
} // end of class
