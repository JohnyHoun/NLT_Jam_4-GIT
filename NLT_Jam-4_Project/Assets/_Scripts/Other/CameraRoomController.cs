using UnityEngine;
using DG.Tweening;
using System.Collections;

public class CameraRoomController : MonoBehaviour
{
    [Header("Room Settings")]
    [SerializeField] private float roomWidth = 32f;
    [SerializeField] private float roomHeigt = 18f;
    [Space]
    [SerializeField] private float moveDuration = 0.25f;
    [Space]
    [SerializeField] private Ease moveEase = Ease.InOutSine;

    private Camera _camera;
    private Tween _moveTween;
    private bool _canMove = true;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void MoveToNextRoom()
    {
        //MoveToRoom();
    }

    private void MoveToPreviousRoom()
    {
        //MoveToRoom();
    }

    public void MoveToRoom(Direction moveDirection, bool positiveMovement)
    {
        StartCoroutine(MoveDelay());

        int moveMultiplyer;

        if(positiveMovement)
            moveMultiplyer = 1;
        else
            moveMultiplyer = 0;

        float targetPosition = transform.position.x + (roomWidth * moveMultiplyer);

        _moveTween?.Kill();

        _moveTween = _camera.transform
            .DOMoveX(targetPosition, moveDuration)
            .SetEase(moveEase);
    }

    private IEnumerator MoveDelay()
    {
        _canMove = false;
        yield return new WaitForSeconds(moveDuration);
        _canMove = true;
    }
}
