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

    private void Awake()
    {
        sideRayMaxDistance_ = mainRayMaxDistance_ * 0.5f;
        screenCenter_ = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2, 0);
        isLadder_ = false;
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
        // Debug.Log("FocusFixed()");
        centerRay_ = Camera.main.ScreenPointToRay(screenCenter_);
      //  staff_.LookAtRay(centerRay_.direction);
        uIFocusPoint_.SetPos(screenCenter_);
        Debug.DrawRay(GetPos(), GetTransform().forward * mainRayMaxDistance_, Color.red);
        RayFindObject();
    }
    /// <summary>
    /// WandRaySpawner로부터 MousePosition으로 Ray쏴주는 메서드
    /// </summary>
    public void RayMoveFocusShot()
    {
        // mousePosition에 따라 화면 돌아가지 않게 해 줘야함
        // Debug.Log("FocusMove()");
        Vector3 mousePos = Input.mousePosition;
        centerRay_ = Camera.main.ScreenPointToRay(mousePos);
       // staff_.LookAtRay(centerRay_.direction);
        RaycastHit hit;
        if (Physics.Raycast(centerRay_, out hit, mainRayMaxDistance_))
        {
            uIFocusPoint_.SetPos(mousePos);
            Debug.DrawRay(GetPos(), GetTransform().forward * mainRayMaxDistance_, Color.red);
            RayFindObject();
        }

    }
    private void RayFindObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(centerRay_, out hit, mainRayMaxDistance_))
        {
            IInteractableObject target = hit.collider.GetComponentInParent<IInteractableObject>();
            if (target != null)
            {
                Debug.Log(target.GetName());

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
            hitPos_ = hit.point;
        }
        else
        {
            hitPos_ = GetPos() + GetTransform().forward * mainRayMaxDistance_;
        }
 
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
