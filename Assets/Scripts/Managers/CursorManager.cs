using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
   [SerializeField] Texture2D defaultCursor;
   [SerializeField] Texture2D crosshairCursor;

   private void Start()
   {

   }

   private void Update()
   {

   }

   public void SetCursorType(bool isDefault)
   {
      if (isDefault)
      {
         Cursor.SetCursor(defaultCursor, new Vector2(0, 0), CursorMode.Auto);
      }
      else
      {
         Cursor.SetCursor(crosshairCursor, new Vector2(crosshairCursor.width / 2, crosshairCursor.height / 2), CursorMode.Auto);
      }
   }
}
