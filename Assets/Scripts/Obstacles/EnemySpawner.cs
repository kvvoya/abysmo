using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct EnemySpawnData
{
   public Enemy enemy;
   public int minPressure;
   public float chance;

   public EnemySpawnData(Enemy enemy, int minPressure, float chance)
   {
      this.enemy = enemy;
      this.minPressure = minPressure;
      this.chance = chance;
   }
}

public class EnemySpawner : MonoBehaviour
{
   [SerializeField] float range = 15f;
   [SerializeField] List<EnemySpawnData> enemies;

   Transform playerTransform;
   Enemy myEnemy = null;

   PressureManager pressureManager;

   private void Start()
   {
      playerTransform = FindObjectOfType<Player>().transform;
      pressureManager = FindObjectOfType<PressureManager>();
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

      foreach (EnemySpawnData spawnData in enemies)
      {
         float randomValue = UnityEngine.Random.value;
         // Debug.Log($"{spawnData.enemy.name} - p/mp: {pressureManager.pressure} / {spawnData.minPressure} - random/chance: {randomValue} / {spawnData.chance} : {pressureManager.pressure >= spawnData.minPressure && randomValue < spawnData.chance}");
         if (pressureManager.pressure >= spawnData.minPressure && randomValue < spawnData.chance)
         {
            myEnemy = Instantiate(spawnData.enemy, transform.position, Quaternion.identity);
            return;
         }

      }
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
