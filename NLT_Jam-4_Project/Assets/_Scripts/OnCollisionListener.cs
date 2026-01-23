using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class OnCollisionListener : MonoBehaviour
{
    [Header("Base Feedback:")]
    [SerializeField] private float delayTime = 0f;
    [SerializeField] private UnityEvent colisionEvent;
    [Header("Move Feedback:")]
    [SerializeField] private Transform objectToMove;
    [SerializeField] private float movementTime;
    [SerializeField] private Vector2 objectNewPosition;
    [Header("Fall Feedback:")]
    [SerializeField] private Rigidbody2D objectToFall;

    private bool _alreadyDone = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || _alreadyDone)
            return;

        StartCoroutine(MoveDelay());

        if (objectToMove != null)
            objectToMove.DOMove(objectNewPosition, movementTime).SetEase(Ease.InOutSine);

        if(objectToFall != null)
        {
            objectToFall.bodyType = RigidbodyType2D.Dynamic;
            objectToFall.gravityScale = 1f;
        }
    }

    private IEnumerator MoveDelay()
    {       
        yield return new WaitForSeconds(delayTime);
        _alreadyDone = true;

        colisionEvent?.Invoke();
    }
}
