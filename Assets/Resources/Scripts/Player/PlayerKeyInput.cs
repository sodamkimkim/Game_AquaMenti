using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeyInput : MonoBehaviour
{
    private PlayerMovement playerMovement_;
    private void Awake()
    {
        playerMovement_ = GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        // move forward
        if(Input.GetKey(KeyCode.W))
        {
            playerMovement_.Walk(transform.forward);
        }
        // move backWard
        if(Input.GetKey(KeyCode.S))
        {
            playerMovement_.Walk(-transform.forward);
        }
        // move left
        if(Input.GetKey(KeyCode.A))
        {
            playerMovement_.Walk(-transform.right);
        }
        // move right
        if(Input.GetKey(KeyCode.D))
        {
            playerMovement_.Walk(transform.right);
        }
    }
} // end of class
