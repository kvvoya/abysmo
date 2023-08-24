using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PriceTag
{
   Low = 10,
   Medium = 20,
   High = 30
}

[CreateAssetMenu(menuName = "Upgrades/Upgrade")]
public class Upgrade : ScriptableObject
{
   public string upgradeName;
   [TextArea(5, 10)]
   public string description;
   public PriceTag priceTag;
   public Sprite sprite;
}
