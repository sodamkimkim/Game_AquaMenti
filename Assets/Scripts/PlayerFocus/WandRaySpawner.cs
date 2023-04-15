using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class WandRaySpawner : MonoBehaviour
{
    // # Composition
    [SerializeField]
    private UIFocusPoint uIFocusPoint_;
    [SerializeField]
    private MeshPaintBrush[] sideRayBrushArr = new MeshPaintBrush[5];

    // # side rays endPosition구할 필드
    [SerializeField]
    private float rayPosDefaultOffset_ = 0.05f; // 5개 각 ray의 offset, center를 기준으로 해당 값만큼 떨어져서 생성 (Rotate전 기본 축 : X)
    [SerializeField]
    private float mainRayMaxDistance_ = 10f;
    [SerializeField]
    private float sideRayMaxDistance_ = 0f; // centerRayMaxDistance 의 0.5만큼으로 초기화

    private Ray centerRay_;

    private Vector3 screenCenter_;
    public Vector3 hitPos_ { get; set; }
    public bool isLadder_ { get; set; }

    public string cleaningTargetName_ { get; private set; }
    public float rayAngle_ { get; set; } // 물 분사 각도


    private void Awake()
    {
        sideRayMaxDistance_ = mainRayMaxDistance_ * 0.5f;
        screenCenter_ = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2, 0);
        isLadder_ = false;
        cleaningTargetName_ = "";
        rayAngle_ = 0f;
    }
    private void Update()
    {
        Debug.DrawRay(GetPos(), centerRay_.direction * mainRayMaxDistance_, Color.red);
     
    }
    private void SetTargetName_(string _targetName)
    {
        cleaningTargetName_ = _targetName;
    }
    public void RotateFocusPointUI()
    {
        uIFocusPoint_.transform.Rotate(new Vector3(0f, 0f, 90f));
    }
    public Transform GetTransform()
    {
        return transform;
    }
    public Vector3 GetPos()
    {
        return transform.position;
    }
    public Vector3 GetLocalPos()
    {
        return transform.localPosition;
    }
    /// <summary>
    /// WandRaySpawner로부터 SreenCenter로 Ray쏴주는 메서드
    /// Ray 맞은 IInteractableObject 물체 인식 함.
    /// </summary>
    public void RayScreenCenterShot()
    {
        centerRay_ = Camera.main.ScreenPointToRay(screenCenter_);
        uIFocusPoint_.SetPos(screenCenter_);
        RayFindObject();
    }
    /// <summary>
    /// WandRaySpawner로부터 MousePosition으로 Ray쏴주는 메서드
    /// </summary>
    public void RayMoveFocusShot()
    {
        Vector3 mousePos = Input.mousePosition;
        centerRay_ = Camera.main.ScreenPointToRay(mousePos);
        uIFocusPoint_.SetPos(mousePos);
        RayFindObject();


    }
    private void RayFindObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(centerRay_, out hit, mainRayMaxDistance_))
        {
            // # 사다리 인식
            IInteractableObject target = hit.collider.GetComponentInParent<IInteractableObject>();
            if (target != null)
            {
                //Debug.Log(target.GetName());

                // target이름이 Ladder이면 bool 값을 바꿔서 Ladder의 위치를 옮길 수 있음
                if (target.GetName() == IInteractableTool.EInteractableTool.Ladder.ToString())
                {
                    isLadder_ = true;
                }
                else
                {
                    isLadder_ = false;
                }
            }
            else
            {
                isLadder_ = false;
    
            }
            hitPos_ = hit.point;

            // # meshpaint target 인식
            MeshPaintTarget meshPaintTarget = hit.collider.gameObject.GetComponent<MeshPaintTarget>();
            if (meshPaintTarget != null)
            {
                cleaningTargetName_ = meshPaintTarget.gameObject.name;
                Debug.Log("meshPaintTarget Name: " + cleaningTargetName_);
            }
            else
            {
                cleaningTargetName_ = "";
                Debug.Log("meshPaintTarget Name: " + cleaningTargetName_);
            }
        }
        else
        {
            hitPos_ = GetPos() + GetTransform().forward * mainRayMaxDistance_;
            isLadder_ = false;
        }



    }
    public Vector3 GetCenterRayDir()
    {
        return centerRay_.direction * mainRayMaxDistance_;
    }
    public Vector3 GetHitPos()
    {
        return hitPos_;
    }
    public void RaysIsPainting(bool _para)
    {
        foreach (MeshPaintBrush brushRay in sideRayBrushArr)
        {
            brushRay.IsPainting(_para);
        }
    }
    /// <summary>
    /// //////////////////////////////
    /// </summary>
    public void RaysTimingDraw()
    {
        sideRayBrushArr[0].TimingDraw(centerRay_);
        sideRayBrushArr[1].TimingDraw(centerRay_);
        sideRayBrushArr[2].TimingDraw(centerRay_);
        sideRayBrushArr[3].TimingDraw(centerRay_);
        sideRayBrushArr[4].TimingDraw(centerRay_);

        // 노즐의 방향을 구함
        Vector3 nozzleDirection = transform.forward;
        // 노즐 방향을 기준으로 sprayAngle만큼 회전한 방향 벡터를 구함
        Quaternion dir0Rotation = Quaternion.AngleAxis(rayAngle_/12, -transform.right);
        Quaternion dir1Rotation = Quaternion.AngleAxis(rayAngle_/(12*2), -transform.right);
        Quaternion dir3Rotation = Quaternion.AngleAxis(rayAngle_/(12*2), transform.right);
        Quaternion dir4Rotation = Quaternion.AngleAxis(rayAngle_/12, transform.right);

        Vector3 dir0 = dir0Rotation * nozzleDirection;
        Vector3 dir1 = dir1Rotation * nozzleDirection;
      
        Vector3 dir3 = dir3Rotation * nozzleDirection;
        Vector3 dir4 = dir4Rotation * nozzleDirection;

        sideRayBrushArr[0].SetRayDirection(dir0);
        sideRayBrushArr[1].SetRayDirection(dir1);
        sideRayBrushArr[2].SetRayDirection(transform.forward);
        sideRayBrushArr[3].SetRayDirection(dir3);
        sideRayBrushArr[4].SetRayDirection(dir4);
    }
    public bool RaysIsPainting()
    {
        if (sideRayBrushArr[2].IsPainting() == true)
        {
            return true;
        }
        return false;
    }
    public void RaysStopCheckTargetProcess()
    {
        foreach (MeshPaintBrush brushRay in sideRayBrushArr)
        {
            brushRay.StopCheckTargetProcess();
        }
    }
    public void RaysStopTimingDrow()
    {
        foreach (MeshPaintBrush brushRay in sideRayBrushArr)
        {
            brushRay.StopTimingDraw();
        }
    }
} // end of class
