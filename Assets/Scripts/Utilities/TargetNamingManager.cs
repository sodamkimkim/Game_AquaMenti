using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetNamingManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] map1WorkSectionsArr_ = null;
    [SerializeField]
    private GameObject[] map2WorkSectionsArr_ = null;

    private MeshPaintTarget[] meshPaintTarget_m1_w1_ = null;
    private MeshPaintTarget[] meshPaintTarget_m1_w2_ = null;
    private MeshPaintTarget[] meshPaintTarget_m1_w3_ = null;
    private MeshPaintTarget[] meshPaintTarget_m1_w4_ = null;
    private MeshPaintTarget[] meshPaintTarget_m1_w5_ = null;
    private MeshPaintTarget[] meshPaintTarget_m2_w1_ = null;
    private MeshPaintTarget[] meshPaintTarget_m2_w2_ = null;
    private MeshPaintTarget[] meshPaintTarget_m2_w3_ = null;

    private Dictionary<string, int[]> sameNameDic_ = new Dictionary<string, int[]>();
    private void Awake()
    {
        Init();
        // # Map1
        DoProcess(meshPaintTarget_m1_w1_, 1,1);
        DoProcess(meshPaintTarget_m1_w2_,1,2);
        DoProcess(meshPaintTarget_m1_w3_,1,3);
        DoProcess(meshPaintTarget_m1_w4_, 1, 4);
        DoProcess(meshPaintTarget_m1_w5_, 1, 5);

        // # Map2
        DoProcess(meshPaintTarget_m2_w1_, 2, 1);
        DoProcess(meshPaintTarget_m2_w2_, 2, 2);
        DoProcess(meshPaintTarget_m2_w3_, 2, 3);
    }
    private void Init()
    {
        meshPaintTarget_m1_w1_ = map1WorkSectionsArr_[0].gameObject.GetComponentsInChildren<MeshPaintTarget>();
        meshPaintTarget_m1_w2_ = map1WorkSectionsArr_[1].gameObject.GetComponentsInChildren<MeshPaintTarget>();
        meshPaintTarget_m1_w3_ = map1WorkSectionsArr_[2].gameObject.GetComponentsInChildren<MeshPaintTarget>();
        meshPaintTarget_m1_w4_ = map1WorkSectionsArr_[3].gameObject.GetComponentsInChildren<MeshPaintTarget>();
        meshPaintTarget_m1_w5_ = map1WorkSectionsArr_[4].gameObject.GetComponentsInChildren<MeshPaintTarget>();

        meshPaintTarget_m2_w1_ = map2WorkSectionsArr_[0].gameObject.GetComponentsInChildren<MeshPaintTarget>();
        meshPaintTarget_m2_w2_ = map2WorkSectionsArr_[1].gameObject.GetComponentsInChildren<MeshPaintTarget>();
        meshPaintTarget_m2_w3_ = map2WorkSectionsArr_[2].gameObject.GetComponentsInChildren<MeshPaintTarget>();
    }
    private void DoProcess(MeshPaintTarget[] _targetArr, int _mapIdx, int _wsIdx)
    {
        ChangeTargetName1(_targetArr, _mapIdx, _wsIdx);
        CountTargets(_targetArr);
        ChangeTargetName2(_targetArr);
    }
    private void ChangeTargetName1(MeshPaintTarget[] _meshPaintTargetArr, int _mapIdx, int _wsIdx)
    {

        foreach (MeshPaintTarget mpt in _meshPaintTargetArr)
        {
            // Debug.Log(mpt.gameObject.name);
            string prevName = mpt.gameObject.name;
            mpt.gameObject.name = prevName + "_"+_mapIdx + "_"+_wsIdx;

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
            else
            {
                item.name = targetName + "_" + 1;
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
