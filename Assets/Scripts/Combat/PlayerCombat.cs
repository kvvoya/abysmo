using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
   [SerializeField] GameObject knife;
   [SerializeField] Transform weaponParent;
   [SerializeField] SpriteRenderer parentRenderer;
   [SerializeField] float cooldown;
   [SerializeField] float stayTime = 0.5f;

   float timeSinceLastAttacked = Mathf.Infinity;
   // bool isAttacking = false;

   Camera mainCamera;

   private void Start()
   {
      mainCamera = Camera.main;
   }

   private void Update()
   {
      Vector3 cursorPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
      cursorPosition.z = 0f;

      Vector3 direction = cursorPosition - transform.position;
      float angle = Vector2.SignedAngle(Vector2.right, direction);

      if (!knife.gameObject.activeInHierarchy)
      {
         weaponParent.transform.rotation = Quaternion.Euler(0f, 0f, angle);
      }

      if (Input.GetMouseButtonDown(0))
      {
         HandleAttackButton();
      }

      timeSinceLastAttacked += Time.deltaTime;
   }

   private void HandleAttackButton()
   {
      if (timeSinceLastAttacked > cooldown)
      {
         knife.SetActive(true);

         // transform.localScale = new Vector3(parentRenderer.flipX ? -1 : 1, 1, 1);

         timeSinceLastAttacked = 0f;
         StartCoroutine(ActivateKnife());
      }
   }

   private IEnumerator ActivateKnife()
   {
      yield return new WaitForSeconds(stayTime);
      knife.SetActive(false);
   }
}
