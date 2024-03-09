using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpeedrunTimer : MonoBehaviour
{
    public TextMeshProUGUI clockText;

    private float elapsedTime = 0f;
    private bool isRunning = false;

    private float timeToReset = 1.5f;
    private float timeRHeld = 0;

    private void Update()
    {
        if (timeRHeld >= timeToReset)
        {
            SceneManager.LoadScene("SampleScene");
        }

        if (isRunning)
        {
            elapsedTime += Time.deltaTime;

            string minutes = Mathf.Floor(elapsedTime / 60).ToString("00");
            string seconds = (elapsedTime % 60).ToString("00");
            string milliseconds = ((elapsedTime * 1000) % 1000).ToString("000");

            clockText.text = $"{minutes}:{seconds}.{milliseconds}";
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            timeRHeld = 0;
        }
        else if (Input.GetKey(KeyCode.R))
        {
            timeRHeld += Time.deltaTime;
        }

    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }
}
