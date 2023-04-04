using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerYRotate : MonoBehaviour
{
    private float offsetAngle_ = 10f;

    private void Update()
    {

    }
    /// <summary>
    /// ¸öÀüÃ¼ rotation
    /// </summary>
    public void RotateBodyAxisY()
    {
        float mouseX = Input.GetAxis("Mouse X");
        transform.Rotate(0f, mouseX * offsetAngle_, 0f, Space.World);
    }
} // end of class
