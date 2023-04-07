using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// 항상 유효한 키는 이 클래스에 정의.
/// flag변수로 유효성 여부가 바뀌는 키는 상황에 따라 적절한 위치에 정의
/// </summary>
public class PlayerKeyInput : MonoBehaviour
{
    private PlayerMovement playerMovement_;
    [SerializeField]
    private PlayerFocusManager playerFocusManager_ = null;
    [SerializeField]
    private MagicManager magicManager_=null;
    [SerializeField]
    private InventoryManager inventoryManager_ = null;
    [SerializeField]
    private MeshPaintBrush meshPaintBrush_ = null;
    private void Awake()
    {
        playerMovement_ = GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        // walk상태 일때 달릴 수 있음
        if (Input.GetKey(KeyCode.LeftShift))
        {
            playerMovement_.isLeftShiftKeyInput = true;
        }
        else
        {
            playerMovement_.isLeftShiftKeyInput = false;
        }
        // move forward
        if (Input.GetKey(KeyCode.W))
        {
            playerMovement_.Walk(playerMovement_.GetPlayerTransform().forward);
        }
        // move backWard
        if (Input.GetKey(KeyCode.S))
        {
            playerMovement_.Walk(-playerMovement_.GetPlayerTransform().forward);
        }
        // move left
        if (Input.GetKey(KeyCode.A))
        {
            playerMovement_.Walk(-playerMovement_.GetPlayerTransform().right);
        }
        // move right
        if (Input.GetKey(KeyCode.D))
        {
            playerMovement_.Walk(playerMovement_.GetPlayerTransform().right);
        }
        // jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerMovement_.Jump();
        }
        // focus Center
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (playerFocusManager_.isFocusFixed_ == true) { playerFocusManager_.isFocusFixed_ = false; Debug.Log("isFocusFixed_ = false"); }
           else if (playerFocusManager_.isFocusFixed_ == false) { playerFocusManager_.isFocusFixed_ = true; Debug.Log("isFocusFixed_ = true"); }
        }
        // 마법영역 Rotate
        if(Input.GetKeyDown(KeyCode.R))
        {
            magicManager_.RotateWaterMagic();
        }
        // inventory on / off
        if(Input.GetKeyDown(KeyCode.I))
        {

            if (inventoryManager_.isInventoryPanOpen_ ==false) { inventoryManager_.OpenInventoryPan(); Debug.Log("InventoryPan open"); }
            else if (inventoryManager_.isInventoryPanOpen_ ==true) { inventoryManager_.CloseInventoryPan(); Debug.Log("InventoryPan close"); }

        }
        // # -홍석-
        if (Input.GetMouseButton(0))
        {
            //IsPainting(true);
            //PaintToTarget();
            meshPaintBrush_.TimingDraw();
            Debug.Log("마우스 좌클릭");
        }
        else if (meshPaintBrush_.IsPainting() == true)
        {
            meshPaintBrush_.IsPainting(false);
            meshPaintBrush_.StopCheckTargetProcess();
            meshPaintBrush_.StopTimingDraw();
            Debug.Log("마우스 좌클릭 해제");
        }
        //else if (drawCoroutine == true)
        //{
        //    StopTimingDraw();
        //    Debug.Log("몇 번 호출되나요?");
        //}

        // 임시 스펠 변경 //
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            meshPaintBrush_.stick.magicType = MeshPaintBrush.MagicType.Zero;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            meshPaintBrush_.stick.magicType = MeshPaintBrush.MagicType.One;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            meshPaintBrush_.stick.magicType = MeshPaintBrush.MagicType.Two;
        }
        // End 임시 스펠 변경 //

        // Utility
        // Dirty 모두 제거 (일정 %에 도달하면 사용할 부분(지금은 단일대상))
        if (meshPaintBrush_.GetTarget() != null && Input.GetKeyDown(KeyCode.E))
        {
            meshPaintBrush_.GetTarget().ClearTexture();
        }
        // Dirty 초기화 (초기화 버튼을 누른다면 적용할 부분(지금은 단일대상))
        if (meshPaintBrush_.GetTarget() != null && Input.GetKeyDown(KeyCode.R))
        {
            meshPaintBrush_.GetTarget().ResetTexture();
        }
        if (meshPaintBrush_.GetTarget() != null && meshPaintBrush_.GetTarget().IsDrawable() && Input.GetKeyDown(KeyCode.T))
        {
            meshPaintBrush_.GetTarget().CompleteTwinkle();
        }


    }
} // end of class
