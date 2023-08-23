using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
   public Vector2 parallaxFactor;
   [HideInInspector] public Vector2 startPos;

   Transform cameraTransform;
   float updatedTransform;

   private void Start()
   {
      startPos = transform.position;
      cameraTransform = GameObject.Find("Follow Camera").transform;
   }

   private void Update()
   {
      float yValue, xOffset, yOffset;
      if (cameraTransform.position.y > updatedTransform)
      {
         yValue = transform.position.y;
      }
      else
      {
         yOffset = cameraTransform.position.y - startPos.y;

         yValue = (startPos.y + yOffset) * parallaxFactor.y;
         updatedTransform = cameraTransform.position.y;
      }

      xOffset = cameraTransform.position.x - startPos.x;
      Vector2 newPosition = new Vector2((startPos.x + xOffset) * parallaxFactor.x, yValue);

      transform.position = newPosition;
   }
}
