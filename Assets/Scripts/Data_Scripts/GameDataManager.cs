using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameDataManager : MonoBehaviour
{
    private List<Dictionary<string, object>> sectionStoryList_ = null;
    private List<Dictionary<string, object>> eventMessageList_ = null;
    private void Awake()
    {
        CSVReader.Read("Datas/GameInfo/SectionStory", out sectionStoryList_);
        CSVReader.Read("Datas/GameInfo/EventMessage", out eventMessageList_);
    }
    public void GetMapDataList(out List<Dictionary<string, object>> _sectionStoryList)
    {
        _sectionStoryList = sectionStoryList_;
    }
    public void GetEventMessageDataList(out List<Dictionary<string, object>> _eventMessageList)
    {
        _eventMessageList = eventMessageList_;
    }
}