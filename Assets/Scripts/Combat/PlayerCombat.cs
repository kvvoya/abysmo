using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCombat : MonoBehaviour
{
   [SerializeField] GameObject knife;
   [SerializeField] GameObject harpoonPrefab;
   [SerializeField] Transform weaponParent;
   [SerializeField] SpriteRenderer parentRenderer;
   [SerializeField] UnityEvent onKnife;
   [SerializeField] float startCooldown;
   [SerializeField] float endCooldown;
   [SerializeField] float stayTime = 0.5f;
   [SerializeField] float hookSpeed = 10f;

   float timeSinceLastAttacked = Mathf.Infinity;
   float cooldown;

   GameObject currentHook;
   bool isHooked = false;
   // bool isAttacking = false;

   Camera mainCamera;
   PressureManager pressureManager;
   UIManager uIManager;
   LineRenderer lineRenderer;

   private void Start()
   {
      mainCamera = Camera.main;
      pressureManager = FindObjectOfType<PressureManager>();
      uIManager = FindObjectOfType<UIManager>();
      lineRenderer = GetComponent<LineRenderer>();
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
      else if (Input.GetMouseButtonDown(1))
      {
         HandleHook();
      }

      if (currentHook != null)
      {
         lineRenderer.SetPosition(0, transform.position);
         lineRenderer.SetPosition(1, currentHook.transform.position);
      }

      timeSinceLastAttacked += Time.deltaTime;
   }

   private void HandleHook()
   {
      if (knife.activeInHierarchy || currentHook != null) return;

      Vector2 targetPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
      Vector2 playerPosition = transform.position;
      Vector2 direction = (targetPosition - playerPosition).normalized;
      float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

      currentHook = Instantiate(harpoonPrefab, playerPosition, Quaternion.Euler(new Vector3(0, 0, angle)), transform);
      Rigidbody2D hookRb = currentHook.GetComponent<Rigidbody2D>();
      lineRenderer.positionCount = 2;
      lineRenderer.SetPosition(0, transform.position);
      lineRenderer.SetPosition(1, currentHook.transform.position);
      hookRb.velocity = direction * hookSpeed;
   }

   private void HandleAttackButton()
   {
      if (timeSinceLastAttacked > cooldown && uIManager.IsInGame() && currentHook == null)
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
