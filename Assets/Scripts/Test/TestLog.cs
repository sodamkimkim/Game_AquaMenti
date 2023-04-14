using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLog : MonoBehaviour
{
    private List<string> missFileList_ = null;


    private void Awake()
    {
        FilePath.Init();
        FileIO.CheckFileState(out missFileList_, "*.png");
        byte[] bytes = FileIO.GetFileBinary(FilePath.MAP_1_SECTION_1_PATH, "Test_Box.png");
        if (bytes == null)
            FileIO.CopyFile(
                FilePath.RESOURCES_MAP_1_SECTION_1_PATH,
                FilePath.MAP_1_SECTION_1_PATH,
                "Test_Box.png",
                true);
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
