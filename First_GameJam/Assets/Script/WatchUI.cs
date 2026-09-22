using UnityEngine;
using TMPro;

public class WatchUI : MonoBehaviour
{
    [Header("Watch UI")]
    public GameObject watchPanel;
    public TMP_Text currentTimeText;

    [Header("Player Time")]
    public float pauseDuration = 5f;

    private bool pauseMode = false;
    private float pauseTimer;

    private void Start()
    {
        watchPanel.SetActive(false);
    }

    private void Update()
    {
        // Update watch time
        if (watchPanel.activeSelf)
        {
            UpdateTimeText();
        }

        // Player-only time
        if (pauseMode)
        {
            pauseTimer -= Time.deltaTime;

            if (pauseTimer <= 0f)
            {
                EndTimerPause();
            }
        }
    }

    public void OpenWatch()
    {
        // Freeze everything
        Time.timeScale = 0f;

        watchPanel.SetActive(true);

        UpdateTimeText();
    }

    public void CloseWatch()
    {
        watchPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    void UpdateTimeText()
    {
        float time = FindObjectOfType<GameTimer>().GetGameTime();

        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        currentTimeText.text =
            string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartTimerPause()
    {
        // Close watch
        watchPanel.SetActive(false);

        // Start 5 second player time
        pauseMode = true;
        pauseTimer = pauseDuration;

        // Resume Unity time
        Time.timeScale = 1f;

        Debug.Log("PLAYER TIME STARTED");
    }

    void EndTimerPause()
    {
        pauseMode = false;

        Debug.Log("PLAYER TIME ENDED");
    }
}