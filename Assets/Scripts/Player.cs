using UnityEngine;

public class Player : MonoBehaviour
{
   public float pressureFactor = 1f;
   public float overFactor = 1f;

   [SerializeField] AudioClip deathSound;

   PressureManager pressureManager;

   bool isDead = false;

   private void Awake()
   {
      pressureManager = FindObjectOfType<PressureManager>();
   }

   private void Update()
   {
      Debugging();
   }

   private static void Debugging()
   {
      if (Input.GetKeyDown(KeyCode.Escape))
      {
         Application.Quit();
      }
   }

   public void Die()
   {
      if (isDead) return;
      isDead = true;
      UIManager uIManager = FindObjectOfType<UIManager>();
      GetComponent<Health>().enabled = false;

      GetComponent<DiverMovement>().enabled = false;
      GetComponent<PlayerCombat>().enabled = false;
      uIManager.DisableEscape();
      FindObjectOfType<FadeOut>().GoToScene("MainMenu", 0.2f);
      GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
      GetComponent<AudioSource>().PlayOneShot(deathSound, 0.6f);
      StartCoroutine(FindObjectOfType<MusicManager>().FadeOut(0.15f));
   }

   public int GetCalculatedPressure()
   {
      float result = Mathf.Floor(pressureManager.pressure * Mathf.Clamp(pressureFactor, 0, Mathf.Infinity));
      return (int)(result * overFactor);
   }

   public void OnKill()
   {
      pressureFactor += UpgradeFunction.Instance.onKillPressure;
      Invoke("RemoveUpgradePressure", UpgradeFunction.Instance.onKillPressureTime);
   }

   private void RemoveUpgradePressure()
   {
      pressureFactor -= UpgradeFunction.Instance.onKillPressure;
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.gameObject.CompareTag("Trident"))
      {
         Debug.Log("Trident!");
         GetComponent<Health>().enabled = false;
         GetComponent<DiverMovement>().enabled = false;
         FindObjectOfType<UIManager>().DisableEscape();
         StartCoroutine(FindObjectOfType<MusicManager>().FadeOut(0.15f));
         FindObjectOfType<FadeOut>().GoToScene("Win");
      }
   }
}
