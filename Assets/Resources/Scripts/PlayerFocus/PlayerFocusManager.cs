using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerFocusManager : MonoBehaviour
{
    // 지팡이 Raycast관련 필드
    private WandRaySpawner wandRaySpawner_;

    // Player Rotate & Look 관련 필드
    private PlayerYRotate playerYRotate_;
    private UpperBodyLook upperBodyLook_;

    // ScreenSideManager
    private ScreenSideManager screenSideManager_;

    // Flag
    public bool isFocusFixed_ { get; set; }

    private void Awake()
    {
        GameObject playerGo = GameObject.FindWithTag("Player");
        wandRaySpawner_ = playerGo.GetComponentInChildren<WandRaySpawner>();
        playerYRotate_ = playerGo.GetComponentInChildren<PlayerYRotate>();
        upperBodyLook_ = playerGo.GetComponentInChildren<UpperBodyLook>();
        screenSideManager_ = GameObject.FindWithTag("Canvas_ScreenSide").GetComponent<ScreenSideManager>();

        isFocusFixed_ = true;
 
    }
    private void Update()
    {
        // # FocusFixed 모드 or FocusMove 모드
        if (isFocusFixed_)
        { // # FocusFixed 모드
            wandRaySpawner_.RayScreenCenterShot();
            playerYRotate_.RotateBodyAxisY(true);
            upperBodyLook_.RotateUpperBodyAxisX(true);
        }
        else if (!isFocusFixed_)
        { // # FocusMove 모드
            wandRaySpawner_.RayMoveFocusShot();
            // # ScreenSide MouseOver 체크
            PlayerLookControlWhenScreenSideMouseHover();
        }

    }
    /// <summary>
    /// ScreenSide에 Mouse hover 되면 RotateBodyAxisY of RotateUpperBodyAxisX
    /// </summary>
    private void PlayerLookControlWhenScreenSideMouseHover()
    {
        // # TopSide 터치 : -x축 회전
        if (screenSideManager_.isScreenSideTop) upperBodyLook_.RotateUpperBodyUP(true);
        else upperBodyLook_.RotateUpperBodyUP(false);

        // # Bottom 터치 : x축 회전
        if (screenSideManager_.isScreenSideBottom) upperBodyLook_.RotateUpperBodyDown(true);
        else upperBodyLook_.RotateUpperBodyDown(false);

        // # LeftSide 터치 : -Y축 회전
        if (screenSideManager_.isScreenSideLeft) playerYRotate_.RotateBodyAxisYLeft(true);
        else playerYRotate_.RotateBodyAxisYLeft(false);

        // # RightSide 터치 : Y축 회전
        if (screenSideManager_.isScreenSideRight) playerYRotate_.RotateBodyAxisYRight(true);
        else playerYRotate_.RotateBodyAxisYLeft(false);
    }
   
} // end of class
