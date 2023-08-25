using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
   [SerializeField] Image black;
   [SerializeField] bool isBlackNow;

   public IEnumerator FadeIn(float speed, Action callback)
   {
      float alpha = 0;
      while (alpha < 1f)
      {
         alpha += speed * Time.deltaTime;
         black.color = new Color(0, 0, 0, alpha);
         yield return null;
      }
      yield return new WaitForSeconds(1f);
      callback();
   }

   public IEnumerator FadeAway(float speed, Action callback)
   {
      float alpha = 1f;
      while (alpha > 0f)
      {
         alpha -= speed * Time.deltaTime;
         black.color = new Color(0, 0, 0, alpha);
         yield return null;
      }
      yield return new WaitForSeconds(1f);
      callback();
   }

   private void Update()
   {

   }

   private void Awake()
   {
      if (isBlackNow)
      {
         black.color = new Color(0, 0, 0, 1);
         StartCoroutine(FadeAway(1f, () => { }));
      }
      else
      {
         black.color = new Color(0, 0, 0, 0);
      }
   }

   public void GoToScene(string sceneName, float speed = 0.5f)
   {
      StartCoroutine(FadeIn(speed, () => SceneManager.LoadScene(sceneName)));
   }
}
