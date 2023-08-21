using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{

   Transform cameraTransform;

   private void Start()
   {
      cameraTransform = Camera.main.transform;
   }

   private void LateUpdate()
   {


   }
}
