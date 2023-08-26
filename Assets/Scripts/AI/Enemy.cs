using UnityEngine;
using Pathfinding;
using System.Collections;
using UnityEngine.Rendering.Universal;

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
   [SerializeField] AudioClip dieSound;

   private float distance;

   Path path;
   int currentWaypoint = 0;
   bool followPlayer = false;

   private bool showedTheAngry = false;

   Seeker seeker;
   Rigidbody2D rb;
   Player player;
   Animator animator;
   Transform playerLight;
   Light2D lightLevel;

   bool coroutineRunning = false;

   private void Start()
   {
      player = FindObjectOfType<Player>();
      seeker = GetComponent<Seeker>();
      rb = GetComponent<Rigidbody2D>();
      animator = GetComponent<Animator>();

      playerLight = GameObject.FindGameObjectWithTag("PlayerLight").transform;
      lightLevel = playerLight.GetComponent<Light2D>();

      InvokeRepeating("UpdatePath", 0f, .1f);

      if (UpgradeFunction.Instance.isNinjaDiven)
      {
         agroRange *= 0.60f;
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
      if (Mathf.Abs(angle) <= maxLightAngle && lightLevel.intensity > 0.5f)
      {
         speed = weakSpeed;
         contactDamage = weakDamage;
         animator.SetBool("weak", true);
      }
      else
      {
         speed = strongSpeed;
         contactDamage = strongDamage;
         animator.SetBool("weak", false);
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
         if (path != null)
         {
            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = direction * speed;


            rb.AddForce(force, ForceMode2D.Impulse);
            yield return new WaitForSeconds(Random.Range(minSwordFishDashInterval, maxSwordFishDashInterval));
         }
         else
         {
            yield return null;
         }

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
      AudioSource.PlayClipAtPoint(dieSound, Camera.main.transform.position);
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
         playerHealth.DealDamage((int)(contactDamage * (UpgradeFunction.Instance.isIronWill ? 1.25f : 1)));

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
