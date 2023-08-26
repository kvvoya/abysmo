using UnityEngine;

public class Obstacle : MonoBehaviour
{
   [SerializeField] int height;
   [SerializeField] int destroyAfter = 100;

   public int Height
   {
      get
      {
         return height;
      }

      set
      {
         height = value;
      }
   }

   Transform playerTransform;

   private void Start()
   {
      playerTransform = FindObjectOfType<Player>().transform;
   }

   private void Update()
   {
      if (Vector3.Distance(transform.position, playerTransform.position) > destroyAfter && playerTransform.position.y < transform.position.y)
      {
         Destroy(gameObject);
      }
   }
}
