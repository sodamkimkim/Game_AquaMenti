using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    // Test를 위한 기본적인 이동, 회전만 구현
    private float moveSpeed = 3f;
    private float rotSpeed = 180f;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rot = Vector3.zero;

    private bool isCaptured = false;


    private void Awake()
    {
        if (velocity == Vector3.zero)
            velocity = this.transform.position;
        if (rot == Vector3.zero)
            rot = this.transform.rotation.eulerAngles;
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        MoveProcess();
        RotProcess();
        CapturedMouse();
    }


    private void Init()
    {
        isCaptured = true;
        if (isCaptured)
            // 마우스를 윈도우 정중앙 고정 후 보이지 않게 함
            Cursor.lockState = CursorLockMode.Locked;
        else
        {
            // 마우스를 창 밖으로 벗어나지 못하게 함
            Cursor.lockState = CursorLockMode.Confined;
            VisibleMouse(false);
        }
    }


    private void MoveProcess()
    {
        float axisH = Input.GetAxis("Horizontal");
        float axisV = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(axisH, 0f, axisV);
        //transform.TransformDirection(rot.normalized);
        //velocity += move * moveSpeed * Time.deltaTime;

        //
        float angle = Mathf.Atan2(axisV * rotSpeed * Time.deltaTime, axisH * rotSpeed * Time.deltaTime);
        angle *= Mathf.Rad2Deg; // Radian을 Degree로 변환
        angle += 270f; // 정면이 0이 되도록 보정
        angle *= -1f; // 좌우 반전 보정
        Vector3 angleVector = new Vector3(0f, angle, 0f);

        // 이동 방향 보정
        move.x *= -1f; // 이동 좌우 반전 보정
        move = Quaternion.Euler(angleVector) * move;

        // 카메라 방향 인식
        angleVector += rot;
        transform.rotation = Quaternion.Euler(angleVector);

        // 캐릭터가 바라보는 방향이 정면이 되도록 보정
        move = transform.TransformDirection(move.normalized);
        //

        velocity += move * moveSpeed * Time.deltaTime;

        transform.position = velocity;
    }

    private void RotProcess()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rot.x += -mouseY * rotSpeed * Time.deltaTime;

        // 축 반전 방지
        if (rot.x > 80f)
            rot.x = 80f;
        else if (rot.x < -80f)
            rot.x = -80f;

        rot.y += mouseX * rotSpeed * Time.deltaTime;

        transform.rotation = Quaternion.Euler(rot);
    }


    #region Mouse Capture in Screen
    private void CapturedMouse()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            isCaptured = !isCaptured;
            if(isCaptured)
                // 마우스를 윈도우 정중앙 고정 후 보이지 않게 함
                Cursor.lockState = CursorLockMode.Locked;
            else
            {
                // 마우스를 창 밖으로 벗어나지 못하게 함
                Cursor.lockState = CursorLockMode.Confined;
                VisibleMouse(false);
            }
        }
    }

    // PowerWash Simulator 기준
    // true: 메인화면, 로딩후, 인게임-태블릿모드, 인게임-인벤토리모드, 인게임-장비변경모드
    private void VisibleMouse(bool _visible)
    {
        Cursor.visible = _visible;
    }
    #endregion
}
