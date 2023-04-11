using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperBodyLook : MonoBehaviour
{
    private readonly float focusFixedModeOffsetAngle_ = 10f;
    private readonly float focusMoveModeOffsetAngle_ = 2f;
    private Camera mainCam_;
    private void Awake()
    {
        SetCameraPos();
    }
    private void Update()
    {
       //Debug.Log(transform.localRotation);
    }
    public void SetCameraPos()
    {
        mainCam_ = Camera.main;
        mainCam_.transform.SetParent(this.transform);
        Vector3 newCamPos = mainCam_.transform.localPosition;

        //   newCamPos.x = -0.78f;
        newCamPos.y = 0.28f;
        newCamPos.z = 0.38f;
        mainCam_.transform.localPosition = newCamPos;
    }
    public Vector3 GetEuler()
    {
        return transform.localRotation.eulerAngles;
    }
    /// <summary>
    /// FocusFixed 모드 - 상체 각도 위아래 보려고 x축 rotate
    /// </summary>s
    public void RotateUpperBodyAxisX(bool _para)
    {
        if (_para)
        {
            // Debug.Log(transform.localRotation);
            // # 위로 보는 회전각도 제약
            if (transform.localRotation.x >= 0.22)
            {
                Quaternion q = transform.localRotation;
                q.x = 0.22f;
                transform.localRotation = q;
            }
            // # 아래로 보는 회전각도 제약
            if (transform.localRotation.x <= -0.31)
            {
                Quaternion q = transform.localRotation;
                q.x = -0.31f;
                transform.localRotation = q;
            }
            float mouseY = Input.GetAxis("Mouse Y");
            transform.Rotate(-mouseY * focusFixedModeOffsetAngle_, 0f, 0f);
        }
    }

    /// <summary>
    /// FocusMove 모드에서 상체 UP방향 rotation
    /// </summary>
    /// <param name="_para"></param>
    public void RotateUpperBodyUP(bool _para)
    {
            Debug.Log("RotateUpperBodyAxisXUP()");
            if (transform.localRotation.x >= 0.22)
            {
                Quaternion q = transform.localRotation;
                q.x = 0.22f;
                transform.localRotation = q;
            }
        if (_para)
        {

            float rotAngle = 0f;
            rotAngle = -focusMoveModeOffsetAngle_;
            transform.Rotate(Vector3.right, rotAngle);

        }
    }
    /// <summary>
    /// FocusMove 모드에서 상체 Down방향 rotation
    /// </summary>
    /// <param name="_para"></param>
    public void RotateUpperBodyDown(bool _para)
    {
            Debug.Log("RotateUpperBodyAxisXDown()");
            if (transform.localRotation.x <= -0.31)
            {
                Quaternion q = transform.localRotation;
                q.x = -0.31f;
                transform.localRotation = q;
            }
        if (_para)
        {

            float rotAngle = 0f;
            rotAngle = focusMoveModeOffsetAngle_;
            transform.Rotate(Vector3.right, rotAngle);

        }
    }
} // end of class
