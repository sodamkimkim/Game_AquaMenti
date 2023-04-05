using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicManager : MonoBehaviour
{
    private MagicRotate magicRotate_;

    private void Awake()
    {
        magicRotate_ = GameObject.FindWithTag("WaterMagic").GetComponent<MagicRotate>();
    }
    public void RotateWaterMagic()
    {
        magicRotate_.RotateWaterMagic();
    }
} // end of class
