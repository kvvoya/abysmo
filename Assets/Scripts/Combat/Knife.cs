using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
   [SerializeField] int damage;

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.TryGetComponent<Health>(out Health targetHealth))
      {
         targetHealth.DealDamage(damage);
      }
   }
}
