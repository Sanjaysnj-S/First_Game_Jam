using UnityEngine;
using TMPro;

public class TimePauseManager : MonoBehaviour
{
    public static TimePauseManager Instance;

    [Header("Pause Settings")]
    public float playerMoveDuration = 5f;

    public bool isPlayerTimeActive = false;

    [Header("PauseText UI")]
    public TMP_Text pausetime;

    private float timer;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!isPlayerTimeActive)
            return;

        timer -= Time.unscaledDeltaTime;

        if(pausetime != null)
        {
            pausetime.text = Mathf.CeilToInt(timer).ToString();
        }

        if (timer <= 0f)
        {
            EndPlayerTime();
        }
    }

    public void StartPlayerTime()
    {
        isPlayerTimeActive = true;
        timer = playerMoveDuration;

        if(pausetime != null)
        {
            pausetime.gameObject.SetActive(true);
            pausetime.text = Mathf.CeilToInt(timer).ToString();
        }
        Debug.Log("PLAYER TIME STARTED");
    }

    private void EndPlayerTime()
    {
        isPlayerTimeActive = false;

        if(pausetime != null)
        {
            pausetime.gameObject.SetActive(false);
        }

        Debug.Log("PLAYER TIME ENDED");
    }

    public float GetRemainingTime()
    {
        return timer;
    }
}