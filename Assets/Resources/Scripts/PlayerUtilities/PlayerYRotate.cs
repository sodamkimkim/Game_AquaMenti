using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerYRotate : MonoBehaviour
{
    private readonly float focusFixedModeOffsetAngle_ = 10f;
    private readonly float focusMoveModeOffsetAngle_ = 2f;

    /// <summary>
    /// FocusFixed 모드에서 몸전체 rotation
    /// </summary>
    public void RotateBodyAxisY(bool _para)
    {
        if (_para)
        {
            float mouseX = Input.GetAxis("Mouse X");
            transform.Rotate(0f, mouseX * focusFixedModeOffsetAngle_, 0f, Space.World);
        }
    }
    /// <summary>
    /// FocusMove 모드에서 몸 전체 Left방향 rotation
    /// </summary>
    /// <param name="_para"></param>
    public void RotateBodyAxisYLeft(bool _para)
    {
        if (_para)
        {
            Debug.Log("RotateBodyAxisYLeft()");
            float rotAngle = 0f;
            rotAngle += -focusMoveModeOffsetAngle_;
            transform.Rotate(0f, rotAngle, 0f, Space.World);
        }
    }
    /// <summary>
    /// FocusMove 모드에서 몸 전체 Right방향 rotation
    /// </summary>
    /// <param name="_para"></param>
    public void RotateBodyAxisYRight(bool _para)
    {
        if (_para)
        {
            Debug.Log("RotateBodyAxisYRight()");
            float rotAngle = 0f;
            rotAngle += focusMoveModeOffsetAngle_;
            transform.Rotate(0f, rotAngle, 0f, Space.World);
        }
    }
} // end of class
