using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
   [SerializeField] TextMeshProUGUI atmText;
   [SerializeField] Image healthMeter;
   [SerializeField] Image oxygenMeter;

   PressureManager pressureManager;
   Health playerHealth;
   Player player;
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
}
