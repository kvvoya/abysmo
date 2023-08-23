using UnityEngine;

public class PressureManager : MonoBehaviour
{
   [SerializeField] float pressureFactor = 1f;

   public float pressure { get; private set; } // measured in atm

   Transform playerTransform;

   private void Start()
   {
      playerTransform = FindObjectOfType<DiverMovement>().gameObject.transform;
   }

   private void Update()
   {
      pressure = Mathf.Floor(-playerTransform.position.y * pressureFactor);
   }
}
