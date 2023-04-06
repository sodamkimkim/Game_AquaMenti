using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuGo_ = null;
    [SerializeField] private GameObject workAreaMenuGo_ = null;
    [SerializeField] private GameObject sectionDetailMenuGo_ = null;
    [SerializeField] private GameObject beforeBtnGo_ = null;

    public void ActiveTogleBeforeBtn()
    {
        if(IsActiveBeforeBtnGo())
            beforeBtnGo_.SetActive(false);
        else
            beforeBtnGo_.SetActive(true);
    }

    public bool IsActiveBeforeBtnGo()
    {
        return beforeBtnGo_.activeSelf;
    }
    public void SetMainMenu()
    {
        workAreaMenuGo_.SetActive(false);
        mainMenuGo_.SetActive(true);
    }
    public void SetWorkAreaMenu()
    {
        if(mainMenuGo_.activeSelf)
            mainMenuGo_.SetActive(false);
        if (sectionDetailMenuGo_.activeSelf)
            sectionDetailMenuGo_.SetActive(false);
        workAreaMenuGo_.SetActive(true);
    }
    public void SetSectionDetailMenu()
    {
        workAreaMenuGo_.SetActive(false);
        sectionDetailMenuGo_.SetActive(true);
    }
}
