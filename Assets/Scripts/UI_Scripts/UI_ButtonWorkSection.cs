using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ButtonWorkSection : MonoBehaviour
{
    private Button btn = null;
    private UI_SectionDetailManager uI_SectionDetailManager = null;
    private UI_Manager ui_manager = null;

    private int selectedMapNum = 0;
    private int selectedSectionNum = 0;
    private void Awake()
    {
        btn = GetComponent<Button>();
    }
    private void Start()
    {
        selectedMapNum = int.Parse(name.Substring(name.Length - 3, 1));
        selectedSectionNum = int.Parse(name.Substring(name.Length - 1));

        btn.onClick.AddListener(() => OnClickSetSection(selectedMapNum, selectedSectionNum));
    }
    private void OnClickSetSection(int _selectedMapNum, int _selectedSectionNum)
    {
        ui_manager.GoToSectionDetailGo(_selectedSectionNum);
        uI_SectionDetailManager.SetWorkSectionDetail(_selectedMapNum, _selectedSectionNum);
    }
    public void SetUI_Managers(UI_SectionDetailManager _uI_SectionDetailManager, UI_Manager _ui_manager)
    {
        ui_manager = _ui_manager;
        uI_SectionDetailManager = _uI_SectionDetailManager;
    }
}
