using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFocusManager : MonoBehaviour
{
    // 지팡이 Raycast관련 필드
    private WandRaySpawner wandRaySpawner_;

    // Player Rotate & Look 관련 필드
    private PlayerYRotate playerYRotate_;
    private UpperBodyLook upperBodyLook_;
    public bool isFocusFixed_ { get; set; }

    private void Awake()
    {
        GameObject playerGo = GameObject.FindWithTag("Player");
        wandRaySpawner_ = playerGo.GetComponentInChildren<WandRaySpawner>();
        playerYRotate_ = playerGo.GetComponentInChildren<PlayerYRotate>();
        upperBodyLook_ = playerGo.GetComponentInChildren<UpperBodyLook>();
        isFocusFixed_ = true;
    }
    private void Update()
    {
        // # FocusFixed 모드 or FocusMove 모드
        if (isFocusFixed_)
        { // # FocusFixed 모드
            wandRaySpawner_.RayScreenCenterShot();
            playerYRotate_.RotateBodyAxisY();
            upperBodyLook_.RotateUpperBodyAxisX();
        }
        else if (!isFocusFixed_)
        { // # FocusMove 모드
            wandRaySpawner_.RayMoveFocusShot();
        }
    }
} // end of class
