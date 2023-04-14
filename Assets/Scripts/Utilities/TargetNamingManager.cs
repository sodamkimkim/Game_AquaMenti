using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TargetNamingManager : MonoBehaviour
{
    [SerializeField]
    private GameObject map1_ = null;
    [SerializeField]
    private GameObject[] map1WorkSectionsArr_ = null;

    [SerializeField]
    private GameObject map2_ = null;
    [SerializeField]
    private GameObject[] map2WorkSectionsArr_ = null;

    private MeshPaintTarget[] meshPaintTarget_m1__w1_ = null;
    private MeshPaintTarget[] meshPaintTarget_m1__w2_ = null;

    private Dictionary<string, int[]> sameNameDic_ = new Dictionary<string, int[]>();
    private void Awake()
    {
        meshPaintTarget_m1__w1_ = map1WorkSectionsArr_[0].gameObject.GetComponentsInChildren<MeshPaintTarget>();
        meshPaintTarget_m1__w2_ = map1WorkSectionsArr_[1].gameObject.GetComponentsInChildren<MeshPaintTarget>();

        ChangeTargetName1(meshPaintTarget_m1__w1_);
        CountTargets(meshPaintTarget_m1__w1_);
        ChangeTargetName2(meshPaintTarget_m1__w1_);
    }
    private void ChangeTargetName1(MeshPaintTarget[] _meshPaintTargetArr)
    {
        foreach (MeshPaintTarget mpt in _meshPaintTargetArr)
        {
            // Debug.Log(mpt.gameObject.name);
            string prevName = mpt.gameObject.name;
            mpt.gameObject.name = prevName + "_1_1";

        }
    }
    private void CountTargets(MeshPaintTarget[] _meshPaintTargetArr)
    {
        foreach (MeshPaintTarget mpt in _meshPaintTargetArr)
        {
            string targetName = mpt.gameObject.name;
            int nameCount = CheckDoubleName(targetName, _meshPaintTargetArr);
            if (nameCount > 1)
            {
                if (!sameNameDic_.ContainsKey(targetName))
                {
                    int[] intArr = new int[2];
                    intArr[0] = nameCount;
                    intArr[1] = 1;
                    sameNameDic_.Add(targetName, intArr);
                }
            }
        }
        foreach (var item in sameNameDic_)
        {
            //  Debug.Log(item);

        }
    }
    private void ChangeTargetName2(MeshPaintTarget[] _meshPaintTargetArr)
    {
        foreach (var item in _meshPaintTargetArr)
        {
            string targetName = item.name;
            if (sameNameDic_.ContainsKey(targetName) && sameNameDic_[targetName][0] > 0)
            {

                item.name = targetName + "_" + sameNameDic_[targetName][1];
                sameNameDic_[targetName][1]++;

            }
        }
    }

    private int CheckDoubleName(string _name, MeshPaintTarget[] _worksectionTargetsArr)
    {
        int nameCount = 0;
        foreach (var item in _worksectionTargetsArr)
        {
            if (_name == item.gameObject.name)
            {
                nameCount += 1;
            }
        }
        return nameCount;
    }
} // end of class
