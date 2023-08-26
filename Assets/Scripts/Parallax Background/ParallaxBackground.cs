using System;
using System.Collections;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{

   private PressureManager pressureManager;
   public int transitionAt = 500;
   public SpriteRenderer[] parallaxLayers;

   private void Start()
   {
      pressureManager = FindObjectOfType<PressureManager>();
   }

   private void Update()
   {
      if (pressureManager.pressure > transitionAt)
      {
         Transition();
      }
   }

   private void Transition()
   {
      foreach (SpriteRenderer parallaxLayer in parallaxLayers)
      {
         StartCoroutine(FadeIn(parallaxLayer, 1f));
      }
   }

   private IEnumerator FadeIn(SpriteRenderer spriteRenderer, float speed)
   {
      if (spriteRenderer.color.a == 0)
      {
         float alpha = 0f;

         while (spriteRenderer.color.a < 1)
         {
            alpha += speed * Time.deltaTime;
            spriteRenderer.color = new Color(0.85f, 0.85f, 0.85f, alpha);
            yield return null;
         }
      }
   }
}
