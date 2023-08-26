using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class UpgradeSlot : MonoBehaviour, IPointerExitHandler, IPointerClickHandler, IPointerEnterHandler
{
   public PriceTag myPriceTag;
   public Upgrade myUpgrade;

   XPManager xPManager;

   [SerializeField] Image image;
   [SerializeField] TextMeshProUGUI nameText;
   [SerializeField] TextMeshProUGUI infoboxText;
   [SerializeField] AudioClip buySound;
   [SerializeField] AudioClip errorSound;

   bool isAvailable = false;
   bool isHovered = false;

   private void Start()
   {
      image = GetComponent<Image>();
      xPManager = FindObjectOfType<XPManager>();
   }

   private void Update()
   {
      if (isHovered)
      {
         infoboxText.text = myUpgrade.description;
      }
   }

   private void OnDisable()
   {
      isHovered = false;
   }

   public void RefreshInfo()
   {
      if (myUpgrade == null)
      {
         gameObject.SetActive(false);
         infoboxText.text = "";
         return;
      }

      image.sprite = myUpgrade.sprite;
      nameText.text = myUpgrade.name;

      if (XPManager.collectedXP >= (int)myUpgrade.priceTag)
      {
         isAvailable = true;
         image.color = !isHovered ? new Color(1f, 1f, 1f, 0.75f) : new Color(1f, 1f, 1f, 1f);
      }
      else
      {
         isAvailable = false;
         image.color = new Color(1f, 1f, 1f, 0.3f);
      }
   }

   public void OnPointerEnter(PointerEventData eventData)
   {
      if (isAvailable)
      {
         image.color = new Color(1f, 1f, 1f, 1f);
      }
      else
      {
         image.color = new Color(1f, 1f, 1f, 0.3f);
      }

      infoboxText.text = myUpgrade.description;
      isHovered = true;
   }

   public void OnPointerExit(PointerEventData eventData)
   {
      if (isAvailable)
      {
         image.color = new Color(1f, 1f, 1f, 0.75f);
      }
      else
      {
         image.color = new Color(1f, 1f, 1f, 0.3f);
      }

      infoboxText.text = "";
      isHovered = false;
   }

   public void OnPointerClick(PointerEventData eventData)
   {
      if (isAvailable)
      {
         xPManager.PurchaseUpgrade(myUpgrade);
         GetComponentInParent<AudioSource>().PlayOneShot(buySound);
      }
      else
      {
         GetComponentInParent<AudioSource>().PlayOneShot(errorSound);
      }
   }
}
