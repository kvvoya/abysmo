using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
   [SerializeField] TextMeshProUGUI atmText;
   [SerializeField] Image healthMeter;
   [SerializeField] Image oxygenMeter;
   [SerializeField] Image knifeCdMeter;

   PressureManager pressureManager;
   Health playerHealth;
   Player player;
   PlayerCombat playerCombat;
   OxygenManager oxygenManager;
   CursorManager cursorManager;
   Animator animator;

   bool isInGame = true;

   private void Start()
   {
      pressureManager = FindObjectOfType<PressureManager>();
      player = FindObjectOfType<Player>();
      playerHealth = player.GetComponent<Health>();
      oxygenManager = FindObjectOfType<OxygenManager>();
      cursorManager = FindObjectOfType<CursorManager>();
      animator = GetComponent<Animator>();
      playerCombat = player.GetComponent<PlayerCombat>();

      cursorManager.SetCursorType(false);
   }

   private void Update()
   {
      atmText.text = $"{pressureManager.pressure} atm";
      healthMeter.fillAmount = (float)playerHealth.health / 100;
      oxygenMeter.fillAmount = oxygenManager.GetPercentage();
   }

   public void OnDamage()
   {
      animator.SetTrigger("playerDamaged");
   }

   public void OnKnifeUsed()
   {
      knifeCdMeter.gameObject.SetActive(true);
      StartCoroutine(UseKnife());
   }

   private IEnumerator UseKnife()
   {
      while (playerCombat.GetCDTimeRatio() < 1f)
      {
         knifeCdMeter.fillAmount = playerCombat.GetCDTimeRatio();
         yield return null;
      }
      knifeCdMeter.gameObject.SetActive(false);
   }
}
