using MoreMountains.Feedbacks;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerPositionReset : MonoBehaviour
{
    //public static PlayerPositionReset Instance;
    //public event Action OnRespawnAction;

    [SerializeField] private float spawnMovementDelay = 0.2f;
    [SerializeField] private UnityEvent respawnFeedbacl;

    [NonSerialized] public bool CanMove = true;

    private Rigidbody2D _rb2d;

    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        gameObject.transform.position = WorldChangeController.Instance.ActualCheckpointScript.gameObject.transform.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            WorldChangeController.Instance.ResetWorld();
            StartCoroutine(MovementDelayCoroutine());
            gameObject.transform.position = WorldChangeController.Instance.ActualCheckpointScript.gameObject.transform.position;

            respawnFeedbacl?.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 3)
        {
            WorldChangeController.Instance.ResetWorld();
            StartCoroutine(MovementDelayCoroutine());

            _rb2d.linearVelocity = Vector2.zero;
            _rb2d.angularVelocity = 0f;

            gameObject.transform.position = WorldChangeController.Instance.ActualCheckpointScript.gameObject.transform.position;

            respawnFeedbacl?.Invoke();
        }
    }

    private IEnumerator MovementDelayCoroutine()
    {
        CanMove = false;
        yield return new WaitForSeconds(spawnMovementDelay);
        CanMove = true;
    }
}
