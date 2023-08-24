using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeFunction : MonoBehaviour
{
   public Upgrade[] upgrades;

   public void Apply(Upgrade upgrade)
   {
      Debug.Log("Applied " + upgrade.name);
      if (upgrade == upgrades[0]) // abbyst
      {

      }
      else if (upgrade == upgrades[1]) // adrenaline
      {

      }
      else if (upgrade == upgrades[2]) // battery borrow
      {

      }
      else if (upgrade == upgrades[3]) // blood rush
      {

      }
      else if (upgrade == upgrades[4]) // iron will
      {

      }
      else if (upgrade == upgrades[5]) // mako insticts
      {

      }
      else if (upgrade == upgrades[6]) // ninja
      {

      }
      else if (upgrade == upgrades[7]) // old rusty
      {

      }
      else if (upgrade == upgrades[8]) // payback
      {

      }
   }
}
