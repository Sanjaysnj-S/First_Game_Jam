using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public TMP_Text timeText;
    
    private float gameTime = 0f;

  

    private void Update()
    {
        if (TimePauseManager.Instance != null &&
            TimePauseManager.Instance.isPlayerTimeActive)
        {
            return;
        }
        gameTime += Time.deltaTime;
 
        int minutes = Mathf.FloorToInt(gameTime / 60f);
        int seconds = Mathf.FloorToInt(gameTime % 60f);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public float GetGameTime()
    {
        return gameTime;
    }
    
}