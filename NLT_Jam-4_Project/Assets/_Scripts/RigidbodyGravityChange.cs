using UnityEngine;

public class RigidbodyGravityChange : MonoBehaviour
{
    private Rigidbody2D _rb2D;
    //private float rbGravityScale;

    private void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        //rbGravityScale = _rb2D.gravityScale;
    }

    public void StartFalling()
    {
        _rb2D.bodyType = RigidbodyType2D.Dynamic;
        _rb2D.gravityScale = 1f;
    }

    public void StopFalling()
    {
        _rb2D.linearVelocity = Vector2.zero;
        _rb2D.bodyType = RigidbodyType2D.Kinematic;
    }
}
