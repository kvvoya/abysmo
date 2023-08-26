using UnityEngine;
using Pathfinding;
using System.Collections;

public enum EnemyType
{
   Contact,
   SwordFish,
   Flarefish
}

public class Enemy : MonoBehaviour
{
   [SerializeField] EnemyType myType = EnemyType.Contact;
   [SerializeField] float speed;
   [SerializeField] int cost;
   [SerializeField] float agroRange = 10f;
   [SerializeField] float dontCareRange = 30f;
   [SerializeField] int contactDamage;
   [SerializeField] float physicsForce;
   [Space(10)]

   [Header("nevermind these")]
   [SerializeField] float minSwordFishDashInterval;
   [SerializeField] float maxSwordFishDashInterval;
   [SerializeField] float maxLightAngle;
   [SerializeField] float weakSpeed;
   [SerializeField] int weakDamage;
   [SerializeField] float strongSpeed;
   [SerializeField] int strongDamage;
   [SerializeField] float nextWaypointDistance = 3f;
   [SerializeField] new SpriteRenderer renderer;
   [SerializeField] ParticleSystem explosionParticles;

   private float distance;

   Path path;
   int currentWaypoint = 0;
   bool followPlayer = false;

   private bool showedTheAngry = false;

   Seeker seeker;
   Rigidbody2D rb;
   Player player;
   Animator animator;
   Camera mainCamera;
   Transform playerLight;

   bool coroutineRunning = false;

   private void Start()
   {
      player = FindObjectOfType<Player>();
      seeker = GetComponent<Seeker>();
      rb = GetComponent<Rigidbody2D>();
      animator = GetComponent<Animator>();
      mainCamera = Camera.main;

      playerLight = GameObject.FindGameObjectWithTag("PlayerLight").transform;

      InvokeRepeating("UpdatePath", 0f, .1f);

      if (UpgradeFunction.Instance.isNinjaDiven)
      {
         agroRange *= 0.75f;
      }
   }

   private void UpdatePath()
   {
      if (seeker.IsDone() && followPlayer)
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
         // Debug.Log("FOLLOW PLAYER");
         followPlayer = true;

         if (myType == EnemyType.SwordFish && !coroutineRunning)
         {
            StartCoroutine(SwordFishDash());
         }
         else if (myType == EnemyType.Flarefish)
         {
            Vector2 playerToEnemy = transform.position - playerLight.position;
            playerToEnemy.Normalize();

            float angle = Vector2.SignedAngle(playerLight.right, playerToEnemy) - 90f;

            FlarefishWeak(angle);
         }

         if (!showedTheAngry)
         {
            animator.SetTrigger("angee");
            showedTheAngry = true;

         }
      }
      else if (distance > dontCareRange)
      {
         followPlayer = false;
         showedTheAngry = false;
      }
   }

   private void FlarefishWeak(float angle)
   {
      if (Mathf.Abs(angle) <= maxLightAngle)
      {
         speed = weakSpeed;
         contactDamage = weakDamage;
         Debug.Log("weak");
      }
      else
      {
         speed = strongSpeed;
         contactDamage = strongDamage;
         Debug.Log("strong");
      }
   }

   private void OnDrawGizmos()
   {
      Gizmos.color = Color.red;
      Gizmos.DrawWireSphere(transform.position, agroRange);

      // if (path != null)
      // {
      //    Gizmos.DrawSphere((Vector2)path.vectorPath[currentWaypoint], 0.2f);
      // }

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

         if (myType != EnemyType.SwordFish)
         {
            rb.AddForce(force);
         }


         float waypointDistance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);


         if (waypointDistance < nextWaypointDistance)
         {
            currentWaypoint++;
         }
      }

   }

   private IEnumerator SwordFishDash()
   {
      coroutineRunning = true;
      while (followPlayer)
      {
         yield return new WaitForSeconds(Random.Range(minSwordFishDashInterval, maxSwordFishDashInterval));

         Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
         Vector2 force = direction * speed;

         Debug.Log(direction);

         rb.AddForce(force, ForceMode2D.Impulse);
      }
      coroutineRunning = false;
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
         if (UpgradeFunction.Instance.isPayback && !playerHealth.IsInvincible())
         {
            GetComponent<Health>().DealDamage(contactDamage / 2);
         }
         playerHealth.DealDamage(contactDamage * (UpgradeFunction.Instance.isIronWill ? 2 : 1));

         Debug.Log(transform.right.normalized * physicsForce * (UpgradeFunction.Instance.isPayback ? 0.5f : 1f));
         playerHealth.ApplyForce(transform.right.normalized * physicsForce * (UpgradeFunction.Instance.isPayback ? 0.5f : 1f));

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
