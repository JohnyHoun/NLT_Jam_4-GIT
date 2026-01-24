using UnityEngine;

public class CameraModeChange : MonoBehaviour
{
    [Header("Follow:")]
    [SerializeField] private bool followPlayer;

    [Header("Static:")]
    [SerializeField] private Vector2 cameraDestinationPosition;

    private bool _playerInside;   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        _playerInside = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!_playerInside || !collision.CompareTag("Player")) return;

        if (followPlayer)
            CameraRoomController.Instance.FollowCameraMode();
        else
            CameraRoomController.Instance.StaticCameraMode(cameraDestinationPosition);

        _playerInside = false;
    }
}
