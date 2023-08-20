using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureManager : MonoBehaviour
{
   public float pressure { get; private set; } // measured in atm

   Transform playerTransform;

   private void Start()
   {
      pressure = 1f;
      playerTransform = FindObjectOfType<PlayerMovement>().gameObject.transform;
   }

   private void Update()
   {

   }
}
