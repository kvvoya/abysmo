using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sex : MonoBehaviour
{
    [SerializeField] AudioClip sexSound;
    [SerializeField] AudioSource audioSource;

    int sexCount = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) && sexCount == 0)
        {
            sexCount++;
        }
        else if (Input.GetKeyDown(KeyCode.E) && sexCount == 1)
        {
            sexCount++;
        }
        else if (Input.GetKeyDown(KeyCode.X) && sexCount == 2)
        {
            StartSex();
            sexCount = 0;
        }
    }

    private void StartSex()
    {
        audioSource.Play();
    }
}
