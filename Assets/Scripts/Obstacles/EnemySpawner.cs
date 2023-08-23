using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
   [SerializeField] float range = 15f;
   [SerializeField] List<Enemy> enemies;

   Transform playerTransform;
   Enemy myEnemy = null;

   private void Start()
   {
      playerTransform = FindObjectOfType<Player>().transform;
      SpawnEnemy();
   }

   private void Update()
   {
      float distance = Vector2.Distance(transform.position, playerTransform.position);

      if (distance > range)
      {
         SpawnEnemy();
      }
   }

   private void SpawnEnemy()
   {
      if (myEnemy != null) return;

      Enemy toGenerate = enemies[Random.Range(0, enemies.Count)];
      myEnemy = Instantiate(toGenerate, transform.position, Quaternion.identity);
   }

   private void OnDrawGizmos()
   {
      Gizmos.color = Color.red;
      Gizmos.DrawSphere(transform.position, 0.1f);

   }

   private void OnDrawGizmosSelected()
   {
      Gizmos.color = Color.red;
      Gizmos.DrawWireSphere(transform.position, range);
   }

   private void OnDestroy()
   {
      if (myEnemy != null)
      {
         Destroy(myEnemy.gameObject);
      }
   }
}
