using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimeRecorder : MonoBehaviour
{
    PlayerController player;
    GameTimer gameTimer;

    public void Start()
    {
        player = GetComponent<PlayerController>();
        gameTimer = FindAnyObjectByType<GameTimer>();
    }
    [System.Serializable]
    public class PlayerState
    {
        public float time;
        public Vector3 position;
        public int health;

        public PlayerState(float time, Vector3 position, int health)
        {
            this.time = time;
            this.position = position;
            this.health = health;
        }
    }

    [Header("Recording")]
    public float recordDuration = 20f;
    public float recordInterval = 0.1f;

    private List<PlayerState> states = new List<PlayerState>();

    private float recordTimer;

    private void Update()
    {
        recordTimer += Time.deltaTime;

        if (recordTimer >= recordInterval)
        {
            RecordState();
            recordTimer = 0f;
        }

        RemoveOldStates();

        if (Keyboard.current.rKey.wasPressedThisFrame)
    {
        Rewind(5f);
    }
    }

    void RecordState()
    {
        PlayerState state = new PlayerState(
            Time.time,
            transform.position,
            player.currentHealth
        );

        states.Add(state);

        Debug.Log(
        "Recording | Time: " + state.time +
        " | Position: " + state.position
    );
    }

    void RemoveOldStates()
    {
        float oldestAllowedTime = Time.time - recordDuration;

        while (states.Count > 0 &&
               states[0].time < oldestAllowedTime)
        {
            states.RemoveAt(0);
        }
    }

    public void Rewind(float seconds)
    {
        if (states.Count == 0)
        {
            Debug.LogWarning("No recorded states!");
            return;
        }

        float currentTime = gameTimer.GetGameTime();

        float targetTime = Time.time - seconds;

        if(targetTime < 0f) targetTime = 0f;

        PlayerState closestState = states[0];

        for (int i = states.Count - 1; i >= 0; i--)
        {
            if (states[i].time <= targetTime)
            {
                closestState = states[i];
                break;
            }
        }

        gameTimer.SetGameTime(targetTime);
        transform.position = closestState.position;
        player.currentHealth = closestState.health;



        Debug.Log("Rewound " + seconds + " seconds");
    }
}