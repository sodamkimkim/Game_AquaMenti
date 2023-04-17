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

    // # side rays endPosition���� �ʵ�
    [SerializeField]
    private float rayPosDefaultOffset_ = 0.05f; // 5�� �� ray�� offset, center�� �������� �ش� ����ŭ �������� ���� (Rotate�� �⺻ �� : X)
    [SerializeField]
    private float mainRayMaxDistance_ = 10f;
    [SerializeField]
    private float sideRayMaxDistance_ = 0f; // centerRayMaxDistance �� 0.5��ŭ���� �ʱ�ȭ

    private Ray centerRay_;

    private Vector3 screenCenter_;
    public Vector3 hitPos_ { get; set; }
    public bool isLadder_ { get; set; }

    public string cleaningTargetName_ { get; private set; }
    public float cleaningPercent_ { get; private set; }
    public float rayAngle_ { get; set; } // �� �л� ����

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
    /// WandRaySpawner�κ��� SreenCenter�� Ray���ִ� �޼���
    /// Ray ���� IInteractableObject ��ü �ν� ��.
    /// </summary>
    public void RayScreenCenterShot()
    {
        centerRay_ = Camera.main.ScreenPointToRay(screenCenter_);
        uIFocusPoint_.SetPos(screenCenter_);
        RayFindObject();
    }
    /// <summary>
    /// WandRaySpawner�κ��� MousePosition���� Ray���ִ� �޼���
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
            // # ��ٸ� �ν�
            IInteractableObject target = hit.collider.GetComponentInParent<IInteractableObject>();
            if (target != null)
            {
                //Debug.Log(target.GetName());

                // target�̸��� Ladder�̸� bool ���� �ٲ㼭 Ladder�� ��ġ�� �ű� �� ����
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

            // # meshpaint target �ν�
            MeshPaintTarget meshPaintTarget = hit.collider.gameObject.GetComponent<MeshPaintTarget>();
            if (meshPaintTarget != null)
            {
                cleaningTargetName_ = meshPaintTarget.gameObject.name;
                cleaningPercent_ = meshPaintTarget.GetPercent();
                //Debug.Log("meshPaintTarget Name: " + cleaningTargetName_);
            }
            else
            {
                cleaningTargetName_ = "";
                //Debug.Log("meshPaintTarget Name: " + cleaningTargetName_);
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

        // ������ ������ ����
        Vector3 nozzleDirection = centerRay_.direction;
        // ���� ������ �������� sprayAngle��ŭ ȸ���� ���� ���͸� ����
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
        sideRayBrushArr[2].SetRayDirection(centerRay_.direction);
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
