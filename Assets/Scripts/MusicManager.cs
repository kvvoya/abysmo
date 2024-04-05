using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MusicManager : MonoBehaviour
{
    public AudioClip song1;
    public AudioClip song2;
    public AudioClip song3;

    public int pressure2;
    public int pressure3;

    public float vol = 0.4f;

    private PressureManager pressureManager;
    private AudioSource audioSource;

    private bool playedSong2 = false;
    private bool fadeOut2 = false;
    private bool fadeOut3 = false;
    private bool playedSong3 = false;

    private void Start()
    {
        pressureManager = FindObjectOfType<PressureManager>();
        audioSource = GetComponent<AudioSource>();

        audioSource.clip = song1;
        audioSource.Play();
    }

    private void Update()
    {
        if (pressureManager.pressure > pressure2 - 20 && !fadeOut2)
        {
            StartCoroutine(FadeOut(0.1f));
            fadeOut2 = true;
        }

        if (pressureManager.pressure > pressure2 && !playedSong2)
        {
            audioSource.clip = song2;
            audioSource.Play();
            audioSource.volume = vol;
            playedSong2 = true;
        }

        if (pressureManager.pressure > pressure3 - 20 && !fadeOut3)
        {
            StartCoroutine(FadeOut(0.1f));
            fadeOut3 = true;
        }

        if (pressureManager.pressure > pressure3 && !playedSong3)
        {
            audioSource.clip = song3;
            audioSource.Play();
            audioSource.volume = vol;
            playedSong3 = true;
        }
    }

    public IEnumerator FadeOut(float speed)
    {
        while (audioSource.volume > 0)
        {
            audioSource.volume -= speed * Time.deltaTime;
            yield return null;
        }
    }

    public void SetMusicVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
