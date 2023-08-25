using System.Collections;
using UnityEngine;

public class Bubble : MonoBehaviour
{
   [SerializeField] private Animator animator = null;

   // Start is called before the first frame update
   IEnumerator Start()
   {
      yield return new WaitForSeconds(Random.Range(0f, 2f));
      animator.SetTrigger("offset");
   }

   // Update is called once per frame
   void Update()
   {

   }
}
