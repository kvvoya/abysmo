using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
   [SerializeField] int maxHealth;
   public int health { get; private set; }

   private void Start()
   {
      health = maxHealth;
   }

   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.N))
      {
         DealDamage(10);
      }
      CheckIfDead();
   }

   public void DealDamage(int damage)
   {
      health -= damage;
   }

   private void CheckIfDead()
   {
      if (health <= 0)
      {
         Die();
      }
   }

   private void Die()
   {
      Debug.Log("Dead");
   }
}
