using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPlataformMovement : MonoBehaviour
{
    [Header("Movement variables")]
    [SerializeField] float overlapRadius;
    [SerializeField] float jumpForce;
    [SerializeField] float speed;

    [Space]
    [SerializeField] float coyoteTime;
    [SerializeField] float jumpBuffer;

    [Space]
    [SerializeField] Vector2 groundOfset;
    [SerializeField] LayerMask groundLayer;

    [Header("Feedbacks")]
    [SerializeField] private MMF_Player jumpFeedback;
    [SerializeField] private MMF_Player landingFeedback;
    [SerializeField] private MMF_Player damageFeedback;

    private float _coyoteTimeCounter;
    private float _jumpBufferCounter;

    private bool _isGrounded;
    private bool _wasGrounded;
    private bool _isFacingRight = true;

    private Vector2 _movementInput;

    private Rigidbody2D _rb2D;
    private Animator _animator;

    private PlayerPositionReset playerPositionReset;

    private void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        playerPositionReset = GetComponent<PlayerPositionReset>();
    }   

    private void Update()
    {
        // Input
        _movementInput.x = Input.GetAxisRaw("Horizontal");

        // Jump buffer
        if (Input.GetKeyDown(KeyCode.Space))
            _jumpBufferCounter = jumpBuffer;
        else
            _jumpBufferCounter -= Time.deltaTime;

        // Jump cut (when releasing the button)
        if (Input.GetKeyUp(KeyCode.Space) && _rb2D.linearVelocity.y > 0)
        {
            _coyoteTimeCounter = 0f;
            _rb2D.linearVelocity = new Vector2(
                _rb2D.linearVelocity.x,
                _rb2D.linearVelocity.y * 0.5f
            );
        }
    }

    private void FixedUpdate()
    {
        // Store previous grounded state
        _wasGrounded = _isGrounded;

        // Ground check
        _isGrounded = Physics2D.OverlapCircle(
            (Vector2)transform.position + groundOfset,
            overlapRadius,
            groundLayer
        );

        // Walk / idle animation (after ground check)
        if (_isGrounded)
        {
            if (_movementInput.x != 0)
                _animator.CrossFade("Walk", 0.1f, 0);
            else
                _animator.CrossFade("Idle", 0.1f, 0);
        }

        // Flip sprite based on movement input
        HandleFlip();

        // Horizontal movement
        if(playerPositionReset.CanMove)
            transform.position += (Vector3)_movementInput * speed * Time.fixedDeltaTime;

        // Landing (just touched the ground)
        if (!_wasGrounded && _isGrounded)
        {
            landingFeedback?.PlayFeedbacks();
        }

        // Jump
        if (_coyoteTimeCounter > 0f && _jumpBufferCounter > 0)
        {
            _rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            jumpFeedback?.PlayFeedbacks();
            _animator.CrossFade("Jump", 0f, 0);

            _jumpBufferCounter = 0f;
            _coyoteTimeCounter = 0f;
        }

        // Coyote time
        if (_isGrounded)
            _coyoteTimeCounter = coyoteTime;
        else
            _coyoteTimeCounter -= Time.fixedDeltaTime;
    }

    private void HandleFlip()
    {
        if (_movementInput.x > 0 && !_isFacingRight)
            Flip();
        else if (_movementInput.x < 0 && _isFacingRight)
            Flip();
    }

    private void Flip()
    {
        _isFacingRight = !_isFacingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere((Vector2)transform.position + groundOfset, overlapRadius);
    }
#endif
}
