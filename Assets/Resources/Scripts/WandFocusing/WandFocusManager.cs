using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandFocusManager : MonoBehaviour
{
    private WandRaySpawner wandRaySpawner_;
    private UIFocusPoint uiFocusPoint_;
    private Vector3 rayPos_;

    private void Awake()
    {
        wandRaySpawner_ = GameObject.FindWithTag("Player").GetComponentInChildren<WandRaySpawner>();
        uiFocusPoint_ = GameObject.FindWithTag("Canvas_Focus").GetComponentInChildren<UIFocusPoint>();
        rayPos_ = wandRaySpawner_.hitPos_;
    }
    private void Start()
    {

    }
    private void Update()
    {
        //Vector3 newPos = rayPos_;
        //newPos.z = 0;
        //uiFocusPoint_.GetTransform().localPosition = Camera.main.WorldToScreenPoint(newPos);




    }
} // end of class
