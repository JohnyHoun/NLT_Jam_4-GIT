using UnityEngine;

public enum Direction { Left, Right, Top, Down }
public enum RoomAxis { Horizontal, Vertical }

public class RoomTrigger : MonoBehaviour
{
    [SerializeField] private RoomAxis axis;
    [SerializeField] private Vector2 destinationPositionIfFollowing;

    private float enterValue;
    private bool playerInside;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        playerInside = true;       

        enterValue = axis == RoomAxis.Horizontal
            ? collision.transform.position.x
            : collision.transform.position.y;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!playerInside) return;
        if (!collision.CompareTag("Player")) return;

        //CameraRoomController.Instance.StaticCameraMode();

        float exitValue = axis == RoomAxis.Horizontal
            ? collision.transform.position.x
            : collision.transform.position.y;

        int dir = exitValue > enterValue ? +1 : -1;

        if (axis == RoomAxis.Horizontal)
            CameraRoomController.Instance.MoveHorizontal(dir);
        else
            CameraRoomController.Instance.MoveVertical(dir);

        playerInside = false;
    }
}
