using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerFocusManager : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager_ = null;
    [SerializeField]
    private GameObject playerGo_ = null;
    private Staff staff_ = null;
    // 지팡이 Raycast관련 필드
    private WandRaySpawner wandRaySpawner_;
    // Player Rotate & Look 관련 필드
    private PlayerYRotate playerYRotate_;
    private UpperBodyLook upperBodyLook_;

    [SerializeField]
    private ScreenSideManager screenSideManager_;

    private MagicRotate magicRotate_;
    // Flag
    public bool isFocusFixed_ { get; set; }


    private void Awake()
    {
        wandRaySpawner_ = playerGo_.GetComponentInChildren<WandRaySpawner>();
        playerYRotate_ = playerGo_.GetComponentInChildren<PlayerYRotate>();
        upperBodyLook_ = playerGo_.GetComponentInChildren<UpperBodyLook>();
        //screenSideManager_ = GameObject.FindWithTag("Canvas_ScreenSide").GetComponent<ScreenSideManager>();

        isFocusFixed_ = true;

    }
    public void SetStaff(Staff _staff)
    {
        staff_ = _staff;
        magicRotate_ = playerGo_.GetComponentInChildren<MagicRotate>();
    }
    private void Update()
    {
        if (!gameManager_.isInGame_) return;
        // # FocusFixed 모드 or FocusMove 모드
        // # FocusFixed 모드 or FocusMove 모드
        if (isFocusFixed_)
        { // # FocusFixed 모드
            wandRaySpawner_.RayScreenCenterShot();
            playerYRotate_.RotateBodyAxisY(true);
            upperBodyLook_.RotateUpperBodyAxisX(true);

            if (staff_ != null)
            {
                staff_.LookAtCenter();
            }
        }
        else if (!isFocusFixed_)
        { // # FocusMove 모드
            wandRaySpawner_.RayMoveFocusShot();
            // # ScreenSide MouseOver 체크
            PlayerLookControlWhenScreenSideMouseHover();
          
            if (staff_ != null)
            {
                staff_.Move(wandRaySpawner_.GetCenterRayDir());
            }
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

        // # BottomSide 터치 : x축 회전
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
        wandRaySpawner_.RotateFocusPointUI();
    }

} // end of class
