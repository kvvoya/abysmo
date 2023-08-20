using System;
using UnityEngine;

public class DiverMovement : MonoBehaviour
{
   [SerializeField] float moveSpeed;
   [SerializeField] float velocityCap;
   [SerializeField] float dampingStrength;

   Rigidbody2D rb;

   private Vector2 movementVector;

   private void Start()
   {
      rb = GetComponent<Rigidbody2D>();
   }

   private void Update()
   {
      ProcessInputs();
   }

   private void FixedUpdate()
   {
      ApplyVelocity();
      ApplyDamping();
   }

   private void ApplyVelocity()
   {
      rb.AddForce(new Vector2(movementVector.x * moveSpeed, movementVector.y * moveSpeed), ForceMode2D.Impulse);

      Vector2 clampedVelocity = rb.velocity;
      clampedVelocity.x = Mathf.Clamp(clampedVelocity.x, -velocityCap, velocityCap);
      clampedVelocity.y = Mathf.Clamp(clampedVelocity.y, -velocityCap, velocityCap);

      rb.velocity = clampedVelocity;
      Debug.Log(rb.velocity);
   }

   private void ProcessInputs()
   {
      float moveX = Input.GetAxisRaw("Horizontal");
      float moveY = Input.GetAxisRaw("Vertical");

      movementVector = new Vector2(moveX, moveY).normalized;
   }


   private void ApplyDamping()
   {
      Vector2 dampingForce = -rb.velocity * dampingStrength;
      rb.AddForce(dampingForce, ForceMode2D.Force);
   }
}


