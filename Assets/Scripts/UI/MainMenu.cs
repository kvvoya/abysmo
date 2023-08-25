using UnityEngine;

public class MainMenu : MonoBehaviour
{
   public GameObject pressAnyButtonPrompt;

   private bool anyButtonPressed = false;

   private void Update()
   {
      if (!anyButtonPressed && Input.anyKeyDown)
      {
         anyButtonPressed = true;
         if (pressAnyButtonPrompt != null)
         {
            pressAnyButtonPrompt.SetActive(false);
         }
         StartGame();
      }
   }

   private void StartGame()
   {
      FindObjectOfType<FadeOut>().GoToScene("SampleScene");
   }
}
