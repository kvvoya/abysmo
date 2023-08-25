using UnityEngine;

public class Knife : MonoBehaviour
{
   [SerializeField] int damage;
   [SerializeField] Transform weaponParent;
   [SerializeField] float knockback;

   public int GetDamage()
   {
      return damage;
   }

   public float damageFactor = 1f;

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.TryGetComponent<Health>(out Health targetHealth))
      {
         targetHealth.DealDamage((int)Mathf.Floor(damage * damageFactor));
         targetHealth.ApplyForce(weaponParent.right * knockback);
      }
   }
}
