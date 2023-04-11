using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour, IPlayerMovement
{
    private Rigidbody rb_;

    private float walkSpeed_ =5f;
    private float jumpPower_ = 5;
    private float runSpeed_ = 7.5f;

    public bool isGround_ { get; set; }
    public bool isLeftShiftKeyInput_ { get; set; }

    private void Awake()
    {
        rb_ = this.GetComponent<Rigidbody>();
        isGround_ = true;
        isLeftShiftKeyInput_ = false;
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
        if (isLeftShiftKeyInput_) { Run(_direction); }
    }
    public void Run(Vector3 _direction)
    {
        transform.position = GetPlayerPos() + _direction * runSpeed_ * Time.deltaTime;
    }
    public void Jump()
    {
        if (isGround_) rb_.AddForce(Vector3.up * jumpPower_ , ForceMode.Impulse);
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
     //   if (_collision.gameObject.layer ==3) isGround_ = true;
        if (_collision.gameObject.tag =="Floor" || _collision.gameObject.tag == "Ladder") isGround_ = true;
    }

    public void SetPosition(Vector3 _direction, Quaternion _rotation)
    {
        this.transform.position = _direction;
        this.transform.rotation = _rotation;
    }
} // end of class
