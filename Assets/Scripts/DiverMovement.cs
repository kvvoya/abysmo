using System;
using UnityEngine;

public class DiverMovement : MonoBehaviour
{
   [Header("Start vars")]
   [SerializeField] float startMoveSpeed = 5f;
   [SerializeField] float startVelocityCap = 7f;
   [SerializeField] float startDampingStrength = 40f;
   [SerializeField] float startMass = 10f;

   [Space(20)]
   [Header("End vars")]

   [SerializeField] float endMoveSpeed;
   [SerializeField] float endVelocityCap;
   [SerializeField] float endDampingStrength;
   [SerializeField] float endMass;

   [Space(20)]
   [Header("Misc")]
   [SerializeField] bool isRightByDefault = true;
   [SerializeField] float rotationFactor = 1f;
   [SerializeField] float particleFactor = 0.7f;

   private float moveSpeed;
   private float velocityCap;
   private float dampingStrength;

   Rigidbody2D rb;
   PressureManager pressureManager;
   public new SpriteRenderer renderer;
   public Transform bodyTransform;
   public ParticleSystem trailParticleSystem;

   ParticleSystem.EmissionModule emissionModule;
   Animator animator;

   private Vector2 movementVector;


   private void Start()
   {
      rb = GetComponent<Rigidbody2D>();
      pressureManager = FindObjectOfType<PressureManager>();
      animator = GetComponent<Animator>();

      moveSpeed = startMoveSpeed;
      velocityCap = startVelocityCap;
      dampingStrength = startDampingStrength;
      rb.mass = startMass;

      emissionModule = trailParticleSystem.emission;
   }

   private void Update()
   {
      ProcessInputs();
      ApplyPressureFactors();
      RotateSprite();

      if (movementVector.magnitude > 0.01f)
      {
         animator.SetBool("isMoving", true);
      }
      else
      {
         animator.SetBool("isMoving", false);
      }
   }

   private void SetFlip(float moveX)
   {
      if (moveX > 0)
      {
         renderer.flipX = !isRightByDefault;
      }
      else if (moveX < 0)
      {
         renderer.flipX = isRightByDefault;
      }
   }

   private void FixedUpdate()
   {
      ApplyVelocity();
      ApplyDamping();
   }

   private void ApplyPressureFactors()
   {
      float pressure = pressureManager.pressure;

      moveSpeed = startMoveSpeed - (startMoveSpeed - endMoveSpeed) / 1000 * pressure;
      velocityCap = startVelocityCap - (startVelocityCap - endVelocityCap) / 1000 * pressure;
      dampingStrength = startDampingStrength + (endDampingStrength - startDampingStrength) / 1000 * pressure;
      rb.mass = startMass + (endMass - startMass) / 1000 * pressure;

      emissionModule.rateOverDistance = rb.mass * particleFactor;
   }

   private void ApplyVelocity()
   {
      rb.AddForce(new Vector2(movementVector.x * moveSpeed, movementVector.y * moveSpeed), ForceMode2D.Impulse);

      Vector2 clampedVelocity = rb.velocity;
      clampedVelocity.x = Mathf.Clamp(clampedVelocity.x, -velocityCap, velocityCap);
      clampedVelocity.y = Mathf.Clamp(clampedVelocity.y, -velocityCap, velocityCap);

      rb.velocity = clampedVelocity;

      // Debug.Log(rb.velocity);
   }

   private void RotateSprite()
   {
      float rotateValue = (renderer.flipX ? -1 : 1) * rb.velocity.y * rotationFactor;
      bodyTransform.rotation = Quaternion.Euler(0, 0, rotateValue);
   }

   private void ProcessInputs()
   {
      float moveX = Input.GetAxisRaw("Horizontal");
      float moveY = Input.GetAxisRaw("Vertical");

      movementVector = new Vector2(moveX, moveY).normalized;
      SetFlip(moveX);
   }


   private void ApplyDamping()
   {
      Vector2 dampingForce = -rb.velocity * dampingStrength;
      rb.AddForce(dampingForce, ForceMode2D.Force);
   }
}


