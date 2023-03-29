using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour, IPlayerMovement
{

    private Rigidbody rb_;
    private float walkSpeed_ = 100;
    private float runSpeed_ = 10;

    public Transform GetPlayerTransform()
    {
        return this.GetComponent<Transform>();
    }
    private void Awake()
    {
        rb_ = this.GetComponent<Rigidbody>();
    }
    private void Update()
    {

    }
    public void Walk(Vector3 _direction)
    {
        rb_.AddForce(_direction * walkSpeed_, ForceMode.Force);
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
        throw new System.NotImplementedException();
    }

    public void SetAnimation()
    {
        throw new System.NotImplementedException();
    }


} // end of class
