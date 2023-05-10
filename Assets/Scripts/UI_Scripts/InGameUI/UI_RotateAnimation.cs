using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_RotateAnimation : MonoBehaviour
{
    private RectTransform rect_Transform_ = null;


    private void Awake()
    {
        TryGetComponent<RectTransform>(out rect_Transform_);
    }

    private void OnEnable()
    {
        RotateAnimation();
    }


    private void RotateAnimation()
    {
        StopCoroutine("RotateAnimationCoroutine");
        StartCoroutine("RotateAnimationCoroutine");
    }
    private IEnumerator RotateAnimationCoroutine()
    {
        Quaternion rot = Quaternion.AngleAxis(180f, Vector3.forward);
        float t = 0f;
        while(true)
        {
            if (t >= 1f) t = 0f;
            t += 0.01f;
            if (t >= 1f) t = 1f;

            rect_Transform_.rotation = Quaternion.Slerp(Quaternion.identity, rot, t) * Quaternion.Slerp(Quaternion.identity, rot, t);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
