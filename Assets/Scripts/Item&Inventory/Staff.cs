using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Staff : Item
{
   // private bool isLookAtRay_ { get; set; }
    private Vector3 originRotation_ = Vector3.zero;
    private void Awake()
    {
      //  isLookAtRay_ = false;
        Quaternion qt = this.transform.localRotation;
        originRotation_ = qt.eulerAngles;
    }
    public void LookAtRay(Vector3 _rayHitPos)
    {

            Debug.Log("LookAtRay");
        //    isLookAtRay_ = true;
            this.transform.LookAt(_rayHitPos);

    }
    public void Move(Vector3 _destPos)
    {
        Debug.Log("staff look at : "+ _destPos);
        this.gameObject.transform.forward = _destPos.normalized;
    }
    public void LookAtCenter()
    {
        //if (isLookAtRay_)
        //{
            Debug.Log("LookAtCenter");
           // isLookAtRay_ = false;
            Quaternion qt = Quaternion.EulerAngles(originRotation_);
            this.transform.localRotation = qt;
        //}
    }
} // end of class
