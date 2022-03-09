using System.Collections;
using System.Collections.Generic;
using Packages.Rider.Editor.UnitTesting;
using UnityEngine;

public class PlayerController : MonoBehaviour,IDamageable
{
   [Header("Movement")]
   [SerializeField] private float speed = 8.0f;
   [SerializeField] private float rotationTime = 0.1f;
   [SerializeField] private bool canRotate = false;
   [Header("Renderer")]
   [SerializeField] private SpriteRenderer spriteRenderer;
   [Header("Health")]
   [SerializeField] private int maxHealth = 0;
   [Header("Gun")]
   [SerializeField] private ParticleSystem[] gunParticles;
   [Header("Effects")]
   [SerializeField] private GameObject hitEffect;
   [SerializeField] private GameObject explosionEffect;
   [Header("Sounds")]
   [SerializeField] private AudioClip shootAudioClip;
   [Range(0f,1f)][SerializeField] private float shootVolume = 0;
   [SerializeField] private GameObject hitSoundPrefab;
   [SerializeField] private GameObject explosionSoundPrefab;


   private Rigidbody2D rBody2D;
   private Vector2 movement;
   private Camera camera1;
   private AudioSource audioSource;

   private List<ParticleCollisionEvent> collisionEvents=new List<ParticleCollisionEvent>();
   private float currentNumberOfParticles = 0;

   private Vector2 screenBounds;
   private float objectWidth;
   private float objectHeight;

   private int currentHealth;
   private bool canFire = false;

   private void Start()
   {
      camera1 = Camera.main;
      rBody2D = GetComponent<Rigidbody2D>();
      audioSource = GetComponent<AudioSource>();


      screenBounds = HelperMethods.GetScreenBounds(camera1);
      HelperMethods.GetObjectHeightAndWidth(spriteRenderer,out objectWidth,out objectHeight);

      currentHealth = maxHealth;

      UIManager.Instance.SetHealthSliderMaxValue(maxHealth);
      UIManager.Instance.UpdateHealthSlider(maxHealth);

   }

   private void Update()
   {
      MovementVector();
      if (Input.GetKeyDown(KeyCode.Space))
      {
         Shoot();
      }

      PlayShootSound();

      Test();
   }

   private void Test()
   {
      if (Input.GetKeyDown(KeyCode.U))
      {
         GotPowerUp();
      }
   }

   private void Shoot()
   {

      canFire = !canFire;

      // Start Emission
      foreach (ParticleSystem gunParticle in gunParticles)
      {
         var particleEmission = gunParticle.emission;
         particleEmission.enabled = canFire;
      }


      // Play shoot sound
      /*if (canFire && !audioSource.isPlaying)
      {
         print("Play");
         audioSource.clip = shootAudioClip;
         audioSource.volume = shootVolume;
         audioSource.loop = true;
         audioSource.Play();
      } else
      {
         print("Stop");
         audioSource.Stop();
      }*/
   }

   private void PlayShootSound()
   {
      if (gunParticles[0].particleCount > currentNumberOfParticles)
      {
         audioSource.PlayOneShot(shootAudioClip,shootVolume);
      }

      currentNumberOfParticles = gunParticles[0].particleCount;
   }

   private void MovementVector()
   {
      float hMove = Input.GetAxis("Horizontal");
      float vMove = Input.GetAxis("Vertical");

      movement = new Vector3(hMove, vMove, 0f).normalized;
   }

   private void FixedUpdate()
   {
      if(!GameManager.Instance.GetCanPlay())   return;

      rBody2D.MovePosition(rBody2D.position+movement*speed*Time.fixedDeltaTime);

      RotateTowardMousePosition();

      KeepPlayerInScreenBounds();
   }

   private void KeepPlayerInScreenBounds()
   {
      transform.position = new Vector2
      (
         Mathf.Clamp(rBody2D.position.x, screenBounds.x * -1 + objectWidth, screenBounds.x - objectWidth),
         Mathf.Clamp(rBody2D.position.y, screenBounds.y * -1 + objectHeight, screenBounds.y - objectHeight)
      );
   }

   private void RotateTowardMousePosition()
   {
      if(!canRotate) return;

      //calculate the difference in angle between the current angle and the destination angle:

      Vector2 lookDir = (Vector2) camera1.ScreenToWorldPoint(Input.mousePosition) - rBody2D.position;
      float currentAngle = rBody2D.rotation;
      float targetAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
      float angleDiff = targetAngle - currentAngle;

      // for any angles greater than or equal to 180, change them to their negative angle equivalent,
      // because it's the closer direction

      angleDiff = Mathf.Repeat(angleDiff + 180f, 360f) - 180f;

      // Then, rotate towards that angle
      targetAngle = currentAngle + angleDiff;
      float smoothedAngle = Mathf.Lerp(currentAngle, targetAngle, rotationTime);

      rBody2D.rotation = smoothedAngle;
   }

   public void GotHit(int damage,Vector3 position)
   {
      currentHealth -= damage;
      currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

      //Update Health UI
      UIManager.Instance.UpdateHealthSlider(currentHealth);

      // Play hit Effect
      GameObject hitParticle = PoolManager.Instance.ReuseGameObject(hitEffect, position, Quaternion.identity);
      hitParticle.SetActive(true);

      // Play hitSound
      GameObject hitSound = PoolManager.Instance.ReuseGameObject(hitSoundPrefab, position, Quaternion.identity);
      hitSound.SetActive(true);

      if (currentHealth == 0)
      {
         GameManager.Instance.GameOver();

         // Todo Play explosion sound effect

         // Play explosion effect
         GameObject expEffect = PoolManager.Instance.ReuseGameObject(explosionEffect, position, Quaternion.identity);
         expEffect.SetActive(true);

         Destroy(gameObject);
      }
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.CompareTag("Meteor") || other.CompareTag("Enemy"))
      {
         // Add Damage to other object
         other.GetComponent<IDamageable>().GotHit(101,other.transform.position);

         // Add damage to player
         GotHit(1000,other.transform.position);
      }

   }

   public void GotHealth()
   {
      currentHealth = maxHealth;
      UIManager.Instance.UpdateHealthSlider(currentHealth);
   }

   public void GotPowerUp()
   {
      foreach (ParticleSystem gunParticle in gunParticles)
      {
         var gunParticleEmission = gunParticle.emission;
         var rateOverTime =gunParticleEmission.rateOverTime.constant;
         rateOverTime+=1;
         rateOverTime = Mathf.Clamp(rateOverTime, 3.0f, 7.1f);
         gunParticleEmission.rateOverTime = rateOverTime;
      }
   }
}
