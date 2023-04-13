using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLog : MonoBehaviour
{
    private List<string> missFileList_ = null;


    private void Awake()
    {
        FilePath.Init();
        FilePath.CheckFileState(out missFileList_);
    }
    private void Start()
    {
        PrintList(missFileList_);
    }


    private void PrintList(List<string> _list)
    {
        for(int i = 0; i < missFileList_.Count; ++i)
        {
            Debug.Log("[TestLog] " + i + "- Missing File Name: " + missFileList_[i]);
        }
    }
}
