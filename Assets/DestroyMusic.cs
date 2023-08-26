using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMusic : MonoBehaviour
{
   private void Start()
   {
      Destroy(FindObjectOfType<MusicManager>().gameObject);
   }
}
