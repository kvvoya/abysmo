using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
   [SerializeField] int maxHealth;
   [SerializeField] float invincibilityTime;

   [SerializeField] UnityEvent onDeath;
   [SerializeField] UnityEvent onDamage;

   public int MaxHealth
   {
      get
      {
         return maxHealth;
      }
   }

   public int health { get; private set; }

   private float timeSinceGotHurt = Mathf.Infinity;

   Rigidbody2D rb;

   private void Start()
   {
      health = maxHealth;
      rb = GetComponent<Rigidbody2D>();
   }

   private void Update()
   {
      CheckIfDead();

      timeSinceGotHurt += Time.deltaTime;
   }

   public void DealDamage(int damage, bool giveInvincibility = true)
   {
      if (timeSinceGotHurt < invincibilityTime && giveInvincibility) return;

      health -= damage;

      if (giveInvincibility)
      {
         onDamage?.Invoke();
         timeSinceGotHurt = 0f;
      }
   }

   private void CheckIfDead()
   {
      if (health <= 0)
      {
         Die();
      }
   }

   public void ApplyForce(Vector2 direction)
   {
      if (timeSinceGotHurt > invincibilityTime || timeSinceGotHurt == 0f)
      {
         rb.AddForce(direction, ForceMode2D.Impulse);
      }

   }

   public bool IsInvincible()
   {
      Debug.Log("timeSinceGotHurt: " + timeSinceGotHurt);
      return timeSinceGotHurt < invincibilityTime;
   }

   private void Die()
   {
      if (gameObject.CompareTag("Hittable"))
      {
         FindObjectOfType<OxygenManager>().OnKill();
         FindObjectOfType<Player>().OnKill();
      }
      onDeath?.Invoke();
   }

   public void UpgradeMaxHP(int amount)
   {
      maxHealth += amount;
      HealHP(amount);
   }

   public void HealHP(int amount)
   {
      if (amount > 0)
         health += amount;
      health = Mathf.Clamp(health, 0, maxHealth);
   }
}
