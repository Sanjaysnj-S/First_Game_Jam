using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Health")]
    public int health = 100;

    public float speed = 5f;
    [Header("Points")]
    public Transform[] points;

    private int currentPoint = 0;
    private Rigidbody2D rb;

    private SpriteRenderer spriteRenderer;
    private Collider2D enemyCollider;

    // Enemy alive/dead state
    public bool IsDead { get; private set; } = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        // Stop enemy during Player Time
        if (TimePauseManager.Instance != null &&
            TimePauseManager.Instance.isPlayerTimeActive)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // Don't move if dead
        if (IsDead)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        PointToPoint();
        MoveEnemy();
    }

  
    void PointToPoint()
    {
        transform.position = Vector2.MoveTowards(transform.position,points[currentPoint].position,speed*Time.deltaTime);

        if (Vector2.Distance(transform.position, points[currentPoint].position) < 0.1f)
        {
            currentPoint++;

        // If last point reached, go back to first point
            if (currentPoint >= points.Length)
            {
                currentPoint = 0;
            }
        }
        
        if (points[currentPoint].position.x > transform.position.x)
        {
            
            transform.localScale = new Vector3(1,1,1);
        }
        else
        {
            transform.localScale = new Vector3(-1,1,1);
        }
    }
    public void TakeDamage(int damage)
    {
        if (IsDead)
            return;

        health -= damage;

        Debug.Log("Enemy Health: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

  
    private void Die()
    {
        IsDead = true;

        rb.linearVelocity = Vector2.zero;

        // Hide enemy
        spriteRenderer.enabled = false;

        // Disable collision
        enemyCollider.enabled = false;

        Debug.Log("ENEMY DIED");
    }

  
    public void Revive(Vector3 position, int savedHealth)
    {
        transform.position = position;

        health = savedHealth;

        IsDead = false;

        // Show enemy
        spriteRenderer.enabled = true;

        // Enable collision
        enemyCollider.enabled = true;

        rb.linearVelocity = Vector2.zero;

        Debug.Log(
            "ENEMY REVIVED | Health: " + health
        );
    }

    public void MoveEnemy()
    {
        rb.linearVelocity =
            new Vector2(speed, rb.linearVelocity.y);
    }
}