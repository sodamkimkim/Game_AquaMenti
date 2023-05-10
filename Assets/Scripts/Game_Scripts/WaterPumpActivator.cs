using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPumpActivator : MonoBehaviour
{
    [SerializeField]
    private GameObject waterPump_ = null;
    [SerializeField]
    private ParticleSystem waterParticle_ = null;

    private bool isRotate_ = false;


    private bool IsRotate()
    {
        return isRotate_;
    }
    private void IsRotate(bool _isRot)
    {
        isRotate_ = _isRot;
    }


    private void Start()
    {
        PlayPump(false);
    }


    public void PlayPump(bool _play)
    {
        ActivePump(_play);
        ActiveParticle(_play);
    }

    private void ActivePump(bool _active)
    {
        if (waterPump_ == null) return;

        waterPump_.SetActive(_active);
    }

    private void ActiveParticle(bool _active)
    {
        if (waterParticle_ == null) return;

        if (_active)
        {
            waterParticle_.Play(true);
            SoundManager.Instance.Play("WaterFluid");
        }
        else
        {
            waterParticle_.Stop(true);
            SoundManager.Instance.Stop("WaterFluid");
        }
    }

    public void RotateParticle()
    {
        IsRotate(!IsRotate());

        Vector3 shapeRot = waterParticle_.shape.rotation;
        if (IsRotate())
            shapeRot.x = 0f;
        else
            shapeRot.x = 90f;

        ParticleSystem.ShapeModule shape = waterParticle_.shape;
        shape.rotation = shapeRot;
    }

    public void AngleParticle(float _angle)
    {
        Vector3 shapeRot = waterParticle_.shape.rotation;
        switch (_angle)
        {
            default:
                shapeRot.z = -_angle / 2f;
                break;
            case 0f:
                shapeRot.z = 0f;
                break;
        }

        ParticleSystem.ShapeModule shape = waterParticle_.shape;
        shape.arc = _angle;
        shape.rotation = shapeRot;
    }
}
