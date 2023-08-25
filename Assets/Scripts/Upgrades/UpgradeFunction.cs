using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeFunction : MonoBehaviour
{
   [SerializeField] Knife knife;

   public Upgrade[] upgrades;

   Player player;
   Health playerHealth;

   bool adrenalineRush = false;

   public bool isIronWill { get; private set; }
   public bool isNinjaDiven { get; private set; }
   public bool isOldRustyGun { get; private set; }
   public bool isPayback { get; private set; }
   public float onKillPressure { get; private set; }
   public float onKillPressureTime { get; private set; }

   private static UpgradeFunction instance;

   public static UpgradeFunction Instance
   {
      get
      {
         if (instance == null)
         {
            instance = FindObjectOfType<UpgradeFunction>();

            if (instance == null)
            {
               GameObject singletonObject = new GameObject(typeof(UpgradeFunction).Name);
               instance = singletonObject.AddComponent<UpgradeFunction>();
            }
         }
         return instance;
      }
   }

   private void Awake()
   {
      if (instance != null && instance != this)
      {
         Destroy(gameObject);
      }
      else
      {
         instance = this;
      }
   }

   private void Start()
   {
      player = FindObjectOfType<Player>();
      playerHealth = player.GetComponent<Health>();
      isIronWill = false;
      isNinjaDiven = false;
      isOldRustyGun = false;
      isPayback = false;
      onKillPressure = 0;
      onKillPressure = 0;
   }

   public void Apply(Upgrade upgrade)
   {
      Debug.Log("Applied " + upgrade.name);
      if (upgrade == upgrades[0]) // abbyst
      {
         player.pressureFactor -= 0.5f;
         player.GetComponent<Health>().UpgradeMaxHP(25);
         FindObjectOfType<OxygenManager>().depletionFactor += .20f;
      }
      else if (upgrade == upgrades[1]) // adrenaline
      {
         player.pressureFactor -= 0.9f;
         OxygenManager oxygenManager = FindObjectOfType<OxygenManager>();
         oxygenManager.onKill += 0.1f;
         adrenalineRush = true;
         StartCoroutine(AdrenalineRushAfter(oxygenManager));
         Invoke("TurnOffAdrenaline", 20f);
      }
      else if (upgrade == upgrades[2]) // battery borrow
      {
         Flashlight flashlight = player.GetComponent<Flashlight>();
         flashlight.directionalLightFactor += 0.3f;
         flashlight.ringLightFactor -= 0.2f;
      }
      else if (upgrade == upgrades[3]) // blood rush
      {
         PlayerCombat playerCombat = player.GetComponent<PlayerCombat>();
         OxygenManager oxygenManager = FindObjectOfType<OxygenManager>();

         knife.damageFactor += 1.0f;
         playerCombat.KnifeCooldownFactor = 0.15f;
         playerCombat.HarpoonCooldownFactor = 0f;
         oxygenManager.depletionFactor += 2f;
         StartCoroutine(BloodRushAfter(playerCombat, oxygenManager));

      }
      else if (upgrade == upgrades[4]) // iron will
      {
         isIronWill = true;
         InvokeRepeating("IronWillHeal", 5f, 5f);
      }
      else if (upgrade == upgrades[5]) // mako insticts
      {
         FindObjectOfType<OxygenManager>().onKill -= 0.01f;
         onKillPressure -= 0.2f;
         onKillPressureTime = 3f;
      }
      else if (upgrade == upgrades[6]) // ninja
      {
         player.GetComponent<PlayerCombat>().KnifeCooldownFactor -= 0.20f;
         isNinjaDiven = true;
      }
      else if (upgrade == upgrades[7]) // old rusty
      {
         player.pressureFactor += 0.5f;
         isOldRustyGun = true;
         Invoke("TurnOffRusty", 30f);
      }
      else if (upgrade == upgrades[8]) // payback
      {
         isPayback = true;
         player.GetComponent<Health>().UpgradeMaxHP(-50);
      }

   }

   private void IronWillHeal()
   {
      playerHealth.HealHP(2);
   }

   private void TurnOffAdrenaline()
   {
      adrenalineRush = false;
   }

   private void TurnOffRusty()
   {
      isOldRustyGun = false;
      player.pressureFactor -= 0.5f;
   }

   private IEnumerator AdrenalineRushAfter(OxygenManager oxygenManager)
   {
      playerHealth = player.GetComponent<Health>();
      while (adrenalineRush)
      {
         playerHealth.DealDamage(2, false);
         yield return new WaitForSeconds(1f);
      }
      player.pressureFactor += 0.9f;
      oxygenManager.onKill -= 0.1f;
   }

   private IEnumerator BloodRushAfter(PlayerCombat playerCombat, OxygenManager oxygenManager)
   {
      yield return new WaitForSeconds(60);
      FindObjectOfType<Knife>().damageFactor -= 1.0f;
      playerCombat.KnifeCooldownFactor = 1f;
      playerCombat.HarpoonCooldownFactor = 1f;
      oxygenManager.depletionFactor -= 2f;
   }
}
