using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandRaySpawner : MonoBehaviour
{
    private UIFocusPoint uIFocusPoint_;
    private Ray ray_;
    public Vector3 hitPos_ { get; set; }
    private float rayMaxDistance_ = 10;
    private Vector3 screenCenter_;

    public bool isCenterFocus_ { get; set; }
    public bool isEffectHorizontal { get; set; } // 마법 effect 각도 변경
    private bool isLadder_ = false;

    private void Awake()
    {
        uIFocusPoint_ = GameObject.FindWithTag("Canvas_Focus").GetComponentInChildren<UIFocusPoint>();
        screenCenter_ = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2, 0);
        isCenterFocus_ = true;
        isEffectHorizontal = true;
    }

    private void Update()
    {
        if (isCenterFocus_) RayScreenCenterShot();
        else if (!isCenterFocus_) RayMoveFocusShot();
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
    public bool isLadder()
    {
        return isLadder_;
    }

    /// <summary>
    /// 지팡이로부터 SreenCenter로 Ray쏴주는 메서드
    /// Ray 맞은 IInteractableObject 물체 인식 함.
    /// </summary>
    private void RayScreenCenterShot()
    {
        Debug.Log("FocusCenter()");
        ray_ = Camera.main.ScreenPointToRay(screenCenter_);
        uIFocusPoint_.SetPos(screenCenter_);
        Debug.DrawRay(GetPos(), GetTransform().forward * rayMaxDistance_, Color.red);
        RayFindObject();
    }
    private void RayMoveFocusShot()
    {
        //TODO
        Debug.Log("FocusMove()");
    }
    private void RayFindObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(ray_, out hit, rayMaxDistance_))
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
            hitPos_ = GetPos() + GetTransform().forward * rayMaxDistance_;
        }
    }

} // end of class
