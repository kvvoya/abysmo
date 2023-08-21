using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenManager : MonoBehaviour
{
   [Tooltip("In seconds")]
   [SerializeField] float maxTimeLimit = 600;

   public float TimeLimit { get; private set; }

   private void Start()
   {
      TimeLimit = maxTimeLimit;
   }

   private void Update()
   {
      TimeLimit -= Time.deltaTime;
      CheckIfTimeIsOut();
   }

   private void CheckIfTimeIsOut()
   {
      if (TimeLimit <= 0)
      {
         Debug.Log("Time out!");
      }
   }

   public float GetPercentage()
   {
      return TimeLimit / maxTimeLimit;
   }
}
