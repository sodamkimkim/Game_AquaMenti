using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class UpperBodyLook : MonoBehaviour
{
    [SerializeField]
    private GameObject rayGroup = null;
    private readonly float focusFixedModeOffsetAngle_ = 2f;
    private readonly float focusMoveModeOffsetAngle_ = 1f;
    private Camera mainCam_;

    private void Awake()
    {
        SetCameraPos();
    }
    private void Update()
    {
        //Debug.Log(transform.localRotation);
        ConstrainRotationAngle();
    }
    public void SetCameraPos()
    {
        mainCam_ = Camera.main;
        mainCam_.transform.SetParent(this.transform);

        Vector3 newCamPos = mainCam_.transform.localPosition;
        newCamPos.x = -0.085f;
        newCamPos.y = 0.413f;
        newCamPos.z = 0.1f;
        mainCam_.transform.localPosition = newCamPos;

        rayGroup.transform.SetParent(null);
        rayGroup.transform.SetParent(mainCam_.transform);
        rayGroup.transform.localPosition = new Vector3(0f, 0f, 0f);
    }
    public Vector3 GetEuler()
    {
        return transform.localRotation.eulerAngles;
    }

    public void ConstrainRotationAngle()
    {
        if (transform.localRotation.x > 0.22)
        {
            Quaternion q = transform.localRotation;
            q.x = 0.22f;
            transform.localRotation = q;
        }

        //Debug.Log("RotateUpperBodyAxisXDown()");
        if (transform.localRotation.x < -0.31)
        {
            Quaternion q = transform.localRotation;
            q.x = -0.31f;
            transform.localRotation = q;
        }
        //if (transform.rotation.x > 22 && transform.rotation.x < 35)
        //{
        //    Vector3 V3 = new Vector3(-Input.GetAxis("Mouse X"), 0, 0);
        //    transform.Rotate(V3 * 2f);
        //}

    }

    /// <summary>
    /// FocusFixed 모드 - 상체 각도 위아래 보려고 x축 rotate
    /// </summary>
    public void RotateUpperBodyAxisX(bool _para)
    {
        if (_para)
        {
            //Debug.Log(transform.localRotation);
            //# 위로 보는 회전각도 제약
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
            //float rotAngle = 0f;
            //rotAngle = focusFixedModeOffsetAngle_;
            //      transform.Rotate(Vector3.right, Mathf.Clamp(rotAngle* -mouseY, -0.3f, 0.22f));

        }
    }

    /// <summary>
    /// FocusMove 모드에서 상체 UP방향 rotation
    /// </summary>
    /// <param name="_para"></param>
    public void RotateUpperBodyUP(bool _para)
    {
        //Debug.Log("RotateUpperBodyAxisXUP()");
        //if (transform.localRotation.x > 0.22)
        //{
        //    Quaternion q = transform.localRotation;
        //    q.x = 0.22f;
        //    transform.localRotation = q;
        //}
        if (_para)
        {

            float rotAngle = 0f;
            rotAngle = -focusMoveModeOffsetAngle_;
            transform.Rotate(Vector3.right, Mathf.Clamp(rotAngle, -0.3f, 0.22f));
            //   transform.Rotate(Vector3.right, Mathf.Clamp(transform.localRotation.x, -0.3f, 0.22f));

        }
    }
    /// <summary>
    /// FocusMove 모드에서 상체 Down방향 rotation
    /// </summary>
    /// <param name="_para"></param>
    public void RotateUpperBodyDown(bool _para)
    {
        //Debug.Log("RotateUpperBodyAxisXDown()");
        //if (transform.localRotation.x < -0.31)
        //{
        //    Quaternion q = transform.localRotation;
        //    q.x = -0.31f;
        //    transform.localRotation = q;
        //}
        if (_para)
        {

            float rotAngle = 0f;
            rotAngle = focusMoveModeOffsetAngle_;
            //   transform.Rotate(Vector3.right, rotAngle);
            transform.Rotate(Vector3.right, Mathf.Clamp(rotAngle, -0.3f, 0.22f));

        }
    }
} // end of class
