using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;

public class FileIO
{
    /// <summary>
    /// External 경로와 Resources 경로를 확인하여 손실된 것이 없다면 true를 반환하고 손실된 것이 있다면 false를 반환합니다.
    /// </summary>
    /// <param name="_missList">손실된 Directory의 Name을 담습니다.</param>
    /// <returns>true: 모든 파일이 존재 | false: 손실된 파일이 존재</returns>
    public static bool CheckFileState(out List<string> _missList, string _fileType = "*")
    {
        bool checker = true;

        // System.Linq 사용
        DirectoryInfo externalDir = new DirectoryInfo(FilePath.SAVE_PATH);
        var externalFiles = from file in externalDir.GetFiles(_fileType, SearchOption.AllDirectories) select file;
        DirectoryInfo resourceDir = new DirectoryInfo(FilePath.ASSETS_MAP_PATH);
        var resourceFiles = from file in resourceDir.GetFiles(_fileType, SearchOption.AllDirectories) select file;

        List<string> missDirList = new List<string>();
        int i = 0;
        foreach (FileInfo file in resourceFiles)
        {
            if (externalFiles.Any(f => f.Name == file.Name) == false)
            {
                // 경로 구분 문자를 '\'에서 '/'으로 통일시킵니다.
                string missFilePath = Regex.Replace(file.FullName, @"[\\]", "/").Replace(FilePath.ASSETS_MAP_PATH, "");
#if UNITY_EDITOR
                //Debug.LogFormat("[FilePath] {0}- Missing File Name: {1}, path: {2}", i, file.Name, missFilePath);
#endif
                missDirList.Add(missFilePath);
                checker = false;
            }
            ++i;
        }

        _missList = missDirList;


        return checker;
    }

    /// <summary>
    /// <paramref name="_dir"/>에 있는 <paramref name="_fileName"/> 파일을 Binary로 가져옵니다.
    /// </summary>
    /// <param name="_dir">가져올 Directory</param>
    /// <param name="_fileName">가져올 파일의 Name (확장자명 포함)</param>
    /// <returns></returns>
    public static byte[] GetFileBinary(string _dir, string _fileName)
    {
        if (Directory.Exists(_dir) == false) return null;

        byte[] bytes = null;
        string path = Path.Combine(_dir, _fileName);

        if(File.Exists(path))
            bytes = File.ReadAllBytes(path);

        return bytes;
    }

    /// <summary>
    /// <paramref name="_sourceDir"/>에 있는 <paramref name="_fileName"/> 파일을 <paramref name="_destinationDir"/>에 복사합니다.<br/>
    /// <paramref name="_overwrite"/>를 통해 파일을 덮어씌울 것인지 정할 수 있습니다.
    /// </summary>
    /// <param name="_sourceDir"></param>
    /// <param name="_destinationDir"></param>
    /// <param name="_fileName"></param>
    /// <param name="_overwrite"></param>
    public static void CopyFile(string _sourceDir, string _destinationDir, string _fileName, bool _overwrite = false)
    {
        if (Directory.Exists(_sourceDir) == false) return;

        if (Directory.Exists(_destinationDir) == false)
            Directory.CreateDirectory(_destinationDir);

        var dir = new DirectoryInfo(_sourceDir);
        
        foreach (FileInfo file in dir.GetFiles(_fileName))
        {
            if (file.Exists && file.Extension != ".meta")
            {
                string path = Path.Combine(_destinationDir, file.Name);
                file.CopyTo(path, _overwrite);
            }
        }
    }

    /// <summary>
    /// <paramref name="_sourceDir"/>에 있는 <paramref name="_fileType"/>형식의 파일들을 <paramref name="_destinationDir"/>에 복사합니다.<br/>
    /// <paramref name="_recursive"/>을 통해 하위 Directory까지 복사할 것인지 정할 수 있습니다.<br/>
    /// <paramref name="_overwrite"/>를 통해 파일을 덮어씌울 것인지 정할 수 있습니다.<br/>
    /// </summary>
    /// <param name="_sourceDir">복사할 Directory</param>
    /// <param name="_destinationDir">대상인 Directory</param>
    /// <param name="_fileType">복사하고자 하는 파일형식 | default: "*"</param>
    /// <param name="_recursive">하위 Directory까지 복사할 것인지 여부 | default: false</param>
    /// <param name="_overwrite">덮어쓰기를 할 것인지 여부 | default: false</param>
    public static void CopyDirectory(string _sourceDir, string _destinationDir, string _fileType = "*", bool _recursive = false, bool _overwrite = false)
    {
        if (Directory.Exists(_sourceDir) == false) return;

        if (Directory.Exists(_destinationDir) == false)
            Directory.CreateDirectory(_destinationDir);

        //Debug.LogFormat("sour: {0}, dest: {1}, type: {2}", _sourceDir, _destinationDir, _fileType);

        DirectoryInfo dir = new DirectoryInfo(_sourceDir);

        foreach (FileInfo file in dir.GetFiles(_fileType))
        {
            string path = Path.Combine(_destinationDir, file.Name);
            file.CopyTo(path, _overwrite);
        }

        if (_recursive)
        {
            DirectoryInfo[] dirs = dir.GetDirectories();
            foreach (DirectoryInfo subDir in dirs)
            {
                string newDestinationDir = Path.Combine(_destinationDir, subDir.Name);
                //Debug.Log("dir: " + newDestinationDir);
                CopyDirectory(_destinationDir, newDestinationDir, _fileType, true);
            }
        }
    }
}
