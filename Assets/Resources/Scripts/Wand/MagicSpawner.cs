using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSpawner : MonoBehaviour
{
    //private
    private LineRenderer lineRenderer;
    private float rayMaxDistance_ = 3000;
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();    
        lineRenderer.enabled = false;
    }
    private void Update()
    {
        RayShot();
    }
    public Transform GetMagicSpawnTransform()
    {
        return transform;
    }

    /// <summary>
    /// 지팡이로부터 Ray쏴주는 메서드
    /// </summary>
    private void RayShot()
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, transform.position);

        RaycastHit hit;
        Vector3 hitPos = Vector3.zero;
        if (Physics.Raycast(GetMagicSpawnTransform().position, GetMagicSpawnTransform().forward, out hit, rayMaxDistance_))
        {
            IInteractableObject target = hit.collider.GetComponentInParent<IInteractableObject>();
            if(target != null)
            {
                Debug.Log(target.GetName());
            }
            hitPos = hit.point;
        }
        else
        {
            hitPos = GetMagicSpawnTransform().position + GetMagicSpawnTransform().forward * rayMaxDistance_;
        }
    }
} // end of class
