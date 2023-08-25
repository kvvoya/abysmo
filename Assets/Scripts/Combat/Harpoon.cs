using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Harpoon : MonoBehaviour
{
   [SerializeField] int hitDamage;
   [SerializeField] float pullForce = 10f;
   [SerializeField] float destroyAt = 3f;
   [SerializeField] float destroyAfter = 1f;
   [SerializeField] float harpoonMaxDistance = 10f;

   new Rigidbody2D rigidbody2D;
   LineRenderer lineRenderer;

   private int baseDamage;

   bool isHooked = false;

   Transform playerTransform;
   float distance;

   private void Start()
   {
      baseDamage = hitDamage;

      rigidbody2D = GetComponent<Rigidbody2D>();
      playerTransform = FindObjectOfType<Player>().transform;
      lineRenderer = playerTransform.GetComponent<LineRenderer>();

      if (UpgradeFunction.Instance.isOldRustyGun)
      {
         hitDamage = baseDamage * 10;
      }
      else
      {
         hitDamage = baseDamage;
      }
   }

   private void Update()
   {
      distance = Vector2.Distance(transform.position, playerTransform.position);

      if (isHooked)
      {
         DestroyAfterDistanceTimeHooked();
      }
      else
      {
         DestroyVoidCaseILoveMadeline();
      }
   }

   private void DestroyVoidCaseILoveMadeline()
   {
      if (distance >= harpoonMaxDistance)
      {
         Destroy(gameObject);
      }
   }

   private void DestroyAfterDistanceTimeHooked()
   {
      if (distance <= destroyAt || distance >= harpoonMaxDistance)
      {
         Destroy(gameObject);
      }
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (isHooked) return;

      if (other.gameObject.CompareTag("Hittable"))
      {
         if (other.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
         {
            transform.SetParent(enemy.transform, true);
            enemy.GetComponent<Health>().DealDamage(hitDamage);
            rigidbody2D.velocity = Vector2.zero;
            rigidbody2D.isKinematic = true;
            isHooked = true;
            PullEnemy(enemy.gameObject);
            Destroy(gameObject, destroyAfter);
         }
         else
         {
            Destroy(gameObject);
         }
      }
   }

   private void PullEnemy(GameObject enemy)
   {
      Vector2 pullDirection = (playerTransform.position - enemy.transform.position).normalized;
      enemy.GetComponent<Rigidbody2D>().AddForce(pullDirection * pullForce, ForceMode2D.Force);
   }

   private void OnDestroy()
   {
      lineRenderer.positionCount = 0;
   }
}
