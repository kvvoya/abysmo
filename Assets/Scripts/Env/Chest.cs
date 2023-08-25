using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
   [SerializeField] bool isXpBox;
   [SerializeField] Sprite open;
   [SerializeField] Sprite xp;
   [SerializeField] int giveHealth = 15;
   [SerializeField] float giveOxygen = 0.15f;

   public SpriteRenderer spriteRenderer;

   // Start is called before the first frame update
   void Start()
   {
      if (isXpBox)
      {
         spriteRenderer.sprite = xp;
      }
   }

   // Update is called once per frame
   void Update()
   {

   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.TryGetComponent<Knife>(out Knife knife))
      {
         Reward();
      }
   }

   private void Reward()
   {
      if (!isXpBox)
      {
         spriteRenderer.sprite = open;
         GetComponent<Collider2D>().enabled = false;

         int rewardType = Random.Range(0, 2);
         if (rewardType == 0)
         {
            FindObjectOfType<Player>().GetComponent<Health>().HealHP(giveHealth);
         }
         else if (rewardType == 1)
         {
            FindObjectOfType<OxygenManager>().GainOxygen(giveOxygen);
         }
      }
      else
      {
         FindObjectOfType<XPManager>().GainXP(5);
         Destroy(gameObject);
      }
   }
}
