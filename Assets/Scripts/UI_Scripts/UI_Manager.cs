using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{

    [SerializeField] private GameObject enterPageGo = null;
    [SerializeField] private GameObject mainBookGo = null;
    [SerializeField] private GameObject mainGo = null;
    [SerializeField] private GameObject mapSelectGo = null;
    [SerializeField] private GameObject workSectionSelectGo = null;
    [SerializeField] private GameObject sectionDetailGo = null;
    [SerializeField] private GameObject detailGo = null;
    [SerializeField] private UI_WorkAreaManager uI_WorkAreaManager = null;
    [SerializeField] private UI_SectionDetailManager uI_SectionDetailManager = null;

    [SerializeField] private UI_MenuManager ui_menuManager = null;

    private GameObject currentContent = null;
    private GameObject beforeContent = null;

    private int selectedMapNum = 0;
    private int selectedSectionNum = 0;

    private string nickName = string.Empty;


    private void OnValueChangedName(string _nickName)
    {
        nickName = _nickName;
    }
    public void EnterMainMenu()
    {
        if (!string.IsNullOrEmpty(nickName))
        {
            enterPageGo.SetActive(false);
            mainBookGo.SetActive(true);
            currentContent = mainGo;
            beforeContent = mainGo;
        }
    }
    public void GoToMain()
    {
        if (currentContent == mainGo) return;
        if( ui_menuManager.IsActiveBeforeBtnGo() )
            ui_menuManager.ActiveTogleBeforeBtn();

        currentContent.SetActive(false);
        mainGo.SetActive(true);
        currentContent = mainGo;
    }
    public void GoToMapSelect()
    {
        if (currentContent == mapSelectGo) return;
        if ( !ui_menuManager.IsActiveBeforeBtnGo() )
            ui_menuManager.ActiveTogleBeforeBtn();

        currentContent.SetActive(false);
        mapSelectGo.SetActive(true);
        currentContent = mapSelectGo;
        beforeContent = mainGo;

    }
    public void GoToWorkSectionSelectGo(int _selectedMapNum)
    {
        if (_selectedMapNum == -1)
            _selectedMapNum = selectedMapNum;
        if (currentContent == workSectionSelectGo && _selectedMapNum == selectedMapNum && _selectedMapNum == 0) return;


        if ( !ui_menuManager.IsActiveBeforeBtnGo() )
            ui_menuManager.ActiveTogleBeforeBtn();

        ui_menuManager.SetWorkAreaMenu();
        currentContent.SetActive(false);
        workSectionSelectGo.SetActive(true);
        uI_WorkAreaManager.SetSectionContent(_selectedMapNum);
        currentContent = workSectionSelectGo;
        beforeContent = mapSelectGo;
    }
    public void GoToSectionDetailGo(int _selectedSectionNum)
    {
        if (currentContent == sectionDetailGo && _selectedSectionNum == selectedSectionNum && _selectedSectionNum == 0) return;

        if(_selectedSectionNum != -1)
            selectedSectionNum = _selectedSectionNum;

        ui_menuManager.SetSectionDetailMenu();
        currentContent.SetActive(false);
        sectionDetailGo.SetActive(true);
        uI_SectionDetailManager.SetWorkSectionDetail(selectedMapNum, _selectedSectionNum);
        currentContent = sectionDetailGo;
        beforeContent = workSectionSelectGo;
    }
    public void GoToWorkDetailGo()
    {
        if (currentContent == detailGo) return;
        currentContent.SetActive(false);
        detailGo.SetActive(true);
        currentContent = detailGo;
        beforeContent = sectionDetailGo;
    }

    public void GoToBeforeGo()
    {
        if (currentContent == mapSelectGo)
        {
            if (ui_menuManager.IsActiveBeforeBtnGo())
                ui_menuManager.ActiveTogleBeforeBtn();

            ui_menuManager.SetMainMenu();
            currentContent.SetActive(false);
            mainGo.SetActive(true);

            currentContent = mainGo;
            beforeContent = mapSelectGo;
        }
        else if (currentContent == workSectionSelectGo)
        {
            ui_menuManager.SetMainMenu();
            currentContent.SetActive(false);
            mapSelectGo.SetActive(true);

            currentContent = mapSelectGo;
            beforeContent = workSectionSelectGo;
        }
        else if (currentContent == sectionDetailGo)
        {
            ui_menuManager.SetWorkAreaMenu();
            currentContent.SetActive(false);
            workSectionSelectGo.SetActive(true);

            currentContent = workSectionSelectGo;
            beforeContent = sectionDetailGo;
        }
        else if (currentContent == detailGo)
        {
            ui_menuManager.SetSectionDetailMenu();
            currentContent.SetActive(false);
            sectionDetailGo.SetActive(true);

            currentContent = sectionDetailGo;
            beforeContent = detailGo;
        }
    }

    public bool IsSameContent()
    {
        return (currentContent == beforeContent) ? true : false;
    }

    public string GetScriptInfo()
    {
        return $" UI_Manager Detail\n nickName : {nickName}\n selectedMapNum : {selectedMapNum}\n selectedSectionNum : {selectedSectionNum}\n CurrentContent : {currentContent}\n";
    }

/*    public void GetMapSectionNumber(out int _selectedMapNum, out int _selectedSectionNum)
    {
        _selectedMapNum = selectedMapNum;
        _selectedSectionNum = selectedSectionNum;
    }*/

}
