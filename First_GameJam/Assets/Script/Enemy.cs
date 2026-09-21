using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Health")]
    public int health = 100;

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
}