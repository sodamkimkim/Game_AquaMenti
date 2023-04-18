using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTransformTrigger : MonoBehaviour
{
    private ParticleSystem particleSystem_ = null;
    private List<ParticleSystem.Particle> particlesList_ = null;
    private List<ParticleCollisionEvent> particleEventList_ = null;

    private void Awake()
    {
        TryGetComponent(out particleSystem_);
        particlesList_ = new List<ParticleSystem.Particle>();
        particleEventList_ = new List<ParticleCollisionEvent>();
    }


    private void OnParticleCollision(GameObject _other)
    {
        //int safeLength = particleSystem_.GetSafeCollisionEventSize();
        //if (collisionEvents.Length < safeLength)
        //    collisionEvents = new ParticleCollisionEvent[safeLength];
        //int numCollisionEvents = GetComponent<ParticleSystem>().GetCollisionEvents(other, collisionEvents);
        //int i = 0;
        //while (i < numCollisionEvents)
        //{
        //    Vector3 collisionHitLoc = collisionEvents[i].intersection;

        //    Instantiate(objectToInstantiate, collisionHitLoc, Quaternion.identity);
        //    i++;
        //}

        //Vector3 collisionHitRot = collisionEvents[i].normal;

        //Quaternion HitRot = Quaternion.LookRotation(Vector3.forward, collisionHitRot);

        int cnt = particleSystem_.GetCollisionEvents(_other, particleEventList_);
        int count = particleSystem_.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, particlesList_);
        //for (int i = 0; i < cnt; ++i)
        //{
        //    particleEventList_[i].normal;
        //}
        Debug.Log("count: " + count);
        if (particlesList_.Count > 0)
        {
            for (int i = 0; i < count; ++i)
            {
                MeshFilter mf = null;
                if (_other.TryGetComponent(out mf))
                {
                    ParticleSystem.Particle p = particlesList_[i];
                    p.rotation3D = Vector3.Reflect(particlesList_[i].position.normalized, mf.mesh.normals[0]);
                    particlesList_[i] = p;
                }
            }
        }
    }

    private void OnParticleTrigger()
    {
        //int count = particleSystem_.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, particlesList_);
        //Debug.Log("count: " + count);
        //if (particlesList_.Count > 0)
        //{
        //    foreach (ParticleSystem.Particle particle in particlesList_)
        //    {
        //        particle.velocity = Vector3.Reflect(particle.position, particle.)
        //    }
        //}
    }
    //{
    //int count = particleSystem_.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, particlesList_);
    //Debug.Log("count: " + count);
    //if (particlesList_.Count > 0)
    //{
    //    foreach (ParticleSystem.Particle particle in particlesList_)
    //    {
    //        particle.velocity = Vector3.Reflect(particle.position, )
    //    }
    //}
    //}
}
