using System;
using UnityEngine;

public class Seaweed : MonoBehaviour
{
   [SerializeField] int toHeal;

   Health playerHealth;
   public Animator animator;

   private void Start()
   {
      playerHealth = FindObjectOfType<Player>().GetComponent<Health>();
   }


   private void Update()
   {

   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.TryGetComponent<Knife>(out Knife knife))
      {
         HealPlayer();
      }
   }

   private void HealPlayer()
   {
      GetComponent<Collider2D>().enabled = false;
      playerHealth.HealHP(toHeal);
      animator.SetTrigger("dieLol");
      Destroy(gameObject, 1f);
   }
}
