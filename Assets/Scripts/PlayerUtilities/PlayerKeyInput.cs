using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        }
    }
} // end of class
