using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour, IPlayerMovement
{

    private Rigidbody rb_;
    private CharacterController characterController;
    private float walkSpeed_ = 3000;
    private float jumpPower_ = 10000;
    private float runSpeed_ = 5;

    public Transform GetPlayerTransform()
    {
        return this.GetComponent<Transform>();
    }
    public Vector3 GetPlayerPos()
    {
        return this.GetComponent<Transform>().position;
    }
    private void Awake()
    {
        rb_ = this.GetComponent<Rigidbody>();
      //  characterController = this.GetComponent<CharacterController>();
    }
    private void Update()
    {
        //public void LookAtMouseCursor()
        //{
        //    Vector3 mousePos = Input.mousePosition;
        //    Vector3 playerPos = Camera.main.WorldToScreenPoint(this.transform.position);
        //    Vector3 dir = mousePos - playerPos;
        //    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //    this.transform.rotation = Quaternion.AngleAxis(-angle + 90.0f, Vector3.up);
        //}
   


    }
    public void Walk(Vector3 _direction)
    {
        //float axisV = Input.GetAxis("Vertical");
        //float axisH = Input.GetAxis("Horizontal");
        //float axisJ = Input.GetAxis("Jump");
        rb_.AddForce(_direction* walkSpeed_*Time.deltaTime, ForceMode.Force);
        //characterController.Move(new Vector3(axisH * walkSpeed_ * Time.deltaTime,0f, axisV * walkSpeed_ * Time.deltaTime));



    }
    public void Run()
    {
        throw new System.NotImplementedException();
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
    public void Jump()
    {
        //float axisJ = Input.GetAxis("Jump");
        //      characterController.Move(Vector3.up * jumpPower * axisJ * Time.deltaTime);
        rb_.AddForce(Vector3.up * jumpPower_ *Time.deltaTime);
    }

    public void SetAnimation()
    {
        throw new System.NotImplementedException();
    }


} // end of class
