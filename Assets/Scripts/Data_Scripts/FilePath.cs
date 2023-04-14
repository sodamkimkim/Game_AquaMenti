using System.IO;
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
    public static readonly string RESOURCES_MAP_1_SECTION_1_PATH = Application.dataPath + "/Resources/Maps/Map_1/Section_1";
    public static readonly string RESOURCES_MAP_1_SECTION_2_PATH = Application.dataPath + "/Resources/Maps/Map_1/Section_2";
    public static readonly string RESOURCES_MAP_1_SECTION_3_PATH = Application.dataPath + "/Resources/Maps/Map_1/Section_3";
    public static readonly string RESOURCES_MAP_1_SECTION_4_PATH = Application.dataPath + "/Resources/Maps/Map_1/Section_4";
    public static readonly string RESOURCES_MAP_1_SECTION_5_PATH = Application.dataPath + "/Resources/Maps/Map_1/Section_5";

    public static readonly string RESOURCES_MAP_2_PATH = Application.dataPath + "/Resources/Maps/Map_2";
    public static readonly string RESOURCES_MAP_2_SECTION_1_PATH = Application.dataPath + "/Resources/Maps/Map_2/Section_1";
    public static readonly string RESOURCES_MAP_2_SECTION_2_PATH = Application.dataPath + "/Resources/Maps/Map_2/Section_2";
    public static readonly string RESOURCES_MAP_2_SECTION_3_PATH = Application.dataPath + "/Resources/Maps/Map_2/Section_3";
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
