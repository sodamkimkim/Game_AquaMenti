using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeyInput : MonoBehaviour
{
    private PlayerMovement playerMovement_;
    private WandRaySpawner magicSpawner_;

    private void Awake()
    {
        playerMovement_ = GetComponent<PlayerMovement>();
        magicSpawner_ = GetComponentInChildren<WandRaySpawner>();
    }
    private void FixedUpdate()
    {
        // walk상태 일때 달릴 수 있음
        if(Input.GetKey(KeyCode.LeftShift))
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
        if (Input.GetKey(KeyCode.Space))
        {
            playerMovement_.Jump();
        }
        // raycast line 그리기
        if (Input.GetKey(KeyCode.R))
        {
            Debug.DrawLine(magicSpawner_.GetPos(), magicSpawner_.GetHitPos(), Color.green, 2f);
        }
        // 사다리 들기
        if(Input.GetKey(KeyCode.F))
        {
            // ray에 사다리가 충돌했으면 해당 사다리의 pos를 바꿔줌.

        }
    }
} // end of class
