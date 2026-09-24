using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public TMP_Text timeText;

    private float gameTime = 0f;

    private void Update()
    {
        // Stop game timer during Player Time
        if (TimePauseManager.Instance != null &&
            TimePauseManager.Instance.isPlayerTimeActive)
        {
            return;
        }

        // Normal game time
        gameTime += Time.deltaTime;

        UpdateTimeText();
    }

    private void UpdateTimeText()
    {
        int minutes = Mathf.FloorToInt(gameTime / 60f);
        int seconds = Mathf.FloorToInt(gameTime % 60f);

        timeText.text = string.Format(
            "{0:00}:{1:00}",
            minutes,
            seconds
        );
    }

    public float GetGameTime()
    {
        return gameTime;
    }

    // Used by the rewind system
    public void SetGameTime(float newTime)
    {
        gameTime = Mathf.Max(0f, newTime);

        UpdateTimeText();
    }
}