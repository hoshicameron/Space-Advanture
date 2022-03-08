using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunLuncher : MonoBehaviour
{
    [SerializeField] private int damage;
    private ParticleSystem part;
    private List<ParticleCollisionEvent> collisionEvents;


    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

        int i = 0;

        while (i < numCollisionEvents)
        {
            Vector3 pos = collisionEvents[i].intersection;
            other.GetComponent<IDamageable>().GotHit(damage,pos);
            i++;
        }
    }
}
