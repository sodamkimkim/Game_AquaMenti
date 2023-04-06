using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVTest : MonoBehaviour
{
    private Transform Map;
    private List<Transform> WorkStationList;
    private Transform[] partArr;
    private void Awake()
    {
        Save();
    }
    public void Save()
    {
        Map = GameObject.FindWithTag("Map").GetComponent<Transform>();
        //WorkStationList = Map.GetComponentsInChildren<Transform>();
        //for (int i = 0; i < WorkStationList.Length; i++)
        //{

        //}
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"Assets/Resources/Datas/GameInfo/test.csv"))
        { // 소속 워크스테이션 이름, 파트이름
            file.WriteLine("workstationBelong, partName");

            file.WriteLine("{0}, {1}", "A", "합격");
        }
    }

} // end of class
