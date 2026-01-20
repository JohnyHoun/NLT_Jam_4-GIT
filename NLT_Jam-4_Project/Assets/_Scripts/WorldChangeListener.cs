using UnityEngine;
using UnityEngine.Events;

public class WorldChangeListener : MonoBehaviour
{
    [SerializeField] private UnityEvent onWorldChange_1;
    [SerializeField] private UnityEvent onWorldChange_2;

    private bool _isBaseWorld = true;
    private bool _isSubscribed;

    private void OnEnable()
    {
        TrySubscribe();
    }

    private void Start() // Ensures that WorldManager is not null on OnEnable.
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
        if (_isBaseWorld || onWorldChange_2.GetPersistentEventCount() == 0) // If there is no second event the first is always called.
            onWorldChange_1?.Invoke();
        else
            onWorldChange_2?.Invoke();

        _isBaseWorld = !_isBaseWorld;
    }    
}
