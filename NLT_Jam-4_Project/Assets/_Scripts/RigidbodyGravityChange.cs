using UnityEngine;

public class RigidbodyGravityChange : MonoBehaviour
{
    [SerializeField] private bool worldChangeManager_Listening;

    private Rigidbody2D _rb2D;

    private void Start() // Ensures that WorldManager is not null on OnEnable.
    {
        _rb2D = GetComponent<Rigidbody2D>();
        TrySubscribe();
    }

    private bool _isBaseWorld = true;
    private bool _isSubscribed;

    private void OnEnable()
    {
        TrySubscribe();
    }

    private void OnDisable()
    {
        if (!_isSubscribed || WorldChangeManager.Instance == null)
            return;

        WorldChangeManager.Instance.OnWorldChangeAction -= HandleWorldChange;
        _isSubscribed = false;
    }

    private void TrySubscribe()
    {
        if (_isSubscribed || WorldChangeManager.Instance == null)
            return;

        WorldChangeManager.Instance.OnWorldChangeAction += HandleWorldChange;
        _isSubscribed = true;
    }

    private void HandleWorldChange()
    {
        if (!worldChangeManager_Listening)
            return;

        if (_isBaseWorld) // If there is no second event the first is always called.
            StartFalling();
        else
            StopFalling();

        _isBaseWorld = !_isBaseWorld;
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
