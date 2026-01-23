using DG.Tweening;
using UnityEngine;

public class CameraRoomController : MonoBehaviour
{
    public static CameraRoomController Instance;

    [SerializeField] private float roomWidth = 32f;
    [SerializeField] private float roomHeight = 18f;
    [SerializeField] private float moveTime = 0.25f;

    private int roomX;
    private int roomY;
    private bool isMoving;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void MoveHorizontal(int dir)
    {
        if (isMoving) return;
        roomX += dir;
        MoveToRoom();
    }

    public void MoveVertical(int dir)
    {
        if (isMoving) return;
        roomY += dir;
        MoveToRoom();
    }

    private void MoveToRoom()
    {
        isMoving = true;

        Vector3 target = new Vector3(
            roomX * roomWidth,
            roomY * roomHeight,
            transform.position.z
        );

        transform.DOKill();
        transform.DOMove(target, moveTime)
            .SetEase(Ease.InOutSine)
            .OnComplete(() => isMoving = false);
    }
}
