using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
   [SerializeField] float moveSpeed;

   Rigidbody2D rb;

   private void Start()
   {
      rb = GetComponent<Rigidbody2D>();
   }

   private void Update()
   {

   }
}
