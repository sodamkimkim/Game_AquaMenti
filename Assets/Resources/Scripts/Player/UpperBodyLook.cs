using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperBodyLook : MonoBehaviour
{
    private float offsetAngle_ = 10f;
    private Camera mainCam_;
    private void Awake()
    {
        mainCam_ = Camera.main;
        mainCam_.transform.SetParent(this.transform);
        Vector3 newCamPos = mainCam_.transform.localPosition;
        newCamPos.y = 0.66f;
        newCamPos.z = 0.18f;
        mainCam_.transform.localPosition = newCamPos;
    }
    private void Update()
    {
        RotateUpperBodyAxisX();
    }

    /// <summary>
    /// 상체 각도 위아래 보려고 변경
    /// </summary>
    private void RotateUpperBodyAxisX()
    {
        Debug.Log(transform.localRotation);
        // 아래로 바라보는 방향
        if (transform.localRotation.x < -0.31 )
        {
            Quaternion q = transform.localRotation;
            q.x = -0.31f;
            transform.localRotation = q;
        }
        // 위로 바라보는 방향
        if (transform.localRotation.x>0.22)
        {
            Quaternion q = transform.localRotation;
            q.x = 0.22f;
            transform.localRotation = q;
        }

        float mouseY = Input.GetAxis("Mouse Y");
        transform.Rotate(-mouseY * offsetAngle_, 0f, 0f);
    }

} // end of class
