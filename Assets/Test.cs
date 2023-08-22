using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
   private void Start()
   {
      Debug.Log(GetComponentInChildren<Collider2D>());
   }

   private void Update()
   {

   }
}
