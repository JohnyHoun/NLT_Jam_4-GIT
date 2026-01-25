using UnityEngine;
using UnityEngine.Events;

public class Trampoline : MonoBehaviour
{
    [SerializeField] private Vector3 jumpVector;
    [Space] 
    //[SerializeField] private UnityEvent firstJumpFeedback;
    [SerializeField] private UnityEvent jumpFeedback;

    private bool _alreadyJump = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player") return;

        Rigidbody2D rb2D = collision.GetComponent<Rigidbody2D>();

        rb2D.linearVelocity = Vector2.zero;
        rb2D.angularVelocity = 0f;

        rb2D.AddForce(jumpVector, ForceMode2D.Impulse);

        jumpFeedback?.Invoke();

        /*
        if(!_alreadyJump)
        {
            _alreadyJump = true;
            firstJumpFeedback?.Invoke();
        }
        else
        {
            jumpFeedback?.Invoke();
        }
        */
    }
}