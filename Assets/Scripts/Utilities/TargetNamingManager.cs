using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetNamingManager : MonoBehaviour
{
    [SerializeField]
    private GameObject map1_ = null;
    [SerializeField]
    private GameObject[] map1WorkSectionsArr_ = null;
    //[SerializeField]
    //private GameObject map2_ = null;
    ////[SerializeField]
    ////private GameObject[] map2WorkSectionsArr_ = null;

    private MeshPaintTarget[] meshPaintTarget_m1__w1 = null;
    private MeshPaintTarget[] meshPaintTarget_m1__w2 = null;
    private void Awake()
    {
        meshPaintTarget_m1__w1 = map1WorkSectionsArr_[0].gameObject.GetComponentsInChildren<MeshPaintTarget>(); 
        meshPaintTarget_m1__w2 = map1WorkSectionsArr_[1].gameObject.GetComponentsInChildren<MeshPaintTarget>();

        ChangeTargetName_m1__w1();
    }
    private void ChangeTargetName_m1__w1()
    {
        foreach(MeshPaintTarget mpt in meshPaintTarget_m1__w1)
        {
            Debug.Log(mpt.gameObject.name);
            //mpt.gameObject.name = "";
        }
    }
} // end of class
