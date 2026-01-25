using DG.Tweening;
using UnityEngine;

public enum RoomAxis
{
    Horizontal,
    Vertical
}

public class RoomTrigger : MonoBehaviour
{
    [SerializeField] private RoomAxis axis;
    [SerializeField] private Vector3 destination;
    [Space]
    [SerializeField] private float verticalForce = 4f;

    private bool playerInside;
    private bool _alreadyMove = false;

    private bool _cameraPosOk = false;
    private Vector3 _cameraLastPosition;

    private GameObject _player;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if(!_cameraPosOk)
        {
            _cameraLastPosition = Camera.main.transform.position;
            _cameraPosOk = true;
        }

        playerInside = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!playerInside) return;
        if (!collision.CompareTag("Player")) return;

        if (!_alreadyMove)
        {
            Camera.main.transform.DOMove(destination, 0.2f).SetEase(Ease.OutCubic);

            if(axis == RoomAxis.Horizontal)
                _player.transform.DOMove(new Vector2(_player.transform.position.x + 0.5f, _player.transform.position.y), 0.2f).SetEase(Ease.OutCubic);
            else if(axis == RoomAxis.Vertical)
                _player.transform.DOMove(new Vector2(_player.transform.position.x, _player.transform.position.y + verticalForce), 0.2f).SetEase(Ease.OutCubic);
        }
        else
        {
            Camera.main.transform.DOMove(_cameraLastPosition, 0.2f).SetEase(Ease.OutCubic);

            if (axis == RoomAxis.Horizontal)
                _player.transform.DOMove(new Vector2(_player.transform.position.x - 0.5f, _player.transform.position.y), 0.2f).SetEase(Ease.OutCubic);
            //else if (axis == RoomAxis.Vertical)
                //_player.transform.DOMove(new Vector2(_player.transform.position.x, _player.transform.position.y - 0.5f), 0.2f).SetEase(Ease.OutCubic);
        }           

        _alreadyMove = !_alreadyMove;
        playerInside = false;
    }
}
