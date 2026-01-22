using System.Collections;
using TMPro;
using UnityEngine;
using DG.Tweening;

public enum Direction { Left, Right, Top, Down }

public class RoomTrigger : MonoBehaviour
{
    [SerializeField] private int checkPointNumber;
    [SerializeField] private Direction direction = Direction.Left;
    [Space]

    private GameObject _mainCamera;
    private bool _canMove = true;
    private bool _alreadyMove = false;

    private void Start()
    {
        _mainCamera = Camera.main.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || !_canMove)
            return;

        int multiplyer = 1;

        StartCoroutine(MoveDelay());       

        if (_alreadyMove)
            multiplyer = -1;
        else
            multiplyer = 0;

        switch(direction)
        {
            case Direction.Right:
                _mainCamera.transform.DOMoveX((multiplyer * _mainCamera.transform.position.x) + 32f, 0.2f).SetEase(Ease.InOutSine);
                break;
            case Direction.Left:
                _mainCamera.transform.DOMoveX((multiplyer * _mainCamera.transform.position.x) - 32f, 0.2f).SetEase(Ease.InOutSine);
                break;
            case Direction.Top:
                _mainCamera.transform.DOMoveY((multiplyer * _mainCamera.transform.position.y) + 18f, 0.2f).SetEase(Ease.InOutSine);
                break;
            case Direction.Down:
                _mainCamera.transform.DOMoveY((multiplyer * _mainCamera.transform.position.y) - 18f, 0.2f).SetEase(Ease.InOutSine);
                break;
        }

        _alreadyMove = !_alreadyMove;
    }

    private IEnumerator MoveDelay()
    {
        _canMove = false;
        yield return new WaitForSeconds(0.5f);
        _canMove = true;
    }
}
