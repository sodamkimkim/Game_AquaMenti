using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerFocusManager : MonoBehaviour
{
    [SerializeField]
    private GameObject playerGo = null;
    [SerializeField]
    private GameManager gameManager_ = null;
    [SerializeField]
    private Staff staff_ = null;
    // 지팡이 Raycast관련 필드
    private WandRaySpawner wandRaySpawner_;

    // Player Rotate & Look 관련 필드
    private PlayerYRotate playerYRotate_;
    private UpperBodyLook upperBodyLook_;

    [SerializeField]
    private ScreenSideManager screenSideManager_;
    [SerializeField]
    private MagicRotate magicRotate_;
    // Flag
    public bool isFocusFixed_ { get; set; }

    private void Awake()
    {
        
        wandRaySpawner_ = playerGo.GetComponentInChildren<WandRaySpawner>();
        playerYRotate_ = playerGo.GetComponentInChildren<PlayerYRotate>();
        upperBodyLook_ = playerGo.GetComponentInChildren<UpperBodyLook>();
        //screenSideManager_ = GameObject.FindWithTag("Canvas_ScreenSide").GetComponent<ScreenSideManager>();

        isFocusFixed_ = true;

    }
    private void Update()
    {
        if (!gameManager_.isInGame_) return;
        // # FocusFixed 모드 or FocusMove 모드
        if (isFocusFixed_)
        { // # FocusFixed 모드
            wandRaySpawner_.RayScreenCenterShot();
            playerYRotate_.RotateBodyAxisY(true);
            upperBodyLook_.RotateUpperBodyAxisX(true);
            staff_.LookAtCenter();
        }
        else if (!isFocusFixed_)
        { // # FocusMove 모드
            wandRaySpawner_.RayMoveFocusShot();
            // # ScreenSide MouseOver 체크
            PlayerLookControlWhenScreenSideMouseHover();
            staff_.LookAtRay(wandRaySpawner_.GetHitPos());
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


    public void RotateWaterMagic()
    {
        magicRotate_.RotateWaterMagic();
    }

} // end of class
