using UnityEngine;

public class XPManager : MonoBehaviour
{
   public static int collectedXP { get; private set; }

   private void Start()
   {
      collectedXP = 0;
   }

   public static void GainXP(int xp)
   {
      collectedXP += xp;
   }
}
