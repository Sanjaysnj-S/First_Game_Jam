using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class WatchUI : MonoBehaviour
{
    [Header("Watch UI")]
    public GameObject watchPanel;
    public TMP_Text currentTimeText;

    [Header("Rewind UI")]
    public GameObject rewindPanel;
    public Slider rewindSlider;
    public TMP_Text rewindCurrentTime;
    public TMP_Text rewindSelectedText;
    public TimeRecorder timeRecorder;



    private GameTimer gameTimer;

    private void Start()
    {
        watchPanel.SetActive(false);
        rewindPanel.SetActive(false);

        rewindSlider.minValue = 0;
        rewindSlider.maxValue = 20;
        rewindSlider.wholeNumbers = true;

        rewindSlider.onValueChanged.AddListener(UpdateRewindTime);

        UpdateRewindTime(rewindSlider.value);

        gameTimer = FindObjectOfType<GameTimer>();
    }

    private void Update()
    {
        OpenWatch();
        // Update the time shown inside the watch
        if (watchPanel.activeSelf)
        {
            UpdateTimeText();
        }
        if (rewindPanel.activeSelf)
        {
            UpdateRewindCurrentTime();
        }
    }

    
    public void OpenWatch()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            Time.timeScale = 0f;

            watchPanel.SetActive(true);

            UpdateTimeText();
        }
        // Freeze the entire game
        

        // Debug.Log("WATCH OPENED - GAME FROZEN");
    }


    public void CloseWatch()
    {
        watchPanel.SetActive(false);

        // Resume the game
        Time.timeScale = 1f;

        Debug.Log("WATCH CLOSED - GAME RESUMED");
    }


    public void StartTimerPause()
    {
        Debug.Log("TIME PAUSE BUTTON CLICKED");

        // Close watch UI
        watchPanel.SetActive(false);

        // Resume Unity time
        // Player can move now
        Time.timeScale = 1f;

        // Tell TimePauseManager to start
        // player-only time
        if (TimePauseManager.Instance != null)
        {
            TimePauseManager.Instance.StartPlayerTime();

            Debug.Log("PLAYER TIME STARTED");
        }
        else
        {
            Debug.LogError("TimePauseManager Instance is NULL!");
        }
    }


    void UpdateTimeText()
    {
        if (gameTimer == null)
            return;

        float time = gameTimer.GetGameTime();

        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        currentTimeText.text =
            string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    //REWIND
    public void OpenRewind()
    {
        
        watchPanel.SetActive(false);
        rewindPanel.SetActive(true);

        rewindSlider.value = 0f;

        UpdateRewindCurrentTime();
        UpdateRewindTime(0f);
    }

    void UpdateRewindTime(float value)
    {
        int seconds = Mathf.RoundToInt(value);

        rewindSelectedText.text = "Rewind:"+seconds+"sec";
    }

    
    void UpdateRewindCurrentTime()
    {
        if (gameTimer == null || rewindCurrentTime == null)
            return;

        float time = gameTimer.GetGameTime();

        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        rewindCurrentTime.text =
            string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void RewindButton()
    {
        int seconds = Mathf.RoundToInt(rewindSlider.value);

        if (seconds <= 0)
        {
            // Debug.Log("Select a rewind time first!");
            return;
        }
        float currentTime = gameTimer.GetGameTime();
        float targetTime = Mathf.Max(0f, currentTime - seconds);

        timeRecorder.Rewind(seconds);

        
        EnemyTimeRecorder[] enemies = FindObjectsByType<EnemyTimeRecorder>(FindObjectsSortMode.None);
        foreach (EnemyTimeRecorder enemy in enemies)
        {
            enemy.Rewind(seconds);
        }

        rewindPanel.SetActive(false);
        gameTimer.SetGameTime(targetTime);

        Time.timeScale = 1f;

        // Debug.Log("Rewound " + seconds + " seconds");
    }
}