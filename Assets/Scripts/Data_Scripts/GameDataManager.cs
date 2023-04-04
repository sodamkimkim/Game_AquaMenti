using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameDataManager : MonoBehaviour
{
    private List<Dictionary<string, object>> sectionStoryList = null;
    private List<Dictionary<string, object>> eventMessageList = null;
    private void Awake()
    {
        sectionStoryList = CSVReader.Read("Datas/GameInfo/SectionStory");
        eventMessageList = CSVReader.Read("Datas/GameInfo/EventMessage");
    }
    public void GetMapDataList(out List<Dictionary<string, object>> _sectionStoryList)
    {
        _sectionStoryList = sectionStoryList;
    }
    public void GetEventMessageDataList(out List<Dictionary<string, object>> _eventMessageList)
    {
        _eventMessageList = eventMessageList;
    }
}