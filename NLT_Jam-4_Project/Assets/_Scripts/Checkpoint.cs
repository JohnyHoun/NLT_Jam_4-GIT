using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int CheckpointNumber;
    public Vector3 CameraPosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || WorldChangeController.Instance.ActualCheckpointNumber == CheckpointNumber) return;

        WorldChangeController.Instance.ActualCheckpointNumber = CheckpointNumber;
    }
}
