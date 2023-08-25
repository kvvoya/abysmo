using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
   [SerializeField] TextMeshProUGUI atmText;
   [SerializeField] TextMeshProUGUI expText;
   [SerializeField] TextMeshProUGUI balanceText;
   [SerializeField] Image healthMeter;
   [SerializeField] Image oxygenMeter;
   [SerializeField] Image knifeCdMeter;
   [SerializeField] Image harpoonCdMeter;
   [SerializeField] GameObject upgradesMenu;

   [Space(10)]
   [Header("Upgrade Slots")]
   [SerializeField] UpgradeSlot lowSlot;
   [SerializeField] UpgradeSlot mediumSlot;
   [SerializeField] UpgradeSlot highSlot;

   PressureManager pressureManager;
   Health playerHealth;
   Player player;
   PlayerCombat playerCombat;
   OxygenManager oxygenManager;
   CursorManager cursorManager;
   Animator animator;
   public Animator upgradesAnimator;

   bool isInGame = true;
   bool canPressEscape = true;

   public void DisableEscape()
   {
      canPressEscape = false;
   }

   public bool IsInGame() { return isInGame; }

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
      expText.text = $"{XPManager.collectedXP} EXP";
      balanceText.text = $"Balance: {XPManager.collectedXP} EXP";
      healthMeter.fillAmount = (float)playerHealth.health / playerHealth.MaxHealth;
      oxygenMeter.fillAmount = oxygenManager.GetPercentage();

      if (Input.GetKeyDown(KeyCode.E))
      {
         HandleEscape();
      }
   }

   public void PrepareShop()
   {
      lowSlot.myUpgrade = XPManager.lowUpgrade;
      mediumSlot.myUpgrade = XPManager.mediumUpgrade;
      highSlot.myUpgrade = XPManager.highUpgrade;
      BroadcastMessage("RefreshInfo");
   }

   private void RefreshInfo()
   {
      return;
   }

   public void HandleEscape()
   {
      if (!canPressEscape) return;

      if (isInGame)
      {
         canPressEscape = false;
         upgradesMenu.SetActive(true);
         isInGame = false;
         Time.timeScale = 0f;
         PrepareShop();
         upgradesAnimator.SetBool("isInGame", isInGame);
         cursorManager.SetCursorType(true);

         StartCoroutine(FromIsInGame(0.5f));
      }
      else
      {
         isInGame = true;
         canPressEscape = false;
         upgradesAnimator.SetBool("isInGame", isInGame);
         cursorManager.SetCursorType(false);
         StartCoroutine(ToIsInGame(0.5f));
      }
   }

   private IEnumerator FromIsInGame(float duration)
   {
      yield return new WaitForSecondsRealtime(duration);
      canPressEscape = true;
   }

   private IEnumerator ToIsInGame(float duration)
   {
      yield return new WaitForSecondsRealtime(duration);
      canPressEscape = true;
      Time.timeScale = 1f;
      upgradesMenu.SetActive(false);
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
      while (playerCombat.GetCDTimeRatioKnife() < 1f)
      {
         knifeCdMeter.fillAmount = playerCombat.GetCDTimeRatioKnife();
         yield return null;
      }
      knifeCdMeter.gameObject.SetActive(false);
   }

   public void OnHarpoonUsed()
   {
      harpoonCdMeter.gameObject.SetActive(true);
      StartCoroutine(UseHarpoon());
   }

   private IEnumerator UseHarpoon()
   {
      while (playerCombat.GetCDTimeRatioHarpoon() < 1f)
      {
         harpoonCdMeter.fillAmount = playerCombat.GetCDTimeRatioHarpoon();
         yield return null;
      }
      harpoonCdMeter.gameObject.SetActive(false);
   }

   public void Die()
   {
      throw new NotImplementedException();
   }
}
