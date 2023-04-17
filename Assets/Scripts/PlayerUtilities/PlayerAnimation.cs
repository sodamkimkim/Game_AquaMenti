using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim_ = null;

    private void Awake()
    {
        anim_ = GetComponent<Animator>(); 
    }
    public void IsWalk(bool _isWalk)
    {
        anim_.SetBool("IsWalking", _isWalk);
    }
} // end of class
