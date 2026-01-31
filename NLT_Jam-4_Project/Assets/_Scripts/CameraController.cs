using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    private GameObject _player;
    private bool _follow = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (!_follow) return;

        transform.position = new Vector3(_player.transform.position.x, _player.transform.position.y, -10);
    }

    public void EnableFollow()
    {
        if (_follow) return;

        transform.DOMove(new Vector3(_player.transform.position.x, _player.transform.position.y, -10), 0.2f).SetEase(Ease.OutCubic).OnComplete(() => _follow = true);
    }

    public void DisableFollowAndMoveTo(Vector3 positionToGo)
    {
        if (!_follow) return;

        _follow = false;

        transform.DOMove(positionToGo, 0.35f).SetEase(Ease.OutCubic);
    }
}
