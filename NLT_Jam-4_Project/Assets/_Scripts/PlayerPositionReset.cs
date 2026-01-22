using MoreMountains.Feedbacks;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerPositionReset : MonoBehaviour
{
    [SerializeField] private Transform firstSpawn;
    [SerializeField] private float spawnMovementDelay = 0.2f;
    [SerializeField] private UnityEvent respawnFeedbacl;

    [NonSerialized] public bool CanMove = true;

    private void Start()
    {
        gameObject.transform.position = firstSpawn.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 3)
        {
            WorldChangeController.Instance.ResetWorld();
            StartCoroutine(MovementDelayCoroutine());
            gameObject.transform.position = firstSpawn.position;

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
