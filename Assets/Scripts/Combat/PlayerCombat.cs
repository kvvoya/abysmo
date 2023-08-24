using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCombat : MonoBehaviour
{
   [SerializeField] GameObject knife;
   [SerializeField] Transform weaponParent;
   [SerializeField] SpriteRenderer parentRenderer;
   [SerializeField] UnityEvent onKnife;
   [SerializeField] float startCooldown;
   [SerializeField] float endCooldown;
   [SerializeField] float stayTime = 0.5f;

   float timeSinceLastAttacked = Mathf.Infinity;
   float cooldown;
   // bool isAttacking = false;

   Camera mainCamera;
   PressureManager pressureManager;
   UIManager uIManager;

   private void Start()
   {
      mainCamera = Camera.main;
      pressureManager = FindObjectOfType<PressureManager>();
      uIManager = FindObjectOfType<UIManager>();
   }

   private void Update()
   {
      Vector3 cursorPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
      cursorPosition.z = 0f;

      Vector3 direction = cursorPosition - transform.position;
      float angle = Vector2.SignedAngle(Vector2.right, direction);

      if (!knife.gameObject.activeInHierarchy && uIManager.IsInGame())
      {
         weaponParent.transform.rotation = Quaternion.Euler(0f, 0f, angle);
      }

      ApplyCooldown();

      if (Input.GetMouseButtonDown(0))
      {
         HandleAttackButton();
      }

      timeSinceLastAttacked += Time.deltaTime;
   }

   private void HandleAttackButton()
   {
      if (timeSinceLastAttacked > cooldown && uIManager.IsInGame())
      {
         knife.SetActive(true);

         // transform.localScale = new Vector3(parentRenderer.flipX ? -1 : 1, 1, 1);

         timeSinceLastAttacked = 0f;
         onKnife?.Invoke();
         StartCoroutine(ActivateKnife());
      }
   }

   private IEnumerator ActivateKnife()
   {
      yield return new WaitForSeconds(stayTime);
      knife.SetActive(false);
   }

   public float GetCDTimeRatio()
   {
      float result = Mathf.Min(timeSinceLastAttacked, cooldown) / cooldown;
      return result;
   }

   private void ApplyCooldown()
   {
      float pressure = pressureManager.pressure;

      cooldown = startCooldown + (endCooldown - startCooldown) / 1000 * pressure;
   }
}
