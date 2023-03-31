using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuGo = null;
    [SerializeField] private GameObject sectionDetailMenuGo = null;
    [SerializeField] private GameObject workAreaMenuGo = null;
    [SerializeField] private GameObject beforeBtnGo = null;

    public void ActiveTogleBeforeBtn()
    {
        if(IsActiveBeforeBtnGo())
            beforeBtnGo.SetActive(false);
        else
            beforeBtnGo.SetActive(true);
    }

    public bool IsActiveBeforeBtnGo()
    {
        return beforeBtnGo.activeSelf;
    }
    public void SetMainMenu()
    {
        workAreaMenuGo.SetActive(false);
        mainMenuGo.SetActive(true);
    }
    public void SetWorkAreaMenu()
    {
        if(mainMenuGo.activeSelf)
            mainMenuGo.SetActive(false);
        if (sectionDetailMenuGo.activeSelf)
            sectionDetailMenuGo.SetActive(false);
        workAreaMenuGo.SetActive(true);
    }
    public void SetSectionDetailMenu()
    {
        workAreaMenuGo.SetActive(false);
        sectionDetailMenuGo.SetActive(true);
    }
}
