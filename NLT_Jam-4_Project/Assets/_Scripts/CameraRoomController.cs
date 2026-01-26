using DG.Tweening;
using System;
using UnityEngine;

public class CameraRoomController : MonoBehaviour
{
    public static CameraRoomController Instance;

    [Header("Room Settings")]
    [SerializeField] private float roomWidth = 32f;
    [SerializeField] private float roomHeight = 18f;

    [Header("Follow Settings")]
    [SerializeField] private float followYOffset = 3f;
    [SerializeField] private float followSnapTime = 0.35f;

    [Header("Movement")]
    [SerializeField] private float moveTime = 0.3f;

    private int roomX;
    private int roomY;

    [SerializeField] private bool followPlayer;
    private bool isMoving;

    private GameObject player;
    private Tween currentTween;

    // -------------------------------------------------

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        player = GameObject.FindGameObjectWithTag("Player");

        roomX = Mathf.RoundToInt(transform.position.x / roomWidth);
        roomY = Mathf.RoundToInt(transform.position.y / roomHeight);

        followPlayer = true;
    }

    private void LateUpdate()
    {
        if (!followPlayer || isMoving || currentTween != null)
            return;

        transform.position = new Vector3(
            player.transform.position.x,
            player.transform.position.y + followYOffset,
            -10f
        );
    }

    // -------------------------------------------------
    // FOLLOW MODES
    // -------------------------------------------------

    public void EnableFollow()
    {
        KillTween();
        if (followPlayer) return;

        followPlayer = false;

        Vector3 target = new Vector3(
            player.transform.position.x,
            player.transform.position.y + followYOffset,
            -10f
        );

        currentTween = transform.DOMove(target, followSnapTime)
            .SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                currentTween = null;
                followPlayer = true;
            });
    }

    public void DisableFollowAndMoveTo(Vector3 worldPosition)
    {
        KillTween();
        if (!followPlayer) return;

        followPlayer = false;

        currentTween = transform.DOMove(worldPosition, moveTime)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                currentTween = null;
            });
    }


    // -------------------------------------------------
    // ROOM TRANSITION (CHAMADO PELO TRIGGER)
    // -------------------------------------------------

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
        followPlayer = false;
        KillTween();

        Vector3 target = new Vector3(
            roomX * roomWidth,
            roomY * roomHeight,
            -10f
        );

        currentTween = transform.DOMove(target, moveTime)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                currentTween = null;
                isMoving = false;
            });
    }

    // -------------------------------------------------
    // UTILS
    // -------------------------------------------------

    private void KillTween()
    {
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
            currentTween = null;
        }
    }
}
