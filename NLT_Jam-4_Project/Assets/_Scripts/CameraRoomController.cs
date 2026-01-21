using UnityEngine;
using DG.Tweening;

public class CameraRoomController : MonoBehaviour
{
    [Header("Room Settings")]
    [SerializeField] private float roomWidth = 32f;
    [SerializeField] private float moveDuration = 0.25f;
    [SerializeField] private Ease moveEase = Ease.InOutSine;

    private int _currentRoomIndex = 0;
    private Camera _camera;
    private Tween _moveTween;

    private void Awake()
    {
        _camera = Camera.main;
    }

    public void MoveToNextRoom()
    {
        MoveToRoom(_currentRoomIndex + 1);
    }

    public void MoveToPreviousRoom()
    {
        MoveToRoom(_currentRoomIndex - 1);
    }

    private void MoveToRoom(int newIndex)
    {
        if (newIndex == _currentRoomIndex)
            return;

        _currentRoomIndex = newIndex;

        float targetX = _currentRoomIndex * roomWidth;

        _moveTween?.Kill();

        _moveTween = _camera.transform
            .DOMoveX(targetX, moveDuration)
            .SetEase(moveEase);
    }
}
