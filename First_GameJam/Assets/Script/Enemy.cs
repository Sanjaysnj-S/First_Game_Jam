using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Health")]
    public int health = 100;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (TimePauseManager.Instance != null &&
            TimePauseManager.Instance.isPlayerTimeActive)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        MoveEnemy();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        Debug.Log("Enemy Health: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    public void MoveEnemy()
    {
        rb.linearVelocity =
            new Vector2(-0.5f, rb.linearVelocity.y);
    }
}