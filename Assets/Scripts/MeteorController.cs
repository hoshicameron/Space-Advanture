using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorController : MonoBehaviour,IDamageable
{
    [SerializeField] private float minSpeed = 0;
    [SerializeField] private float maxSpeed = 0;
    [SerializeField] private float minRotationSpeed = 0;
    [SerializeField] private float maxRotationSpeed = 0;

    [SerializeField] private int maxHealth = 0;
    [SerializeField] private GameObject[] meteors;

    private int currentHealth;

    private float rotationSpeed;
    private float speedX, speedY;
    private bool moveOnX, moveOnY = true;
    private float zRotation = 0f;
    private Vector2 direction;

    private void Start()
    {
        currentHealth=maxHealth;

        rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);

        speedX = Random.Range(minSpeed, maxSpeed);
        speedY = speedX;

        if (Random.Range(0, 2) > 0) speedX *= -1;
        if (Random.Range(0, 2) > 0) rotationSpeed *= -1;
        if (Random.Range(0, 2) > 0) moveOnX = false;
    }

    private void Update()
    {
        HandleMovementX();
        HandleMovementY();

        RotateMeteor();
    }

    private void HandleMovementX()
    {
        if(!moveOnX)    return;

        var tempPosition = transform.position;
        tempPosition.x += speedX * Time.deltaTime;
        transform.position= tempPosition;

    }

    private void HandleMovementY()
    {
        if(!moveOnY)     return;
        var tempPosition = transform.position;
        tempPosition.y += speedY * Time.deltaTime * -1;
        transform.position = tempPosition;
    }

    private void RotateMeteor()
    {
        zRotation += rotationSpeed * Time.deltaTime;
        transform.rotation=Quaternion.AngleAxis(zRotation,Vector3.forward);
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
