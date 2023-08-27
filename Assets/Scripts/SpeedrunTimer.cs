using TMPro;
using UnityEngine;

public class SpeedrunTimer : MonoBehaviour
{
   public TextMeshProUGUI clockText;

   private float elapsedTime = 0f;
   private bool isRunning = false;

   private void Update()
   {
      if (isRunning)
      {
         elapsedTime += Time.deltaTime;

         string minutes = Mathf.Floor(elapsedTime / 60).ToString("00");
         string seconds = (elapsedTime % 60).ToString("00");
         string milliseconds = ((elapsedTime * 1000) % 1000).ToString("000");

         clockText.text = $"{minutes}:{seconds}.{milliseconds}";
      }
   }

   public void StartTimer()
   {
      isRunning = true;
   }

   public void StopTimer()
   {
      isRunning = false;
   }
}
