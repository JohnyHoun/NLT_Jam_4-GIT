using DG.Tweening;
using System.Collections;
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
            transform.position = new Vector3 (_player.transform.position.x, _player.transform.position.y + 3f, -10f);
    }

    public void FollowCameraMode() => followPlayer = true;

    public void StaticCameraMode(Vector2 destinationPosition)
    {
        if(!followPlayer) return;

        //followPlayer = false;
        StartCoroutine(PointPlayer());

        gameObject.transform.DOMove(new Vector3(destinationPosition.x, destinationPosition.y, -10f), 0.5f).SetEase(Ease.InOutSine).OnComplete(() => followPlayer = false);
    }

    private IEnumerator PointPlayer()
    {
        yield return new WaitForSeconds(0.5f);

        gameObject.transform.DOMove(new Vector3(_player.transform.position.x, _player.transform.position.y + 3f, -10f), 0.5f).SetEase(Ease.InOutSine).
            OnComplete(() => followPlayer = true);
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
