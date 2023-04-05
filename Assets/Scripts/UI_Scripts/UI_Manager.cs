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
    [SerializeField] private GameObject workDetailGo = null;
    [SerializeField] private UI_WorkAreaManager uI_WorkAreaManager = null;
    [SerializeField] private UI_SectionDetailManager uI_SectionDetailManager = null;

    [SerializeField] private Button[] btns = null;

    [SerializeField] private UI_MenuManager ui_menuManager = null;

    private GameObject currentContent = null;

    private int selectedMapNum = 0;
    private int selectedSectionNum = 0;

    private string nickName = string.Empty;
    private Color defaultNomalColor;

    private void Start()
    {
        defaultNomalColor = btns[0].colors.normalColor;
    }

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

            SetTapColor(0);
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

        SetTapColor(0);
    }
    public void GoToMapSelect()
    {
        if (currentContent == mapSelectGo) return;
        if ( !ui_menuManager.IsActiveBeforeBtnGo() )
            ui_menuManager.ActiveTogleBeforeBtn();

        currentContent.SetActive(false);
        mapSelectGo.SetActive(true);
        currentContent = mapSelectGo;

        SetTapColor(1);

    }
    public void GoToWorkSectionSelectGo(int _selectedMapNum)
    {
        if (_selectedMapNum == -1)
            _selectedMapNum = selectedMapNum;
        if (currentContent == workSectionSelectGo && _selectedMapNum == selectedMapNum && _selectedMapNum == 0) return;


        if ( !ui_menuManager.IsActiveBeforeBtnGo() )
            ui_menuManager.ActiveTogleBeforeBtn();
        selectedMapNum = _selectedMapNum;
        ui_menuManager.SetWorkAreaMenu();
        currentContent.SetActive(false);
        workSectionSelectGo.SetActive(true);
        uI_WorkAreaManager.SetSectionContent(_selectedMapNum);
        currentContent = workSectionSelectGo;
        SetTapColor(2);
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

        SetTapColor(3);
    }
    public void GoToWorkDetailGo()
    {
        if (currentContent == workDetailGo) return;
        currentContent.SetActive(false);
        workDetailGo.SetActive(true);
        currentContent = workDetailGo;

        SetTapColor(4);
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

            SetTapColor(0);
            currentContent = mainGo;
        }
        else if (currentContent == workSectionSelectGo)
        {
            ui_menuManager.SetMainMenu();
            currentContent.SetActive(false);
            mapSelectGo.SetActive(true);

            SetTapColor(1);
            currentContent = mapSelectGo;
        }
        else if (currentContent == sectionDetailGo)
        {
            ui_menuManager.SetWorkAreaMenu();
            currentContent.SetActive(false);
            workSectionSelectGo.SetActive(true);

            SetTapColor(2);
            currentContent = workSectionSelectGo;
        }
        else if (currentContent == workDetailGo)
        {
            ui_menuManager.SetSectionDetailMenu();
            currentContent.SetActive(false);
            sectionDetailGo.SetActive(true);

            SetTapColor(3);
            currentContent = sectionDetailGo;
        }
    }

    private void SetTapColor(int _index)
    {

        for (int i = 0; i < btns.Length; ++i)
        {
            ColorBlock cb = btns[i].colors;

            if (btns[_index] == btns[i])
            {
                cb.normalColor = Color.grey;
                cb.selectedColor = Color.grey;
                btns[i].colors = cb;

            }else
            {
                cb.normalColor = defaultNomalColor;
                cb.selectedColor = defaultNomalColor;
                btns[i].colors = cb;
            }

        }
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
