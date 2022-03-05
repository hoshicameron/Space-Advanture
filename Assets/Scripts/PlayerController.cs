using System.Collections;
using System.Collections.Generic;
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
   [SerializeField] private ParticleSystem particle;

   private Rigidbody2D rBody2D;
   private Vector2 movement;
   private Camera camera1;

   private Vector2 screenBounds;
   private float objectWidth;
   private float objectHeight;

   private int currentHealth;
   private bool canFire = false;

   private void Start()
   {
      camera1 = Camera.main;
      rBody2D = GetComponent<Rigidbody2D>();

      screenBounds = HelperMethods.GetScreenBounds(camera1);
      HelperMethods.GetObjectHeightAndWidth(spriteRenderer,out objectWidth,out objectHeight);

      currentHealth = maxHealth;

   }

   private void Update()
   {
      MovementVector();
      if (Input.GetKeyDown(KeyCode.Space))
      {
         canFire = !canFire;

         var particleEmission = particle.emission;
         particleEmission.enabled = canFire;
      }
   }

   private void MovementVector()
   {
      float hMove = Input.GetAxis("Horizontal");
      float vMove = Input.GetAxis("Vertical");

      movement = new Vector3(hMove, vMove, 0f).normalized;
   }

   private void FixedUpdate()
   {
      rBody2D.MovePosition(rBody2D.position+movement*speed*Time.fixedDeltaTime);

      RotateTowardMousePosition();

      KeepPlayerInScreenBounds();
   }

   private void KeepPlayerInScreenBounds()
   {
      transform.position = new Vector2
      (
         Mathf.Clamp(rBody2D.position.x, screenBounds.x * -1 - objectWidth, screenBounds.x + objectWidth),
         Mathf.Clamp(rBody2D.position.y, screenBounds.y * -1 - objectHeight, screenBounds.y + objectHeight)
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

   public void GotHit(int damage)
   {
      currentHealth -= damage;
      currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

      if (currentHealth == 0)
      {
         // Todo show Game Over screen
         // Todo stop game
         // Todo Play explosion sound effect
         // Todo play explosion vfx

         Destroy(gameObject);
      }
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.CompareTag("MeteorController") || other.CompareTag("Enemy"))
      {
         // Add Damage to other object
         other.GetComponent<IDamageable>().GotHit(101);

         // Add damage to player
         GotHit(1000);
      }

   }

   private void OnParticleCollision(GameObject other)
   {
      print("Particle collision");
   }
}
