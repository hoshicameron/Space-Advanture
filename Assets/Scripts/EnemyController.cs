using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour,IDamageable
{
    [Header("Movement")]
    [SerializeField] private float minSpeed = 0;
    [SerializeField] private float maxSpeed = 0;
    [SerializeField] private float decisionTime;
    [SerializeField] private float minRotationSpeed = 0;
    [SerializeField] private float maxRotationSpeed = 0;
    [SerializeField] private bool canRotate = false;
    [Header("Health and XP")]
    [SerializeField] private int maxHealth = 0;
    [SerializeField] private int xpOnDestroy = 0;
    [SerializeField] private SpriteRenderer spriteRenderer = null;
    [Header("Health Bar")]
    [SerializeField] private Slider healthSlider=null;
    [Header("Effects")]
    [SerializeField] private GameObject hitEffect=null;
    [SerializeField] private GameObject explosionEffect=null;
    [Header("Sounds")]
    [SerializeField] private GameObject hitSoundPrefab=null;
    [SerializeField] private AudioClip shootAudioClip=null;
    [Range(0f,1f)][SerializeField] private float shootVolume = 0;
    [Header("Gun")]
    [SerializeField] private ParticleSystem[] gunParticle = null;
    [SerializeField] private float minStartDelay = 0;
    [SerializeField] private float maxStartDelay = 0;



    private Vector2 screenBounds;
    private float objectHeight;
    private float objectWidth;
    private int CurrentHealth;

    private bool horizontalMovement = false;
    private float rotationSpeed=0f;
    private float zRotation = 0f;

    private AudioSource audioSource;
    private Camera camera1;
    private float speed;



    private Vector2 direction=Vector2.zero;

    private void Awake()
    {
        float randomDelay = Random.Range(minStartDelay, maxStartDelay);
        foreach (ParticleSystem particle in gunParticle)
        {
            var particleMain = particle.main;
            particleMain.startDelay = randomDelay;
        }
    }

    private void Start()
    {
        camera1=Camera.main;
        audioSource = GetComponent<AudioSource>();

        screenBounds = HelperMethods.GetScreenBounds(camera1);
        HelperMethods.GetObjectHeightAndWidth(spriteRenderer,out objectWidth,out objectHeight);

        direction=Vector2.right;

        CurrentHealth = maxHealth;
        speed = Random.Range(minSpeed, maxSpeed);

        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;

        horizontalMovement = Random.Range(1, 10) > 3? true:false;

        rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);

    }

    private void Update()
    {
        ChangeMoveDirection();

        if (horizontalMovement)
        {
            HorizontalMovement();
        } else
        {
            VerticalMovement();
        }

        Rotate();
    }

    private void ChangeMoveDirection()
    {
        if (Time.time >= decisionTime)
        {
            horizontalMovement = Random.Range(1, 10) > 3 ? true : false;
            if (horizontalMovement) direction = Vector2.right;
            else direction = Vector2.down;

            decisionTime += Time.time;
        }
    }

    private void LateUpdate()
    {
        KeepWithinScreenHorizontal();
        KeepWithinScreenVertical();

    }

    private void HorizontalMovement()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    public void VerticalMovement()
    {
        transform.Translate(direction*speed*Time.deltaTime);
    }

    private void KeepWithinScreenHorizontal()
    {
        Vector3 transformPosition = transform.position;
        transformPosition.x = Mathf.Clamp(transform.position.x, screenBounds.x * -1 + objectWidth,
                                           screenBounds.x - objectWidth);
       transform.position = transformPosition;

        if (transform.position.x >= screenBounds.x - objectWidth
            ||
            transform.position.x <= screenBounds.x * -1 + objectWidth
           )
        {
            direction *= -1;
        }
    }

    private void KeepWithinScreenVertical()
    {
        Vector3 transformPosition = transform.position;
       transformPosition.y = Mathf.Clamp(transform.position.y, screenBounds.y * -1 + objectHeight,
            screenBounds.y - objectHeight);
        transform.position = transformPosition;

        if (transform.position.y >= screenBounds.y - objectHeight
            ||
            transform.position.y <= screenBounds.y * -1 + objectHeight)
        {
            direction *= -1;
        }
    }




    public void GotHit(int damage,Vector3 position)
    {
        CurrentHealth -= damage;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);

        // Update health bar UI
        healthSlider.value = CurrentHealth;

        // Play hit particle
        Instantiate(hitEffect, position, Quaternion.identity);

        // Play hitSound
        PoolManager.Instance.ReuseGameObject(hitSoundPrefab, transform.position, Quaternion.identity);


        // Todo play hit sound
        if (CurrentHealth == 0)
        {
            // Todo play hit sound
            GameManager.Instance.UpdateScore(xpOnDestroy);

            // Play explosion effect
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

            // Todo disable object

            EnemySpawner.Instance.CheckForSpawnNewWave(gameObject);
            Destroy(gameObject);
        }
    }

    private void Rotate()
    {
        if(!canRotate)    return;

        zRotation += rotationSpeed * Time.deltaTime;
        transform.rotation=Quaternion.AngleAxis(zRotation,Vector3.forward);
    }
}
