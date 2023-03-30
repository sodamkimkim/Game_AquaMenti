using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour, IPlayerMovement
{
    private Rigidbody rb_;
    private Camera mainCam;
    private float walkSpeed_ =5f;
    private float jumpPower_ = 10000;
    private float runSpeed_ = 7.5f;

    public bool isGround_ { get; set; }
    public bool isLeftShiftKeyInput { get; set; }

    private void Awake()
    {
        rb_ = this.GetComponent<Rigidbody>();
        mainCam = Camera.main;
        mainCam.transform.SetParent(this.transform);
        Vector3 newCamPos = mainCam.transform.localPosition;
        newCamPos.y = 0.66f;
        newCamPos.z = 0.18f;
        mainCam.transform.localPosition = newCamPos;
        //  characterController = this.GetComponent<CharacterController>();
    }
    public Transform GetPlayerTransform()
    {
        return this.GetComponent<Transform>();
    }
    public Vector3 GetPlayerPos()
    {
        return this.GetComponent<Transform>().position;
    }
    public void Walk(Vector3 _direction)
    {
        transform.position = GetPlayerPos() + _direction * walkSpeed_ * Time.deltaTime;
        if (isLeftShiftKeyInput) { Run(_direction); }
    }
    public void Run(Vector3 _direction)
    {
        transform.position = GetPlayerPos() + _direction * runSpeed_ * Time.deltaTime;
    }
    public void Jump()
    {
        if (isGround_) rb_.AddForce(Vector3.up * jumpPower_ * Time.deltaTime);
        isGround_ = false;
    }
    public void StandUp()
    {
        throw new System.NotImplementedException();
    }
    public void SitDown()
    {
        throw new System.NotImplementedException();
    }
    public void KneelDown()
    {
        throw new System.NotImplementedException();
    }


    public void SetAnimation()
    {
        throw new System.NotImplementedException();
    }
    public void OnCollisionEnter(Collision _collision)
    {
        if (_collision.gameObject.CompareTag("Walkable")) isGround_ = true;
    }

} // end of class
