using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
   [SerializeField] TextMeshProUGUI atmText;

   PressureManager pressureManager;

   private void Start()
   {
      pressureManager = FindObjectOfType<PressureManager>();
   }

   private void Update()
   {
      atmText.text = $"{pressureManager.pressure} atm";
   }
}
