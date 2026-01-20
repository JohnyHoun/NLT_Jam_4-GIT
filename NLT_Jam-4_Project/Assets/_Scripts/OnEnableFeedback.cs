using UnityEngine;
using UnityEngine.Events;

public class OnEnableFeedback : MonoBehaviour
{
    [SerializeField] private UnityEvent onEnableEvent;

    private void OnEnable()
    {
        onEnableEvent?.Invoke();
    }
}
