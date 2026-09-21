using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    public float speed = 5f;
    float horizontalInput;  

    [Header("Jump")]
    public float jumpForce = 10f;
    private float jumpRemaining = 1;
    

    [Header("Ground Check")]

    public Transform groundCheckPos;
    public Vector2 groundCheckSize;
    public bool isGrounded;

    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        rb.linearVelocity = new Vector2(horizontalInput * speed, rb.linearVelocityY);
        GroundCheck();
        
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalInput = context.ReadValue<Vector2>().x;
    }

    [System.Obsolete]
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

            

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheckPos.position,groundCheckSize);
    }
}
