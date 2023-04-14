using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SectionDetailManager : MonoBehaviour
{
    [SerializeField] private GameDataManager gameDataManager_ = null;
    [SerializeField] private UI_Manager uI_Manager_ = null;

    [SerializeField] private TextMeshProUGUI intermediaryTMPGUI_ = null;
    [SerializeField] private TextMeshProUGUI storyDescriptionTMPGUI_ = null;
    private List<Dictionary<string, object>> workSectionDataList = null;



    public void SetWorkSectionDetail(int _selectedMapNum, int _selectedSectionNum)
    {
        gameDataManager_.GetMapDataList(out workSectionDataList);
        foreach (Dictionary<string, object> workSectionDics in workSectionDataList)
        {
            if(workSectionDics["MapNumber"].ToString() == $"{_selectedMapNum}"&& workSectionDics["SectionNumber"].ToString() == $"{_selectedSectionNum}")
            {
                intermediaryTMPGUI_.text = $"의뢰인 : {workSectionDics["Intermediary"].ToString()}";
                storyDescriptionTMPGUI_.text = $"To 작업자 에게..\n{workSectionDics["StoryBegin"].ToString()}";
            }
        }
    }
}
