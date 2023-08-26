using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoBack : MonoBehaviour
{
   // Start is called before the first frame update
   void Start()
   {
      Invoke("GoBackScene", 15f);
   }

   // Update is called once per frame
   void Update()
   {

   }

   private void GoBackScene()
   {
      SceneManager.LoadScene("MainMenu");
   }
}
