using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderBoundaries : MonoBehaviour
{
   Transform playerPosition;
   float updatedPlayerPosition;

   private void Start()
   {
      playerPosition = FindObjectOfType<DiverMovement>().transform;
   }

   private void Update()
   {
      if (playerPosition.position.y > updatedPlayerPosition) return;

      updatedPlayerPosition = playerPosition.position.y;
      transform.position = new Vector2(transform.position.x, playerPosition.position.y);
   }
}
