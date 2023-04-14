using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshPaintManager : MonoBehaviour
{
    // DirtyMask를 추출하여 저장하는 용도. DirtyMask를 추출 후 사용중지될 예정.
    private List<MeshPaintTarget> meshTargetList_ = null; // InGame MeshPaintTarget List
    private bool saveChecker_ = false;

    private void Awake()
    {
        // Scene에 존재하는 Object를 대상으로 MeshPaintTarget을 가져옴.
        MeshPaintTarget[] targets = FindObjectsOfType<MeshPaintTarget>();
        meshTargetList_ = new List<MeshPaintTarget>(targets);
#if UNITY_EDITOR
        Debug.Log("[MeshPaintManager] target Count: " + meshTargetList_.Count);
#endif
    }
    private void Start()
    {
        //LoadTargetMask(0, 0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SaveTargetMask(0, 0);
        }
    }

    private void LateUpdate()
    {
        //if (saveChecker_ == false)
        //{
        //    saveChecker_ = true;
        //    SaveTargetMask(0, 0);
        //}
    }


    public void CopyInitFiles()
    {
        string sourceDir = FilePath.RESOURCES_MAP_PATH;
        string destinationDir = FilePath.SAVE_PATH;

        FileIO.CopyDirectory(sourceDir, destinationDir, "*.png");
    }

    public void SaveTargetMask(int _mapNum, int _sectionNum)
    {
        foreach (MeshPaintTarget target_ in meshTargetList_)
        {
            if (target_.IsDrawable())
            {
                string path = FilePath.GetPath(
                    FilePath.EPathType.RESOURCES,
                    (FilePath.EMapType)_mapNum,
                    (FilePath.ESection)_sectionNum
                    );

                target_.SaveMask(path);
            }
        }
    }

    public void LoadTargetMask(int _mapNum, int _sectionNum)
    {
        foreach (MeshPaintTarget target_ in meshTargetList_)
        {
            if (target_.IsDrawable())
            {
                string path = FilePath.GetPath(
                    FilePath.EPathType.EXTERNAL,
                    (FilePath.EMapType)_mapNum,
                    (FilePath.ESection)_sectionNum
                    );

                target_.LoadMask(path);
            }
        }
    }

}
