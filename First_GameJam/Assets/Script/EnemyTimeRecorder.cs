using System.Collections.Generic;
using UnityEngine;

public class EnemyTimeRecorder : MonoBehaviour
{
    [System.Serializable]
    public class EnemyState
    {
        public float time;
        public Vector3 position;
        public int health;
        public bool isAlive;

        public EnemyState(
            float time,
            Vector3 position,
            int health,
            bool isAlive)
        {
            this.time = time;
            this.position = position;
            this.health = health;
            this.isAlive = isAlive;
        }
    }

    [Header("Recorder")]
    public float recordDuration = 20f;
    public float recordInterval = 0.1f;

    private List<EnemyState> states =
        new List<EnemyState>();

    private float recordTimer;

    private GameTimer gameTimer;
    private Enemy enemy;
    private Rigidbody2D rb;

    // =========================
    // START
    // =========================

    private void Start()
    {
        gameTimer = FindAnyObjectByType<GameTimer>();

        enemy = GetComponent<Enemy>();

        rb = GetComponent<Rigidbody2D>();

        if (gameTimer == null)
        {
            Debug.LogError("GameTimer not found!");
            return;
        }

        if (enemy == null)
        {
            Debug.LogError("Enemy component not found!");
            return;
        }

        // Record initial state
        RecordState();
    }

    // =========================
    // UPDATE
    // =========================

    private void Update()
    {
        if (gameTimer == null ||
            enemy == null)
        {
            return;
        }

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

    // =========================
    // RECORD STATE
    // =========================

    private void RecordState()
    {
        EnemyState state = new EnemyState(
            gameTimer.GetGameTime(),
            transform.position,
            enemy.health,
            !enemy.IsDead
        );

        states.Add(state);
    }

    // =========================
    // REMOVE OLD STATES
    // =========================

    private void RemoveOldStates()
    {
        float currentTime =
            gameTimer.GetGameTime();

        float oldestAllowedTime =
            currentTime - recordDuration;

        while (states.Count > 0 &&
               states[0].time < oldestAllowedTime)
        {
            states.RemoveAt(0);
        }
    }

    // =========================
    // REWIND
    // =========================

    public void Rewind(float seconds)
    {
        if (states.Count == 0)
        {
            Debug.LogWarning(
                gameObject.name +
                " has no recorded history!"
            );

            return;
        }

        float currentTime =
            gameTimer.GetGameTime();

        float targetTime =
            currentTime - seconds;

        if (targetTime < 0f)
        {
            targetTime = 0f;
        }

        EnemyState closestState =
            states[0];

        // Find closest state before target time
        for (int i = states.Count - 1; i >= 0; i--)
        {
            if (states[i].time <= targetTime)
            {
                closestState = states[i];
                break;
            }
        }

        // =========================
        // RESTORE ENEMY
        // =========================

        if (closestState.isAlive)
        {
            // Enemy was alive at rewind time
            enemy.Revive(
                closestState.position,
                closestState.health
            );
        }
        else
        {
            // Enemy was already dead at rewind time
            transform.position =
                closestState.position;

            enemy.health =
                closestState.health;

            rb.linearVelocity =
                Vector2.zero;
        }

        Debug.Log(
            "ENEMY REWIND | " +
            "Current: " + currentTime +
            " | Target: " + targetTime +
            " | State: " + closestState.time +
            " | Health: " + closestState.health +
            " | Alive: " + closestState.isAlive
        );
    }
}