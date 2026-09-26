using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.LowLevelPhysics2D.PhysicsShape;

public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    public float speed = 5f;
    float horizontalInput;  

    [Header("Jump")]
    public float jumpForce = 10f;
    private float jumpRemaining = 1;
    public float enemyPush = 15f;
    
    [Header("Gravity")]
    public float baseGravity = 2f;
    public float fallSpeed = 20f;
    public float fallMultiplier = 2.5f;

    [Header("Ground Check")]

    public Transform groundCheckPos;
    public Vector2 groundCheckSize;
    public bool isGrounded;

    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Damage")]
    public int damageTaken = 50;
    [Header("Enemy Damage")]
    public int enemyDamageTaken = 20;

    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
        rb.linearVelocity = new Vector2(horizontalInput * speed, rb.linearVelocityY);
        GroundCheck();
        Gravity();
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalInput = context.ReadValue<Vector2>().x;
    }

    
    public void Jump(InputAction.CallbackContext context)
    {
        if(context.performed && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
        }
        else if (context.canceled)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, rb.linearVelocityY * 0.5f);
        }
    }

    public void GroundCheck()
    {
        isGrounded = Physics2D.OverlapBox(groundCheckPos.position,groundCheckSize,0f,LayerMask.GetMask("Ground"));

    }

    private void Gravity()
    {
        if(rb.linearVelocityY < 0)
        {
            rb.gravityScale = baseGravity*fallSpeed;
            rb.linearVelocity = new Vector2(rb.linearVelocityX,Mathf.Max(rb.linearVelocityY, -fallSpeed));
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

            

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheckPos.position,groundCheckSize);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            foreach(ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.5f)
                {
                    Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                
                    if (enemy != null)
                    {
                        enemy.TakeDamage(enemyDamageTaken);
                    }

                    rb.linearVelocity = new Vector2(rb.linearVelocityX, enemyPush);
                }
                else
                {
                    TakeDamage(damageTaken);
                
                }
            }
        }
        
    }
    private IEnumerator BlinkRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.color = Color.white;
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        StartCoroutine(BlinkRed());
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Player Died");
    }
}

