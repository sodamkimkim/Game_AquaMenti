using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicManager : MonoBehaviour
{
    [SerializeField]
    private MagicRotate magicRotate_;

    public void RotateWaterMagic()
    {
        magicRotate_.RotateWaterMagic();
    }
} // end of class
