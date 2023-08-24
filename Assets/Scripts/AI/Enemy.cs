using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
   [SerializeField] float speed;
   [SerializeField] int cost;
   [SerializeField] float agroRange = 10f;
   [SerializeField] float dontCareRange = 30f;
   [SerializeField] int contactDamage;
   [SerializeField] float physicsForce;
   [Space(10)]

   [Header("nevermind these")]
   [SerializeField] float nextWaypointDistance = 3f;
   [SerializeField] new SpriteRenderer renderer;
   [SerializeField] ParticleSystem explosionParticles;

   private float distance;

   Path path;
   int currentWaypoint = 0;
   bool followPlayer = false;

   Seeker seeker;
   Rigidbody2D rb;
   Player player;

   private void Start()
   {
      player = FindObjectOfType<Player>();
      seeker = GetComponent<Seeker>();
      rb = GetComponent<Rigidbody2D>();

      InvokeRepeating("UpdatePath", 0f, .25f);

   }

   private void UpdatePath()
   {
      if (seeker.IsDone())
      {
         seeker.StartPath(rb.position, player.transform.position, OnPathComplete);
      }
   }

   private void OnPathComplete(Path p)
   {
      if (!p.error)
      {
         path = p;
         currentWaypoint = 0;
      }
   }

   private void Update()
   {
      distance = Vector2.Distance(transform.position, player.transform.position);

      if (followPlayer)
      {
         RotateTowardsPlayer();
         FlipSprite();
      }

      if (distance < agroRange)
      {
         followPlayer = true;
      }
      else if (distance > dontCareRange)
      {
         followPlayer = false;
      }
   }

   private void OnDrawGizmos()
   {
      Gizmos.color = Color.red;
      Gizmos.DrawWireSphere(transform.position, agroRange);
   }

   private void RotateTowardsPlayer()
   {
      Vector2 direction = player.transform.position - transform.position;
      direction.Normalize();

      float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
      transform.rotation = Quaternion.Euler(Vector3.forward * angle);
   }

   private void FixedUpdate()
   {
      if (path != null && followPlayer)
      {
         if (currentWaypoint >= path.vectorPath.Count)
         {
            return;
         }

         Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
         Vector2 force = direction * speed * Time.deltaTime;

         rb.AddForce(force);


         float waypointDistance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);


         if (waypointDistance < nextWaypointDistance)
         {
            currentWaypoint++;
         }
      }

   }

   private void FlipSprite()
   {
      if (player.transform.position.x > transform.position.x)
      {
         renderer.flipY = false;
      }
      else if (player.transform.position.x < transform.position.x)
      {
         renderer.flipY = true;
      }
   }

   public void Die()
   {
      if (explosionParticles != null)
      {
         ParticleSystem particles = Instantiate(explosionParticles, transform.position, Quaternion.identity);
         particles.Play();
         Destroy(particles.gameObject, particles.main.duration * 2f);
      }

      FindObjectOfType<XPManager>().GainXP(cost);
      Destroy(gameObject);
   }

   private void OnCollisionStay2D(Collision2D other)
   {
      if (other.gameObject.TryGetComponent<Player>(out Player player))
      {
         Health playerHealth = player.GetComponent<Health>();
         playerHealth.DealDamage(contactDamage);


         playerHealth.ApplyForce(transform.right.normalized * physicsForce);
      }
   }

   private float GetDirectionToPlayer(Player player)
   {
      Vector2 direction = player.transform.position - transform.position;
      direction.Normalize();

      float angle = Mathf.Atan2(direction.y, direction.x);
      return angle;
   }
}
