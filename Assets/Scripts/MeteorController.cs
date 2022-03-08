using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorController : MonoBehaviour,IDamageable
{
    [Header("Movement")]
    [SerializeField] private float minSpeed = 0;
    [SerializeField] private float maxSpeed = 0;
    [SerializeField] private float minRotationSpeed = 0;
    [SerializeField] private float maxRotationSpeed = 0;
    [Header("Health And XP")]
    [SerializeField] private int xpOnDestroy = 0;
    [SerializeField] private int maxHealth = 0;
    [Header("Spawn on GameObject destroy ")]
    [SerializeField] private GameObject[] meteors;
    [Header("Effects")]
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private GameObject explosionEffect;
    [Header("Sounds")]
    [SerializeField] private AudioClip[] hitAudioClips;
    [SerializeField] private AudioClip explosionAudioClip;
    [Range(0f,1f)][SerializeField] private float hitVolume = 0;


    private int currentHealth;

    private float rotationSpeed;
    private float speedX, speedY;
    private bool moveOnX, moveOnY = true;
    private float zRotation = 0f;
    private Vector2 direction;

    private AudioSource audioSource;

    private void Start()
    {
        currentHealth=maxHealth;
        audioSource = GetComponent<AudioSource>();


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


    public void GotHit(int damage,Vector3 position)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth,0, maxHealth);

        // Play hitSound
        audioSource.PlayOneShot(hitAudioClips[Random.Range(0, hitAudioClips.Length)], hitVolume);

        // Play hit effect
        Instantiate(hitEffect, position, Quaternion.identity);

        if (currentHealth == 0)
        {
            GameManager.Instance.UpdateScore(xpOnDestroy);
            // Todo play explosion sound

            // Play explosion effect
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

            // Todo disable gameobject to reuse it with pool system

            SpawnMeteors();
            if(Random.Range(1,10)>6)
                CollectableSpawner.Instance.SpawnCollectables(transform.position);
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
