using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager_ = null;

    [SerializeField] private GameObject enterPageGo_ = null;
    [SerializeField] private GameObject mainBookGo_ = null;
    [SerializeField] private GameObject mainGo_ = null;
    [SerializeField] private GameObject mapSelectGo_ = null;
    [SerializeField] private GameObject workSectionSelectGo_ = null;
    [SerializeField] private GameObject sectionDetailGo_ = null;
    [SerializeField] private GameObject workDetailGo_ = null;
    [SerializeField] private GameObject popupGo_ = null;


    [SerializeField] private UI_MenuManager ui_menuManager_ = null;
    [SerializeField] private Button[] btns_ = null;

    private UI_WorkAreaManager uI_WorkAreaManager_ = null;
    private UI_SectionDetailManager uI_SectionDetailManager_ = null;
    private GameObject currentContent_ = null;

    private int selectedMapNum_ = 0;
    private int selectedSectionNum_ = 0;

    private string nickName_ = string.Empty;
    private Color defaultNomalColor_;
    private void Awake()
    {
        uI_WorkAreaManager_ = workSectionSelectGo_.GetComponent<UI_WorkAreaManager>();
        uI_SectionDetailManager_ = sectionDetailGo_.GetComponent<UI_SectionDetailManager>();
    }
    private void Start()
    {
        defaultNomalColor_ = btns_[0].colors.normalColor;
    }

    private void OnValueChangedName(string _nickName)
    {
        nickName_ = _nickName;
    }
    public void EnterMainMenu()
    {
        if (!string.IsNullOrEmpty(nickName_))
        {
            enterPageGo_.SetActive(false);
            mainBookGo_.SetActive(true);
            currentContent_ = mainGo_;

            SetTapColor(0);
        }
    }
    public void GoToMain()
    {
        if (currentContent_ == mainGo_) return;
        if( ui_menuManager_.IsActiveBeforeBtnGo() )
            ui_menuManager_.ActiveTogleBeforeBtn();

        currentContent_.SetActive(false);
        mainGo_.SetActive(true);
        currentContent_ = mainGo_;

        SetTapColor(0);
    }
    public void GoToMapSelect()
    {
        if (currentContent_ == mapSelectGo_) return;
        if ( !ui_menuManager_.IsActiveBeforeBtnGo() )
            ui_menuManager_.ActiveTogleBeforeBtn();

        currentContent_.SetActive(false);
        mapSelectGo_.SetActive(true);
        currentContent_ = mapSelectGo_;

        SetTapColor(1);

    }
    public void GoToWorkSectionSelectGo(int _selectedMapNum)
    {
        if (_selectedMapNum == -1)
            _selectedMapNum = selectedMapNum_;
        if (currentContent_ == workSectionSelectGo_ && _selectedMapNum == selectedMapNum_ && _selectedMapNum == 0) return;


        if ( !ui_menuManager_.IsActiveBeforeBtnGo() )
            ui_menuManager_.ActiveTogleBeforeBtn();
        selectedMapNum_ = _selectedMapNum;
        ui_menuManager_.SetWorkAreaMenu();
        currentContent_.SetActive(false);
        workSectionSelectGo_.SetActive(true);
        uI_WorkAreaManager_.SetSectionContent(_selectedMapNum);
        currentContent_ = workSectionSelectGo_;
        SetTapColor(2);
    }
    public void GoToSectionDetailGo(int _selectedSectionNum)
    {
        if (currentContent_ == sectionDetailGo_ && _selectedSectionNum == selectedSectionNum_ && _selectedSectionNum == 0) return;

        if(_selectedSectionNum != -1)
            selectedSectionNum_ = _selectedSectionNum;

        ui_menuManager_.SetSectionDetailMenu();
        currentContent_.SetActive(false);
        sectionDetailGo_.SetActive(true);
        uI_SectionDetailManager_.SetWorkSectionDetail(selectedMapNum_, _selectedSectionNum);
        currentContent_ = sectionDetailGo_;

        SetTapColor(3);
    }
    public void GoToWorkDetailGo()
    {
        if (currentContent_ == workDetailGo_) return;
        currentContent_.SetActive(false);
        workDetailGo_.SetActive(true);
        currentContent_ = workDetailGo_;

        SetTapColor(4);
    }

    public void GoToBeforeGo()
    {
        if (currentContent_ == mapSelectGo_)
        {
            if (ui_menuManager_.IsActiveBeforeBtnGo())
                ui_menuManager_.ActiveTogleBeforeBtn();

            ui_menuManager_.SetMainMenu();
            currentContent_.SetActive(false);
            mainGo_.SetActive(true);

            SetTapColor(0);
            currentContent_ = mainGo_;
        }
        else if (currentContent_ == workSectionSelectGo_)
        {
            ui_menuManager_.SetMainMenu();
            currentContent_.SetActive(false);
            mapSelectGo_.SetActive(true);

            SetTapColor(1);
            currentContent_ = mapSelectGo_;
        }
        else if (currentContent_ == sectionDetailGo_)
        {
            ui_menuManager_.SetWorkAreaMenu();
            currentContent_.SetActive(false);
            workSectionSelectGo_.SetActive(true);

            SetTapColor(2);
            currentContent_ = workSectionSelectGo_;
        }
        else if (currentContent_ == workDetailGo_)
        {
            ui_menuManager_.SetSectionDetailMenu();
            currentContent_.SetActive(false);
            sectionDetailGo_.SetActive(true);

            SetTapColor(3);
            currentContent_ = sectionDetailGo_;
        }
    }

    private void SetTapColor(int _index)
    {

        for (int i = 0; i < btns_.Length; ++i)
        {
            ColorBlock cb = btns_[i].colors;

            if (btns_[_index] == btns_[i])
            {
                cb.normalColor = Color.grey;
                cb.selectedColor = Color.grey;
                btns_[i].colors = cb;

            }else
            {
                cb.normalColor = defaultNomalColor_;
                cb.selectedColor = defaultNomalColor_;
                btns_[i].colors = cb;
            }

        }
    }

    public void ExitGame()
    {
        popupGo_.SetActive(true);

    }
    public void ExitYes()
    {
        Application.Quit();
        Debug.Log("Á¾·á");
    }

    public void ExitNo()
    {
        popupGo_.SetActive(false);
    }

    public string GetScriptInfo()
    {
        return $" UI_Manager Detail\n nickName : {nickName_}\n selectedMapNum : {selectedMapNum_}\n selectedSectionNum : {selectedSectionNum_}\n CurrentContent : {currentContent_}\nGameManager Detail\n" +
            $"isStartGame_ : {gameManager_.isStartGame_}\n isInGame_ : {gameManager_.isInGame_}";
    }

    public void GetMapSectionNumber(out int _selectedMapNum, out int _selectedSectionNum)
    {
        _selectedMapNum = selectedMapNum_;
        _selectedSectionNum = selectedSectionNum_;
    }

}
