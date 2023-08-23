using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public struct LevelPartData
{
   public LevelPartData(Obstacle obstacleType, Vector3 obstaclePosition)
   {
      ObstacleType = obstacleType;
      ObstaclePosition = obstaclePosition;
   }

   public Obstacle ObstacleType { get; }
   public Vector3 ObstaclePosition { get; }
}

public class ObstacleSpawner : MonoBehaviour
{
   [SerializeField] List<Obstacle> obstacles = new List<Obstacle>();
   [SerializeField] int units = 1000;
   [SerializeField] Transform gridParent;
   [SerializeField] int toGenerateBefore;

   List<LevelPartData> generatedLevel = new List<LevelPartData>();

   int minHeight;
   int height = 10;

   int levelPartIndex = 0;
   LevelPartData nextLevelPart;

   Transform playerTransform;

   private void Start()
   {
      minHeight = DefineMinHeight();
      playerTransform = FindObjectOfType<Player>().transform;

      GenerateLevel();
      AstarPath.active.logPathResults = PathLog.None;
      nextLevelPart = generatedLevel[levelPartIndex];
   }

   private void Update()
   {

      if ((playerTransform.position.y - nextLevelPart.ObstaclePosition.y) < toGenerateBefore)
      {
         Obstacle newObject = Instantiate(nextLevelPart.ObstacleType, nextLevelPart.ObstaclePosition, Quaternion.identity, gridParent);

         levelPartIndex++;
         nextLevelPart = generatedLevel[levelPartIndex];
      }
   }

   private void GenerateLevel()
   {
      while ((units - height) > minHeight)
      {
         Obstacle newObstacle = GetRandomObstacle();
         if ((units - height) < newObstacle.Height)
         {
            break; // not a big deal honestly, it's alright
         }

         Vector3 newObjectPosition = new Vector3(0, -height, 0);

         Instantiate(newObstacle, newObjectPosition, Quaternion.identity, gridParent);
         generatedLevel.Add(new LevelPartData(newObstacle, newObjectPosition));
         height += newObstacle.Height;

      }
      AstarPath.active.Scan();
      foreach (Obstacle obstacle in FindObjectsOfType<Obstacle>())
      {
         Destroy(obstacle.gameObject);
      }

   }

   private Obstacle GetRandomObstacle()
   {
      int randomIndex = Random.Range(0, obstacles.Count);
      return obstacles[randomIndex];
   }

   private int DefineMinHeight()
   {
      int tempHeight = 9999999; // that should do it
      foreach (Obstacle obstacle in obstacles)
      {
         if (obstacle.Height < tempHeight)
         {
            tempHeight = obstacle.Height;
         }
      }
      return tempHeight;
   }
}
