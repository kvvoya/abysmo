using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
   [SerializeField] int maxHealth;
   [SerializeField] float invincibilityTime;

   [SerializeField] UnityEvent onDeath;
   [SerializeField] UnityEvent onDamage;

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
      if (Input.GetKeyDown(KeyCode.N))
      {
         DealDamage(10);
      }
      CheckIfDead();

      timeSinceGotHurt += Time.deltaTime;
   }

   public void DealDamage(int damage)
   {
      if (timeSinceGotHurt < invincibilityTime) return;

      health -= damage;
      onDamage?.Invoke();

      timeSinceGotHurt = 0f;
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

   private void Die()
   {
      Debug.Log("Dead");
      onDeath?.Invoke();
   }
}
