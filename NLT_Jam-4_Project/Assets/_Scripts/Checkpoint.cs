using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Vector3 CameraPosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || WorldChangeController.Instance.ActualCheckpointScript.gameObject.transform.position == gameObject.transform.position) return;

        WorldChangeController.Instance.ActualCheckpointScript = this;
    }
}
