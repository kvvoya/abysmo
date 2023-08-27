using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Flashlight : MonoBehaviour
{
   [SerializeField] float blinkChanceFactor;
   [SerializeField] Light2D ringLight;
   [SerializeField] Light2D directionalLight;
   [SerializeField] float startRingRadius;
   [SerializeField] float endRingRadius;
   [SerializeField] float startDirectionalLightRadius;
   [SerializeField] float endDirectionalLightRadius;
   [SerializeField] float onIntensity = 0.4f;
   [SerializeField] float offIntensity = 1f;
   [SerializeField] AudioClip flickerSound;

   public float directionalLightFactor = 1f;
   public float ringLightFactor = 1f;

   PressureManager pressureManager;
   AudioSource audioManager;
   float pressure;
   bool canBlink = true;

   private void Start()
   {
      pressureManager = FindObjectOfType<PressureManager>();
      audioManager = GetComponent<AudioSource>();

      InvokeRepeating("Blink", 3f, 3f);
   }

   private void Update()
   {
      pressure = pressureManager.pressure;
      ApplyPressureFactors();
   }

   private void ApplyPressureFactors()
   {
      ringLight.pointLightOuterRadius = (startRingRadius - (startRingRadius - endRingRadius) / 1000 * pressure) * ringLightFactor;
      directionalLight.pointLightOuterRadius = (startDirectionalLightRadius - (startDirectionalLightRadius - endDirectionalLightRadius) / 1000 * pressure) * directionalLightFactor;
   }

   private void Blink()
   {
      if (!canBlink) return;

      int randomNumber = Random.Range(0, (int)(1000 * blinkChanceFactor + 1));
      if (randomNumber < pressure)
      {
         int lightType = Random.Range(0, 2);
         StartCoroutine(BlinkLight(lightType == 0 ? ringLight : directionalLight));
      }
   }

   private IEnumerator BlinkLight(Light2D light)
   {
      int timeToBlink = Random.Range(5, 15);
      Debug.Log("timeToBlink: " + timeToBlink);
      audioManager.PlayOneShot(flickerSound);

      canBlink = false;
      light.intensity = offIntensity;
      yield return new WaitForSeconds(0.25f);
      light.intensity = onIntensity;
      yield return new WaitForSeconds(0.5f);
      light.intensity = offIntensity;
      yield return new WaitForSeconds(0.25f);
      light.intensity = onIntensity;
      yield return new WaitForSeconds(0.1f);
      light.intensity = offIntensity;
      yield return new WaitForSeconds(timeToBlink);
      light.intensity = onIntensity;
      canBlink = true;
   }
}
