using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameManager : MonoBehaviour
{
    [SerializeField]
    private UI_Manager uI_Manager_ = null;
    [SerializeField]
    private MeshPaintManager meshPaintManager_ = null;

    private List<string> missFileList_ = null;
    //private int mapNum_ = 0;
    //private int sectionNum_ = 0;


    private void Awake()
    {
        // 1) Save 경로를 생성합니다. (이미 있다면 생성을 하지 않습니다. - 조건문 실행이기 때문)
        FilePath.Init();

        // 2) Resources 경로의 파일과 비교하여 Save 경로의 파일이 누락 되었는지 확인을 합니다.
        // 2-1) 하나라도 누락이 되었다면 false를 반환하고 List에 담습니다.
        bool isOk = FileIO.CheckFileState(out missFileList_, "*.png");
        // 3) 누락되었다면
        if (isOk == false)
        {
            // 3-1)손실된 파일을 복구합니다.
            foreach (string file in missFileList_)
            {
                string[] split = file.Split("/");
                string fileName = split[split.Length - 1];
                split[split.Length - 1] = null; // FileName은 경로에 포함시키지 않기 위해 null 처리합니다.
                string dir = string.Join("/", split); // Split한 문자열들을 다시 통합시킵니다.

#if UNITY_EDITOR
                //Debug.Log("[TestLog] dir: " + dir);
#endif
                // 3-2) Resources 경로로부터 Save경로로 파일을 복사합니다.
                FileIO.CopyFile(
                    FilePath.ASSETS_MAP_PATH + dir,
                    FilePath.SAVE_PATH + dir,
                    fileName);
            }
        }
    }

    private void Start()
    {
#if UNITY_EDITOR
        // 누락된 파일들 확인용
        PrintList(missFileList_);
#endif

        // 4) 누락되거나 손실된 파일을 복구하였다면 기초 준비가 끝난 상태로 판단합니다.
        // Scene에 있는 모든 MeshPaintTarget을 가진 Object에 대해 Mask Texture를 불러옵니다.
        meshPaintManager_.Init(uI_Manager_.LoadingStart, uI_Manager_.LoadingEnd);
    }


    private void PrintList(List<string> _list)
    {
        for(int i = 0; i < _list.Count; ++i)
        {
            Debug.Log("[TestLog] " + i + "- Missing File Name: " + _list[i]);
        }
    }
}
