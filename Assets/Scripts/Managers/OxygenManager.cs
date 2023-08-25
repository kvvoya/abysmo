using System.Collections;
using UnityEngine;

public class OxygenManager : MonoBehaviour
{
   [Tooltip("In seconds")]
   [SerializeField] float maxTimeLimit = 600;

   public float depletionFactor = 1f;
   public float onKill = 0f;

   public float TimeLimit { get; private set; }

   private Health playerHealth;

   private void Start()
   {
      TimeLimit = maxTimeLimit;
      playerHealth = FindObjectOfType<Player>().GetComponent<Health>();
   }

   private void Update()
   {
      TimeLimit -= Time.deltaTime * depletionFactor;
      CheckIfTimeIsOut();
   }

   private void CheckIfTimeIsOut()
   {
      if (TimeLimit <= 0)
      {
         StartCoroutine(KillPlayer(10, 1));
      }
   }

   private IEnumerator KillPlayer(int damageAmount, float rate)
   {
      while (playerHealth.health > 0)
      {
         playerHealth.DealDamage(damageAmount);
         yield return new WaitForSeconds(rate);
      }
   }

   public float GetPercentage()
   {
      return Mathf.Min(TimeLimit / maxTimeLimit, 1f);
   }

   public void OnKill()
   {
      GainOxygen(onKill);
   }

   public void GainOxygen(float percentage)
   {
      float toGet = maxTimeLimit * percentage;
      TimeLimit += toGet;

      if (percentage > 0)
      {
         FindObjectOfType<UIManager>().GetComponent<Animator>().SetTrigger("playerOxygen");
      }
   }
}
