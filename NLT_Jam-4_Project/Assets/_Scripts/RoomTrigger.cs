using UnityEngine;

public enum Direction { Left, Right }

public class RoomTrigger : MonoBehaviour
{   
    [SerializeField] private Direction direction;
    [SerializeField] private CameraRoomController cameraController;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;
        print("Trigger");

        if (direction == Direction.Right)
            cameraController.MoveToNextRoom();
        else
            cameraController.MoveToPreviousRoom();
    }
}
