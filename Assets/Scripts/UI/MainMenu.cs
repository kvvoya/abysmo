using System.Collections;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
   public GameObject pressAnyButtonPrompt;
   public AudioSource audioSource;

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
      StartCoroutine(FadeOut(0.25f));
   }

   private IEnumerator FadeOut(float speed)
   {
      while (audioSource.volume > 0)
      {
         audioSource.volume -= speed * Time.deltaTime;
         yield return null;
      }
   }
}
