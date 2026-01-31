using DG.Tweening;
using UnityEngine;

public enum RoomAxis
{
    Horizontal,
    Vertical
}

[RequireComponent(typeof(BoxCollider2D))]
public class RoomChange : MonoBehaviour
{
    [Header("Direction")]
    [SerializeField] private RoomAxis axis;
    [Tooltip("1 = direita / cima | -1 = esquerda / baixo")]
    [SerializeField] private int direction = 1;

    [Header("Room Size")]
    [SerializeField] private float roomWidth = 32f;
    [SerializeField] private float roomHeight = 18f;

    [Header("Transition")]
    [SerializeField] private float transitionTime = 0.4f;
    [SerializeField] private Ease ease = Ease.OutCubic;

    [Header("Block Return")]
    [Tooltip("Parede invisível que bloqueia a sala anterior")]
    [SerializeField] private GameObject backBlocker;

    private Transform _camera;
    private BoxCollider2D _trigger;
    private bool _used;

    private void Awake()
    {
        _camera = Camera.main.transform;
        _trigger = GetComponent<BoxCollider2D>();

        // Segurança extra: garante que o Z nunca mude
        Vector3 camPos = _camera.position;
        camPos.z = -10f;
        _camera.position = camPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_used) return;
        if (!collision.CompareTag("Player")) return;

        _used = true;

        // Mata o trigger imediatamente (evita reentrada)
        _trigger.enabled = false;

        // Bloqueia a volta
        if (backBlocker != null)
            backBlocker.SetActive(true);

        StartCameraTransition();
    }

    private void StartCameraTransition()
    {
        _camera.DOKill();

        if (axis == RoomAxis.Horizontal)
        {
            _camera.DOMoveX(
                _camera.position.x + roomWidth * direction,
                transitionTime
            ).SetEase(ease);
        }
        else
        {
            _camera.DOMoveY(
                _camera.position.y + roomHeight * direction,
                transitionTime
            ).SetEase(ease);
        }
    }
}
