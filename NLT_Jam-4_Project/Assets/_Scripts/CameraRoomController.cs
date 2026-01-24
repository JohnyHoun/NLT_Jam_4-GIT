using DG.Tweening;
using UnityEngine;

public class CameraRoomController : MonoBehaviour
{
    public static CameraRoomController Instance;

    [SerializeField] private float roomWidth = 32f;
    [SerializeField] private float roomHeight = 18f;
    [SerializeField] private float moveTime = 0.25f;
    [Space]
    [SerializeField] private bool followPlayer = false;

    private int roomX;
    private int roomY;
    private bool isMoving;
    private GameObject _player;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if(followPlayer)
            transform.position = new Vector3 (_player.transform.position.x, _player.transform.position.y, -10f);
    }

    public void FollowCameraMode() => followPlayer = true;

    public void StaticCameraMode(Vector2 destinationPosition)
    {
        if(!followPlayer) return;

        //followPlayer = false;

        gameObject.transform.DOMove(destinationPosition, 0.2f).SetEase(Ease.InOutSine).OnComplete(() => followPlayer = false);
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
