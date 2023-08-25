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
   [SerializeField] UnityEvent onHarpoon;
   [SerializeField] float startCooldownKnife;
   [SerializeField] float endCooldownKnife;
   [SerializeField] float startCooldownHarpoon;
   [SerializeField] float endCooldownHarpoon;
   [SerializeField] float stayTime = 0.5f;
   [SerializeField] float hookSpeed = 10f;

   float timeSinceLastAttacked = Mathf.Infinity;
   float timeSinceLastHarpoon = Mathf.Infinity;
   float knifeCooldown;
   float harpoonCooldown;

   public float KnifeCooldownFactor { get; set; }
   public float HarpoonCooldownFactor { get; set; }

   GameObject currentHook;
   bool isHooked = false;
   // bool isAttacking = false;

   Camera mainCamera;
   Player player;
   UIManager uIManager;
   LineRenderer lineRenderer;

   private void Start()
   {
      mainCamera = Camera.main;
      player = GetComponent<Player>();
      uIManager = FindObjectOfType<UIManager>();
      lineRenderer = GetComponent<LineRenderer>();

      KnifeCooldownFactor = 1f;
      HarpoonCooldownFactor = 1f;
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

      if (timeSinceLastAttacked < knifeCooldown && UpgradeFunction.Instance.isNinjaDiven)
      {
         player.overFactor = 1.5f;
      }
      else
      {
         player.overFactor = 1f;
      }

      timeSinceLastAttacked += Time.deltaTime;
      timeSinceLastHarpoon += Time.deltaTime;
   }

   private void HandleHook()
   {
      if (knife.activeInHierarchy || currentHook != null || timeSinceLastHarpoon < harpoonCooldown) return;

      Vector2 targetPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
      Vector2 playerPosition = transform.position;
      Vector2 direction = (targetPosition - playerPosition).normalized;
      float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

      timeSinceLastHarpoon = 0f;
      onHarpoon?.Invoke();

      currentHook = Instantiate(harpoonPrefab, playerPosition, Quaternion.Euler(new Vector3(0, 0, angle)), transform);
      Rigidbody2D hookRb = currentHook.GetComponent<Rigidbody2D>();
      lineRenderer.positionCount = 2;
      lineRenderer.SetPosition(0, transform.position);
      lineRenderer.SetPosition(1, currentHook.transform.position);
      hookRb.velocity = direction * hookSpeed;
   }

   private void HandleAttackButton()
   {
      if (timeSinceLastAttacked > knifeCooldown && uIManager.IsInGame() && currentHook == null)
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

   public float GetCDTimeRatioKnife()
   {
      float result = Mathf.Min(timeSinceLastAttacked, knifeCooldown) / knifeCooldown;
      return result;
   }

   public float GetCDTimeRatioHarpoon()
   {
      float result = Mathf.Min(timeSinceLastHarpoon, harpoonCooldown) / harpoonCooldown;
      return result;
   }

   private void ApplyCooldown()
   {
      float pressure = player.GetCalculatedPressure();

      knifeCooldown = (startCooldownKnife + (endCooldownKnife - startCooldownKnife) / 1000 * pressure) * KnifeCooldownFactor;
      harpoonCooldown = (startCooldownHarpoon + (endCooldownHarpoon - startCooldownHarpoon) / 1000 * pressure) * HarpoonCooldownFactor;
   }
}
