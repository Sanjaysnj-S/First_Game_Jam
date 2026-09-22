using UnityEngine;

public class TimePauseManager : MonoBehaviour
{
    public static TimePauseManager Instance;

    [Header("Pause Settings")]
    public float playerMoveDuration = 5f;

    public bool isPlayerTimeActive = false;

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

        if (timer <= 0f)
        {
            EndPlayerTime();
        }
    }

    public void StartPlayerTime()
    {
        isPlayerTimeActive = true;
        timer = playerMoveDuration;

        Debug.Log("PLAYER TIME STARTED");
    }

    private void EndPlayerTime()
    {
        isPlayerTimeActive = false;

        Debug.Log("PLAYER TIME ENDED");
    }

    public float GetRemainingTime()
    {
        return timer;
    }
}