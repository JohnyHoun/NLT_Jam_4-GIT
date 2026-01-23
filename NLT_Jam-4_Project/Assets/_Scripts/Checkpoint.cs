using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || WorldChangeController.Instance.ActualCheckpointPosition.position == gameObject.transform.position) return;

        WorldChangeController.Instance.ActualCheckpointPosition.position = gameObject.transform.position;
    }
}
