using UnityEngine;

public class Knife : MonoBehaviour
{
   [SerializeField] int damage;
   [SerializeField] Transform weaponParent;
   [SerializeField] float knockback;

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.TryGetComponent<Health>(out Health targetHealth))
      {
         Debug.Log($"Hit {other.gameObject.name} who has {targetHealth.health}");
         targetHealth.DealDamage(damage);
         targetHealth.ApplyForce(weaponParent.right * knockback);
      }
   }
}
