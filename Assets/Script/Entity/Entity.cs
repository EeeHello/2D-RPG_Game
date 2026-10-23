using System;
using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public event Action OnFlipped;

    public Animator animator { get; private set; }
    public Rigidbody2D rb { get; private set; }

    protected StateMachine stateMachine;

    private bool facingRight = true;
    public int facingDir { get; private set; } = 1;

    [Header("Collision Detection")]
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform primaryWallCheck;
    [SerializeField] private Transform secondaryWallCheck;
    public bool isGroundDetected { get; private set; }
    public bool isWallDetected { get; private set; }

    // condition var
    private bool isKnocked;
    private Coroutine knockBackCoroutine;

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponentInChildren<Rigidbody2D>();

        stateMachine = new StateMachine();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        HandleCollisionDetection();
        stateMachine.UpdateActiveState();
    }

    public void CurrentStateAnimationTrigger()
    {
        stateMachine.currentState.AnimationTrigger();
    }

    public virtual void EntityDeath()
    {
  
    }

    public void ReceiveKnockBack(Vector2 knockback, float duration)
    {
        if (knockBackCoroutine != null)
        {
            StopCoroutine(knockBackCoroutine);
        }

        knockBackCoroutine = StartCoroutine(KnockBackCo(knockback, duration));
    }

    private IEnumerator KnockBackCo(Vector2 knockback, float duration)
    {
        isKnocked = true;
        rb.linearVelocity = knockback;

        yield return new WaitForSeconds(duration);

        rb.linearVelocity = Vector2.zero;
        isKnocked = false; 
    }

    public void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
        facingDir = facingDir * -1;

        OnFlipped?.Invoke();

    }

    public void HandleFlip(float xVelo)
    {
        // if moving right and not facing right 
        // then we need to flip transform
        // if facing left and facing right
        // then we need to flip transform

        if (xVelo > 0 && facingRight == false)
        {
            Flip();
        }
        else if (xVelo < 0 && facingRight)
        {
            Flip();
        }

    }

    public void SetVelocity(float xVelo, float yVelo)
    {
        if (isKnocked)
        {
            return;
        }

        rb.linearVelocity = new Vector2(xVelo, yVelo);
        HandleFlip(xVelo);
    }

    private void HandleCollisionDetection()
    {
        isGroundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

        if (secondaryWallCheck != null)
        {
            isWallDetected = Physics2D.Raycast(primaryWallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround)
                          && Physics2D.Raycast(secondaryWallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
        }
        else
        {
            isWallDetected = Physics2D.Raycast(primaryWallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
        }
    }


    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + new Vector3(0, -groundCheckDistance));
        Gizmos.DrawLine(primaryWallCheck.position, primaryWallCheck.position + new Vector3(wallCheckDistance * facingDir, 0));

        if (secondaryWallCheck != null)
        {
            Gizmos.DrawLine(secondaryWallCheck.position, secondaryWallCheck.position + new Vector3(wallCheckDistance * facingDir, 0));
        }
    }

}
