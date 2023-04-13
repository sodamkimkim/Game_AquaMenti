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
        if (this.gameObject.CompareTag("ScreenSideTop"))
        {
            screenSideManager_.isScreenSideTop = true; // Debug.Log("isScreenSideTop == true");
        }
        if (this.gameObject.CompareTag("ScreenSideBottom"))
        {
            screenSideManager_.isScreenSideBottom = true; // Debug.Log("isScreenSideBottom == true");
        }
        if (this.gameObject.CompareTag("ScreenSideLeft"))
        {
            screenSideManager_.isScreenSideLeft = true; // Debug.Log("isScreenSideLeft == true");
        }
        if (this.gameObject.CompareTag("ScreenSideRight"))
        {
            screenSideManager_.isScreenSideRight = true; // Debug.Log("isScreenSideRight == true");
        }
    }
    public void OnPointerExit(PointerEventData _eventData)
    {
        if (this.gameObject.CompareTag("ScreenSideTop"))
        {
            screenSideManager_.isScreenSideTop = false;
        }
        if (this.gameObject.CompareTag("ScreenSideBottom"))
        {
            screenSideManager_.isScreenSideBottom = false;
        }
        if (this.gameObject.CompareTag("ScreenSideLeft"))
        {
            screenSideManager_.isScreenSideLeft = false;
        }
        if (this.gameObject.CompareTag("ScreenSideRight")) { screenSideManager_.isScreenSideRight = false; }
    }
} // end of class
