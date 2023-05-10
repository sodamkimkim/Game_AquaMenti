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
    [SerializeField] private TextMeshProUGUI titleTMPGUI_ = null;
    private List<Dictionary<string, object>> workSectionDataList = null;



    public void SetWorkSectionDetail(int _selectedMapNum, int _selectedSectionNum)
    {
        gameDataManager_.GetMapDataList(out workSectionDataList);
        foreach (Dictionary<string, object> workSectionDics in workSectionDataList)
        {
            if(workSectionDics["MapNumber"].ToString() == $"{_selectedMapNum}"&& workSectionDics["SectionNumber"].ToString() == $"{_selectedSectionNum}")
            {
                intermediaryTMPGUI_.text = $"의뢰인 : {workSectionDics["Intermediary"].ToString()}";
                storyDescriptionTMPGUI_.text = $"To {uI_Manager_.nickName_} 에게..\n\n{workSectionDics["StoryBegin"].ToString()}";
                titleTMPGUI_.text = $"{workSectionDics["Intermediary"].ToString()}의\n 의뢰 청소 하기";
            }
        }
    }
}
