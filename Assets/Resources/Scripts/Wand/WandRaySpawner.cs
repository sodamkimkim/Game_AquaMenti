using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandRaySpawner : MonoBehaviour
{
    private Ray ray_;
    Vector3 hitPos_ = Vector3.zero;
    private float rayMaxDistance_ = 10;
    private LineRenderer lineRenderer_;

    private bool isLadder_ = false;
    private void Awake()
    {
        lineRenderer_ = GetComponent<LineRenderer>();
        lineRenderer_.positionCount = 2;

    }
    private void Update()
    {
        RayShot();

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
    public Vector3 GetHitPos()
    {
        return hitPos_;
    }
    public bool isLadder()
    {
        return isLadder_;
    }

    /// <summary>
    /// 지팡이로부터 Ray쏴주는 메서드
    /// Ray 맞은 IInteractableObject 물체 인식 함.
    /// </summary>
    private void RayShot()
    {
        ray_.origin = GetPos();
        ray_.direction = transform.forward;
        //Vector3 wPos = transform.localToWorldMatrix * new Vector4(GetLocalPos().x, GetLocalPos().y, GetLocalPos().z, 1f);
      //  lineRenderer_.SetPosition(0, wPos);
        lineRenderer_.SetPosition(0, GetPos());

        RaycastHit hit;
        hitPos_ = Vector3.zero;
        if (Physics.Raycast(ray_, out hit, rayMaxDistance_))
        {
            IInteractableObject target = hit.collider.GetComponentInParent<IInteractableObject>();
            if(target != null)
            {
                Debug.Log(target.GetName());

                // target이름이 Ladder이면 bool 값을 바꿔서 Ladder의 위치를 옮길 수 있음
                if(target.GetName() == "Ladder")
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
            hitPos_ = GetPos() + GetTransform().forward * rayMaxDistance_;
        }
        lineRenderer_.SetPosition(1, hitPos_);
        lineRenderer_.enabled = true;
    }
} // end of class
