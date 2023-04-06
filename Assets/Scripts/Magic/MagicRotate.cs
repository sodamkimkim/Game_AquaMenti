using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicRotate : MonoBehaviour
{
    public Transform GetTransform()
    { return transform; }
    public void RotateWaterMagic()
    {
        this.GetTransform().Rotate(new Vector3(0f, 0f, 90f));
    }

} // end of class
