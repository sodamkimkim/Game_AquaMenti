using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class PlayerMovement : MonoBehaviour, IPlayerMovement
{
    //private Rigidbody rb_;
    private CharacterController characterController_;

    [SerializeField] private float walkSpeed_ = 5f;
    [SerializeField] private float jumpPower_ = 5f;
    [SerializeField] private float runSpeed_ = 7.5f;
    private float gravity_ = -9.8f;
    private float beforePosY_ = 0f;


    public bool isGround_ { get; set; }
    private bool isJump_ { get; set; }

    public bool isLeftShiftKeyInput_ { get; set; }

    private void Awake()
    {
        //rb_ = this.GetComponent<Rigidbody>();
        characterController_ = GetComponent<CharacterController>();
        isGround_ = true;
        isJump_ = false;
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

        if (isJump_)
        {
            gravity_ = jumpPower_;
            if (transform.position.y >= jumpPower_ + beforePosY_)
            {
                isJump_ = false;
            }
        }
        else if (!isJump_ || characterController_.isGrounded == false)
        {
            gravity_ += -9.8f * Time.deltaTime;
        }
        else
        {
            gravity_ = 0f;
        }
        _direction.y = gravity_;
        // rb_.velocity = (_direction * walkSpeed_);
        // transform.position = GetPlayerPos() + _direction * walkSpeed_ * Time.deltaTime;
        if (isLeftShiftKeyInput_)
        {
       
            Run(_direction);
        }
        else
        {
            characterController_.Move(transform.TransformDirection(_direction) * Time.deltaTime * walkSpeed_);
            
        };
    }
    public void Run(Vector3 _direction)
    {
        //transform.position = GetPlayerPos() + _direction * runSpeed_ * Time.deltaTime;
        //rb_.velocity = (_direction * runSpeed_);
        characterController_.Move(transform.TransformDirection(_direction) * Time.deltaTime * runSpeed_);
    }
    public void Jump()
    {
        /*        if (isGround_) rb_.AddForce(Vector3.up * jumpPower_ , ForceMode.Impulse);
                isGround_ = false;*/

        if (characterController_.isGrounded == true)
        {
            isJump_ = true;
            beforePosY_ = transform.position.y;
        }



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

    public void SetPosition(Vector3 _direction, Quaternion _rotation)
    {
        this.transform.position = _direction;
        this.transform.rotation = _rotation;
        Physics.SyncTransforms();
    }
    public void OnCollisionEnter(Collision _collision)
    {
        //   if (_collision.gameObject.layer ==3) isGround_ = true;
        if (_collision.gameObject.tag == "Floor" || _collision.gameObject.tag == "Ladder") isGround_ = true;
    }
} // end of class
