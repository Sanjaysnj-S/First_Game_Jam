using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyTimeRecorder : MonoBehaviour
{
    [System.Serializable]
    public class EnemyState
    {
        public float time;
        public Vector3 position;
        public int health;

        public EnemyState(float time, Vector3 position, int health)
        {
            this.time = time;
            this.position = position;
            this.health = health;
        }
    }

    [Header("Recorder")]
    public float recordDuration = 20f;
    public float recordInterval = 0.1f;

    private List<EnemyState> states = new List<EnemyState>();

    private float recordTimer;

    private GameTimer gameTimer;
    private Enemy enemy;
    private Rigidbody2D rb;

    private void Start()
    {
        gameTimer = FindAnyObjectByType<GameTimer>();
        enemy = GetComponent<Enemy>();
        rb = GetComponent<Rigidbody2D>();

        RecordState();
    }

    private void Update()
    {
        if (gameTimer == null || enemy == null)
            return;

        // Don't record during Player Time
        if (TimePauseManager.Instance != null &&
            TimePauseManager.Instance.isPlayerTimeActive)
        {
            return;
        }

        recordTimer += Time.deltaTime;

        if (recordTimer >= recordInterval)
        {
            RecordState();
            recordTimer = 0f;
        }
       

        RemoveOldStates();
    }

    private void RecordState()
    {
        states.Add(
            new EnemyState(
                gameTimer.GetGameTime(),
                transform.position,
                enemy.health
            )
        );
    }

    private void RemoveOldStates()
    {
        float oldestAllowedTime =
            gameTimer.GetGameTime() - recordDuration;

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
            Debug.LogWarning(
                gameObject.name + " has no recorded history!"
            );

            return;
        }

        float currentTime = gameTimer.GetGameTime();

        float targetTime = currentTime - seconds;

        if (targetTime < 0f)
            targetTime = 0f;

        EnemyState closestState = states[0];

        for (int i = states.Count - 1; i >= 0; i--)
        {
            if (states[i].time <= targetTime)
            {
                closestState = states[i];
                break;
            }
        }

        // Restore position
        transform.position = closestState.position;

        // Restore health
        enemy.health = closestState.health;

        // Stop current movement
        rb.linearVelocity = Vector2.zero;

        Debug.Log(
            gameObject.name +
            " REWOUND | Time: " +
            closestState.time +
            " | Health: " +
            closestState.health
        );
    }
}