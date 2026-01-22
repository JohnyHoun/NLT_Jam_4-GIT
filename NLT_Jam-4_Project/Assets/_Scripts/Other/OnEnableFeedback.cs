using UnityEngine;
using UnityEngine.Events;

public class OnEnableFeedback : MonoBehaviour
{
    [SerializeField] private bool doFirstFeedback = true;
    [SerializeField] private UnityEvent onEnableEvent;

    private void OnEnable()
    {
        if(doFirstFeedback)
            onEnableEvent?.Invoke();

        doFirstFeedback = true;
        print("Tween");
    }
}
