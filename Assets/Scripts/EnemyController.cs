using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour,IDamageable
{
    [SerializeField] private float speed = 0;
    [SerializeField] private int maxHealth = 0;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Vector2 screenBounds;
    private float objectHeight;
    private float objectWidth;
    private int CurrentHealth;

    private Camera camera1;


    private Vector2 direction=Vector2.zero;
    private void Start()
    {
        camera1=Camera.main;

        screenBounds = HelperMethods.GetScreenBounds(camera1);
        HelperMethods.GetObjectHeightAndWidth(spriteRenderer,out objectWidth,out objectHeight);

        direction=Vector2.right;

        CurrentHealth = maxHealth;
    }

    private void Update()
    {
        if (transform.position.x > screenBounds.x + objectWidth ||
            transform.position.x<screenBounds.x*-1-objectWidth)
        {
            direction *= -1;
        }

        transform.Translate(direction*speed*Time.deltaTime);
    }

    private void OnParticleCollision(GameObject other)
    {
        GotHit(10);
    }

    public void GotHit(int damage)
    {
        CurrentHealth -= damage;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);
         // Todo play hit sound
        if (CurrentHealth == 0)
        {
            // Todo play hit sound
            // Todo add xp to player
            // Todo play explosion effect
            // Todo disable object

            Destroy(gameObject);
        }
    }
}
