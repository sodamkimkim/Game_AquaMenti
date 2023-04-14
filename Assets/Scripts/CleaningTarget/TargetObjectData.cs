using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetObjectData : MonoBehaviour
{
    private List<Dictionary<string, object>> targetObjectList_ = new List<Dictionary<string, object>>();
    private void Awake()
    {
        CSVReader.Read("Datas/GameInfo/TargetInfo", out targetObjectList_);
    }
    public string GetKoreanName(string _ingameObjName)
    {
        string koreanName = "";
        for (int i = 0; i < targetObjectList_.Count; i++)
        {
            if (targetObjectList_[i]["TargetObjectName"].ToString().Equals(_ingameObjName.Trim()))
            {
                koreanName = targetObjectList_[i]["KoreanName"].ToString().Trim();
            }
        }
                return koreanName;
    }
    //public string 
} // end of class
