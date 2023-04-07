using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class CSVReader
{
    static readonly string SPLIT_RE_ = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static readonly string LINE_SPLIT_RE_ = @"\r\n|\n\r|\n";
    static readonly char[] TRIM_CHARS_ = { '\"' };

    public static void Read(string file, out List<Dictionary<string, object>> _list)
    {
        var list = new List<Dictionary<string, object>>();
        TextAsset data = Resources.Load(file) as TextAsset;
        _list = list;

        var lines = Regex.Split(data.text, LINE_SPLIT_RE_);

        if (lines.Length > 1) { 

            var header = Regex.Split(lines[0], SPLIT_RE_);
            for (var i = 1; i < lines.Length; i++)
            {

                var values = Regex.Split(lines[i], SPLIT_RE_);
                if (values.Length == 0 || values[0] == "") continue;

                var entry = new Dictionary<string, object>();
                for (var j = 0; j < header.Length && j < values.Length; j++)
                {
                    string value = values[j];
                    value = value.TrimStart(TRIM_CHARS_).TrimEnd(TRIM_CHARS_).Replace("\\", "");
                    value = value.Replace("#n", "\n");
                    object finalvalue = value;
                    int n;
                    float f;
                    if (int.TryParse(value, out n))
                    {
                        finalvalue = n;
                    }
                    else if (float.TryParse(value, out f))
                    {
                        finalvalue = f;
                    }
                    entry[header[j]] = finalvalue;
                }
                list.Add(entry);
                _list = list;
            }
        }

    }
}
