using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSpawner : MonoBehaviour
{
    private Ray ray_;
    Vector3 hitPos_ = Vector3.zero;
    private float rayMaxDistance_ = 3000;
    private LineRenderer lineRenderer_;
    private void Awake()
    {
        lineRenderer_ = GetComponent<LineRenderer>();
        lineRenderer_.positionCount = 2;

    }
    private void Update()
    {
        RayShot();
        if (Input.GetKey(KeyCode.R))
        {
            Debug.DrawLine(GetPos(), hitPos_, Color.green, 1f);
        }
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
    /// 지팡이로부터 Ray쏴주는 메서드
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
