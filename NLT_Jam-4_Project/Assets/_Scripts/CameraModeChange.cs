using UnityEngine;

public class CameraModeChange : MonoBehaviour
{
    [Header("Camera Mode")]
    [SerializeField] private bool enableFollow;

    [Header("Static Camera Position")]
    [SerializeField] private Vector3 staticCameraPosition;

    private bool playerInside;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (playerInside) return;

        playerInside = true;

        if (enableFollow)
        {
            CameraController.Instance.EnableFollow();
        }
        else
        {
            Vector3 pos = staticCameraPosition;
            pos.z = -10f;

            CameraController.Instance.DisableFollowAndMoveTo(pos);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        playerInside = false;
    }
}
