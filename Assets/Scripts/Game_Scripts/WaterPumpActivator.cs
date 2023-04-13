using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPumpActivator : MonoBehaviour
{
    [SerializeField]
    private GameObject waterPump = null;
    [SerializeField]
    private ParticleSystem waterParticle = null;


    private void Start()
    {
        PlayPump(false);
    }

    private void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    PlayPump(true);
        //}
        //if (Input.GetMouseButtonUp(0))
        //{
        //    PlayPump(false);
        //}
    }



    public void PlayPump(bool _play)
    {
        ActivePump(_play);
        ActiveParticle(_play);
    }

    private void ActivePump(bool _active)
    {
        if (waterPump == null) return;

        waterPump.SetActive(_active);
    }

    private void ActiveParticle(bool _active)
    {
        if (waterParticle == null) return;

        if (_active)
            waterParticle.Play(true);
        else
            waterParticle.Stop(true);
    }
}
