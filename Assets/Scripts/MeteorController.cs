using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorController : MonoBehaviour,IDamageable
{
    [SerializeField] private float Speed = 0;
    [SerializeField] private int damageAmount = 0;
    [SerializeField] private int maxHealth = 0;
    [SerializeField] private GameObject[] meteors;

    private int currentHealth;
    private Vector2 direction;

    private void Start()
    {
        currentHealth=maxHealth;
        if (Random.Range(0, 10) > 6)
        {
            direction=new Vector2((int) Random.Range(-1f,1f),-1f);
        } else
        {
            direction=Vector2.down;
        }
    }

    private void Update()
    {
        transform.Translate(direction*Speed*Time.deltaTime);
    }

    private void OnParticleCollision(GameObject other)
    {
        GotHit(10);
    }

    public void GotHit(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth,0, maxHealth);

        // Todo play hitSound

        if (currentHealth == 0)
        {
            // Todo Add xp to player
            // Todo play explosion sound
            // Todo play explosion effect
            // Todo disable gameobject to reuse it with pool system

            SpawnMeteors();
            Destroy(gameObject);
        }
    }

    private void SpawnMeteors()
    {
        if (meteors.Length == 0) return;

        for (int i = 0; i < meteors.Length; i++)
        {
            Vector3 rnd = Random.insideUnitSphere;
            Instantiate(meteors[i], new Vector3(transform.position.x +rnd.x,
                transform.position.y +rnd.y, 0), Quaternion.identity);
        }
    }
}
