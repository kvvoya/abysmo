using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class XPManager : MonoBehaviour
{
   public UnityEvent onGainXP;

   public static int collectedXP { get; private set; }

   private List<Upgrade> lowUpgrades = new List<Upgrade>();
   private List<Upgrade> mediumUpgrades = new List<Upgrade>();
   private List<Upgrade> highUpgrades = new List<Upgrade>();

   public static Upgrade lowUpgrade { get; private set; }
   public static Upgrade mediumUpgrade { get; private set; }
   public static Upgrade highUpgrade { get; private set; }

   UpgradeFunction upgradeFunction;
   Upgrade[] upgrades;
   UIManager uIManager;

   private void Start()
   {
      upgradeFunction = GetComponent<UpgradeFunction>();
      uIManager = FindObjectOfType<UIManager>();
      upgrades = upgradeFunction.upgrades;
      collectedXP = 0;

      ScanThroughUpgrades();
      GetShopRotation();
   }

   public void GainXP(int xp)
   {
      collectedXP += xp;
      onGainXP?.Invoke();
   }

   public void PurchaseUpgrade(Upgrade upgrade)
   {
      switch (upgrade.priceTag)
      {
         case PriceTag.Low:
            lowUpgrades.Remove(upgrade);
            collectedXP -= 10;
            break;
         case PriceTag.Medium:
            mediumUpgrades.Remove(upgrade);
            collectedXP -= 20;
            break;
         case PriceTag.High:
            highUpgrades.Remove(upgrade);
            collectedXP -= 30;
            break;
         default:
            Debug.LogWarning("price tag not found!");
            break;
      }
      upgradeFunction.Apply(upgrade);
      GetShopRotation();
      uIManager.PrepareShop();
   }

   private void ScanThroughUpgrades()
   {
      foreach (Upgrade upgrade in upgrades)
      {
         switch (upgrade.priceTag)
         {
            case PriceTag.Low:
               lowUpgrades.Add(upgrade);
               break;
            case PriceTag.Medium:
               mediumUpgrades.Add(upgrade);
               break;
            case PriceTag.High:
               highUpgrades.Add(upgrade);
               break;
            default:
               Debug.LogWarning("No price tag found");
               break;
         }
      }
   }

   private void GetShopRotation()
   {
      lowUpgrade = lowUpgrades.Count != 0 ? lowUpgrades[Random.Range(0, lowUpgrades.Count)] : null;
      mediumUpgrade = mediumUpgrades.Count != 0 ? mediumUpgrades[Random.Range(0, mediumUpgrades.Count)] : null;
      highUpgrade = highUpgrades.Count != 0 ? highUpgrades[Random.Range(0, highUpgrades.Count)] : null;
   }
}
