using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
   [SerializeField] float maxMoveSpeed;
   [SerializeField] float accelerationSpeed = 1f;
   [SerializeField] float decreaseSpeed = 0.5f;

   private float moveSpeed;

   Rigidbody2D rb;

   private Vector2 movementVector;
   private Vector2 savedMovementVector = new Vector2();

   private void Start()
   {
      rb = GetComponent<Rigidbody2D>();
   }

   private void Update()
   {
      ProcessInputs();
      Move();
   }

   private void FixedUpdate()
   {
      ApplyVelocity();
   }

   private void ApplyVelocity()
   {
      Vector2 calculatedVector;

      if (movementVector.magnitude == 0)
      {
         calculatedVector = savedMovementVector;
      }
      else
      {
         calculatedVector = movementVector;
      }
      // Debug.Log($"Calculated vector: {calculatedVector}");

      // rb.velocity = new Vector2(calculatedVector.x * moveSpeed * Time.deltaTime, calculatedVector.y * moveSpeed * Time.deltaTime);
      rb.AddForce(new Vector2(calculatedVector.x * moveSpeed, calculatedVector.y * moveSpeed), ForceMode2D.Impulse);

      Vector2 clampedVelocity = rb.velocity;
      clampedVelocity.x = Mathf.Clamp(clampedVelocity.x, -moveSpeed, moveSpeed);
      clampedVelocity.y = Mathf.Clamp(clampedVelocity.y, -moveSpeed, moveSpeed);

      rb.velocity = clampedVelocity;
      Debug.Log(rb.velocity);
   }

   private void ProcessInputs()
   {
      float moveX = Input.GetAxisRaw("Horizontal");
      float moveY = Input.GetAxisRaw("Vertical");

      movementVector = new Vector2(moveX, moveY).normalized;
      if (movementVector.magnitude != 0)
      {
         savedMovementVector = new Vector2(moveX, moveY).normalized;
      }
   }

   private void AccelerateMovement()
   {
      if (moveSpeed < maxMoveSpeed)
      {
         moveSpeed += accelerationSpeed * Time.deltaTime;
      }
   }

   private void DecreaseMovement()
   {
      if (moveSpeed > 0)
      {
         moveSpeed -= decreaseSpeed * Time.deltaTime;
      }
   }


   private void Move()
   {
      if (movementVector.magnitude > 0)
      {
         AccelerateMovement();
      }
      else
      {
         DecreaseMovement();
      }
      moveSpeed = Mathf.Clamp(moveSpeed, 0f, maxMoveSpeed);
   }

}


