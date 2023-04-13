using System.IO;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilePath
{
    public enum EPathType { EXTERNAL, RESOURCES, LEN }
    public enum EMapType { MAP_1, MAP_2, LEN }
    public enum ESection { SECTION_1, SECTION_2, SECTION_3, SECTION_4, SECTION_5, LEN }

    // PersistentDataPath //
    public static readonly string EXTERNAL_PATH = Application.persistentDataPath;

    public static readonly string SAVE_PATH = Application.persistentDataPath + "/Saves";

    public static readonly string MAP_1_PATH = Application.persistentDataPath + "/Saves/Map_1";
    public static readonly string MAP_1_SECTION_1_PATH = Application.persistentDataPath + "/Saves/Map_1/Section_1";
    public static readonly string MAP_1_SECTION_2_PATH = Application.persistentDataPath + "/Saves/Map_1/Section_2";
    public static readonly string MAP_1_SECTION_3_PATH = Application.persistentDataPath + "/Saves/Map_1/Section_3";
    public static readonly string MAP_1_SECTION_4_PATH = Application.persistentDataPath + "/Saves/Map_1/Section_4";
    public static readonly string MAP_1_SECTION_5_PATH = Application.persistentDataPath + "/Saves/Map_1/Section_5";
    
    public static readonly string MAP_2_PATH = Application.persistentDataPath + "/Saves/Map_2";
    public static readonly string MAP_2_SECTION_1_PATH = Application.persistentDataPath + "/Saves/Map_2/Section_1";
    public static readonly string MAP_2_SECTION_2_PATH = Application.persistentDataPath + "/Saves/Map_2/Section_2";
    public static readonly string MAP_2_SECTION_3_PATH = Application.persistentDataPath + "/Saves/Map_2/Section_3";
    //

    // DataPath //
    public static readonly string DATA_PATH = Application.dataPath; // Assets

    public static readonly string RESOURCES_PATH = Application.dataPath + "/Resources";
    public static readonly string RESOURCES_MAP_PATH = Application.dataPath + "/Resources/Maps";

    public static readonly string RESOURCES_MAP_1_PATH = Application.dataPath + "/Resources/Maps/Map_1";
    public static readonly string RESOURCES_MAP_1_SECTION_1_PATH = Application.persistentDataPath + "/Resources/Maps/Map_1/Section_1";
    public static readonly string RESOURCES_MAP_1_SECTION_2_PATH = Application.persistentDataPath + "/Resources/Maps/Map_1/Section_2";
    public static readonly string RESOURCES_MAP_1_SECTION_3_PATH = Application.persistentDataPath + "/Resources/Maps/Map_1/Section_3";
    public static readonly string RESOURCES_MAP_1_SECTION_4_PATH = Application.persistentDataPath + "/Resources/Maps/Map_1/Section_4";
    public static readonly string RESOURCES_MAP_1_SECTION_5_PATH = Application.persistentDataPath + "/Resources/Maps/Map_1/Section_5";

    public static readonly string RESOURCES_MAP_2_PATH = Application.dataPath + "/Resources/Maps/Map_2";
    public static readonly string RESOURCES_MAP_2_SECTION_1_PATH = Application.persistentDataPath + "/Resources/Maps/Map_2/Section_1";
    public static readonly string RESOURCES_MAP_2_SECTION_2_PATH = Application.persistentDataPath + "/Resources/Maps/Map_2/Section_2";
    public static readonly string RESOURCES_MAP_2_SECTION_3_PATH = Application.persistentDataPath + "/Resources/Maps/Map_2/Section_3";
    //

    public static readonly string SAVE_PATH_NAME = "Saves";
    public static readonly string RESOURCES_MAP_PATH_NAME = "Maps";

    public static readonly string MAP_1_PATH_NAME = "Map_1";
    public static readonly string MAP_2_PATH_NAME = "Map_2";
    public static readonly string SECTION_1_PATH_NAME = "Section_1";
    public static readonly string SECTION_2_PATH_NAME = "Section_2";
    public static readonly string SECTION_3_PATH_NAME = "Section_3";
    public static readonly string SECTION_4_PATH_NAME = "Section_4";
    public static readonly string SECTION_5_PATH_NAME = "Section_5";


    /// <summary>
    /// 기본 Directory 경로 생성
    /// </summary>
    public static void Init()
    {
        // External 영역에 기본 Directory 생성
        CreateDirectory(SAVE_PATH);

        CreateDirectory(MAP_1_PATH);
        CreateDirectory(MAP_1_SECTION_1_PATH);
        CreateDirectory(MAP_1_SECTION_2_PATH);
        CreateDirectory(MAP_1_SECTION_3_PATH);
        CreateDirectory(MAP_1_SECTION_4_PATH);
        CreateDirectory(MAP_1_SECTION_5_PATH);

        CreateDirectory(MAP_2_PATH);
        CreateDirectory(MAP_2_SECTION_1_PATH);
        CreateDirectory(MAP_2_SECTION_2_PATH);
    }


    /// <summary>
    /// <paramref name="_sourceDir"/>에 있는 <paramref name="_fileType"/>형식의 파일들을 <paramref name="_destinationDir"/>에 복사합니다.<br/>
    /// <paramref name="_recursive"/>을 통해 하위 Directory까지 복사할 것인지 정할 수 있습니다.
    /// </summary>
    /// <param name="_sourceDir">복사할 Directory</param>
    /// <param name="_destinationDir">대상인 Directory</param>
    /// <param name="_fileType">복사하고자 하는 파일형식 | default: "*"</param>
    /// <param name="_recursive">하위 Directory까지 복사할 것인지 여부 | default: false</param>
    public static void CopyDirectory(string _sourceDir, string _destinationDir, string _fileType = "*", bool _recursive = false)
    {
        if (Directory.Exists(_destinationDir) == false)
            CreateDirectory(_destinationDir);

        var dir = new DirectoryInfo(_sourceDir);

        foreach (FileInfo file in dir.GetFiles(_fileType))
        {
            string path = Path.Combine(_destinationDir, file.Name);
            file.CopyTo(path);
        }

        if (_recursive)
        {
            DirectoryInfo[] dirs = dir.GetDirectories(_sourceDir);
            foreach (DirectoryInfo subDir in dirs)
            {
                string newDestinationDir = Path.Combine(_destinationDir, subDir.Name);
                CopyDirectory(_destinationDir, newDestinationDir, _fileType, true);
            }
        }
    }

    /// <summary>
    /// External 경로와 Resources 경로를 확인하여 손실된 것이 없다면 true를 반환하고 손실된 것이 있다면 false를 반환합니다.
    /// </summary>
    /// <param name="_missList">손실된 Directory의 Name을 담습니다.</param>
    /// <returns>true: 모든 파일이 존재 | false: 손실된 파일이 존재</returns>
    public static bool CheckFileState(out List<string> _missList)
    {
        bool checker = true;
        // System.Linq 사용
        var externalFiles = from file in Directory.EnumerateFiles(SAVE_PATH, "*", SearchOption.AllDirectories) select file;
        var resourceFiles = from file in Directory.EnumerateFiles(RESOURCES_MAP_PATH, "*.png", SearchOption.AllDirectories) select file;

        List<string> missDirList = new List<string>();
        int i = 0;
        foreach (var file in resourceFiles)
        {
            if (externalFiles.Any(f => f.Contains(file.ToString()) == false))
            {
#if UNITY_EDITOR
                Debug.LogFormat("[FilePath] {0}- Missing File Name: {1}", i, file.ToString());
#endif
                missDirList.Add(file.ToString());
                checker = false;
            }
            ++i;
        }

        _missList = missDirList;


        return checker;
    }


    /// <summary>
    /// 해당하는 경로를 반환합니다. 해당하는 값이 없다면 string.Empty를 반환합니다.
    /// </summary>
    /// <param name="_pathType"></param>
    /// <returns>해당 값이 없다면 string.Empty 반환</returns>
    public static string GetPath(EPathType _pathType)
    {
        switch (_pathType)
        {
            case EPathType.EXTERNAL:
                return EXTERNAL_PATH;
            case EPathType.RESOURCES:
                return RESOURCES_PATH;
        }

        return string.Empty;
    }
    /// <summary>
    /// 해당하는 경로를 반환합니다. 해당하는 값이 없다면 string.Empty를 반환합니다.
    /// </summary>
    /// <param name="_pathType"></param>
    /// <param name="_mapType"></param>
    /// <returns>해당 값이 없다면 string.Empty 반환</returns>
    public static string GetPath(EPathType _pathType, EMapType _mapType)
    {
        if (_pathType == EPathType.EXTERNAL)
        {
            switch (_mapType)
            {
                case EMapType.MAP_1:
                    return MAP_1_PATH;
                case EMapType.MAP_2:
                    return MAP_2_PATH;
            }
        }
        else if (_pathType == EPathType.RESOURCES)
        {
            switch (_mapType)
            {
                case EMapType.MAP_1:
                    return RESOURCES_MAP_1_PATH;
                case EMapType.MAP_2:
                    return RESOURCES_MAP_2_PATH;
            }
        }

        return string.Empty;
    }
    /// <summary>
    /// 해당하는 경로를 반환합니다. 해당하는 값이 없다면 string.Empty를 반환합니다.
    /// </summary>
    /// <param name="_pathType"></param>
    /// <param name="_mapType"></param>
    /// <param name="_sectionType"></param>
    /// <returns>해당 값이 없다면 string.Empty 반환</returns>
    public static string GetPath(EPathType _pathType, EMapType _mapType, ESection _sectionType)
    {
        if (_pathType == EPathType.EXTERNAL)
        {
            if (_mapType == EMapType.MAP_1)
            {
                switch (_sectionType)
                {
                    case ESection.SECTION_1:
                        return MAP_1_SECTION_1_PATH;
                    case ESection.SECTION_2:
                        return MAP_1_SECTION_2_PATH;
                    case ESection.SECTION_3:
                        return MAP_1_SECTION_3_PATH;
                    case ESection.SECTION_4:
                        return MAP_1_SECTION_4_PATH;
                    case ESection.SECTION_5:
                        return MAP_1_SECTION_5_PATH;
                }
            }
            else if (_mapType == EMapType.MAP_2)
            {
                switch (_sectionType)
                {
                    case ESection.SECTION_1:
                        return MAP_2_SECTION_1_PATH;
                    case ESection.SECTION_2:
                        return MAP_2_SECTION_2_PATH;
                    case ESection.SECTION_3:
                        return MAP_2_SECTION_3_PATH;
                }
            }
        }
        else if (_pathType == EPathType.RESOURCES)
        {
            if (_mapType == EMapType.MAP_1)
            {
                switch (_sectionType)
                {
                    case ESection.SECTION_1:
                        return RESOURCES_MAP_1_SECTION_1_PATH;
                    case ESection.SECTION_2:
                        return RESOURCES_MAP_1_SECTION_2_PATH;
                    case ESection.SECTION_3:
                        return RESOURCES_MAP_1_SECTION_3_PATH;
                    case ESection.SECTION_4:
                        return RESOURCES_MAP_1_SECTION_4_PATH;
                    case ESection.SECTION_5:
                        return RESOURCES_MAP_1_SECTION_5_PATH;
                }
            }
            else if (_mapType == EMapType.MAP_2)
            {
                switch (_sectionType)
                {
                    case ESection.SECTION_1:
                        return RESOURCES_MAP_2_SECTION_1_PATH;
                    case ESection.SECTION_2:
                        return RESOURCES_MAP_2_SECTION_2_PATH;
                    case ESection.SECTION_3:
                        return RESOURCES_MAP_2_SECTION_3_PATH;
                }
            }
        }

        return string.Empty;
    }

    private static void CreateDirectory(string _path)
    {
        if (Directory.Exists(_path) == false)
            Directory.CreateDirectory(_path);
    }
}
