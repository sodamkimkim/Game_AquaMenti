using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFocusPoint : MonoBehaviour
{
    public Transform GetTransform()
    {
        return transform;
    }
    public Vector3 GetPos()
    {
        return GetTransform().position;
    }
    public void SetPos(Vector3 _pos)
    {
        GetTransform().position = _pos;
    }

} // end of class
