using UnityEngine;

public class Player : MonoBehaviour
{
   private void Start()
   {

   }

   private void Update()
   {
      Debugging();
   }

   private static void Debugging()
   {
      if (Input.GetKeyDown(KeyCode.Escape))
      {
         Application.Quit();
      }
   }

   public void Die()
   {
      Destroy(gameObject);
   }
}
