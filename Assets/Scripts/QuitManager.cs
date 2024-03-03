using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuitManager : MonoBehaviour
{
    [SerializeField] private float holdToEscape = 2f;
    [SerializeField] TextMeshProUGUI quittingText;

    private float held = 0;

    Color newColor;

    // Start is called before the first frame update
    void Start()
    {
        newColor = quittingText.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            held += Time.deltaTime;
        }
        else
        {
            held = 0;
        }

        newColor.a = Mathf.Clamp(held, 0, 0.75f);
        quittingText.color = newColor;

        if (held >= holdToEscape)
        {
            Application.Quit();
            Debug.Log("Application.Quit()");
        }
    }
}
