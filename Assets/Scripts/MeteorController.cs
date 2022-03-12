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
    [SerializeField] private GameObject[] meteors=null;
    [Header("Effects")]
    [SerializeField] private GameObject hitEffectPrefab=null;
    [SerializeField] private GameObject explosionEffectPrefab=null;
    [Header("Sounds")]
    [SerializeField] private GameObject hitSoundPrefab=null;
    [SerializeField] private GameObject explosionSoundPrefab = null;


    private int currentHealth;

    private float rotationSpeed;
    private float speedX, speedY;
    private bool moveOnX, moveOnY = true;
    private float zRotation = 0f;
    private Vector2 direction;

    private AudioSource audioSource;

    private void OnEnable()
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
        GameObject hitSound = PoolManager.Instance.ReuseGameObject(hitSoundPrefab, position, Quaternion.identity);
        hitSound.SetActive(true);

        // Play hit effect
        GameObject hitEffect = PoolManager.Instance.ReuseGameObject(hitEffectPrefab, position, Quaternion.identity);
        hitEffect.SetActive(true);


        if (currentHealth == 0)
        {
            StartDeathSequence();
        }
    }

    private void StartDeathSequence()
    {

        GameManager.Instance.UpdateScore(xpOnDestroy);

        // Play explosion sound
        GameObject explosionSound =
            PoolManager.Instance.ReuseGameObject(explosionSoundPrefab, transform.position, Quaternion.identity);
        explosionSound.SetActive(true);

        // Play explosion effect
        GameObject explosionEffect =
            PoolManager.Instance.ReuseGameObject(explosionEffectPrefab, transform.position, Quaternion.identity);
        explosionEffect.SetActive(true);

        SpawnMeteors();

        // Randomize spawn collectables
        if (Random.Range(1, 10) > 6)
            CollectableSpawner.Instance.SpawnCollectables(transform.position);

        gameObject.SetActive(false);
    }

    /// <summary>
    /// If there are meteors to spawn, spawn them
    /// </summary>
    private void SpawnMeteors()
    {
        if (meteors.Length == 0) return;

        for (int i = 0; i < meteors.Length; i++)
        {
            Vector3 rnd = Random.insideUnitSphere;
            GameObject newGameObject = PoolManager.Instance.ReuseGameObject(meteors[i], new Vector3(transform.position.x +rnd.x,
                transform.position.y +rnd.y, 0), Quaternion.identity);
            newGameObject.SetActive(true);
        }
    }
}
