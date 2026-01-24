using DG.Tweening;
using System.Collections;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    [SerializeField] private Vector2Int tilesToMove;
    [Space]
    [SerializeField] private float movementTime = 0.2f;
    [SerializeField] private float movementDelay = 0.1f;
    [Space]
    [SerializeField] private Material TrollSpike_Material;

    private bool _alreadyDone = false;
    private Vector3 _firstPosition;

    private void Start()
    {
        _firstPosition = transform.position;
        gameObject.GetComponentInParent<SpriteRenderer>().material = TrollSpike_Material;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_alreadyDone || !other.CompareTag("Player"))
            return;

        _alreadyDone = true;

        StartCoroutine(MoveWithDelay());
    }

    public IEnumerator MoveWithDelay()
    {
        yield return new WaitForSeconds(movementDelay);

        gameObject.transform.parent.DOMove(new Vector3(transform.position.x + tilesToMove.x, transform.position.y + tilesToMove.y), movementTime).SetEase(Ease.InOutSine);
    }

    public void ResetPosition()
    {
        if(_firstPosition != Vector3.zero)
            gameObject.transform.parent.position = _firstPosition;

        _alreadyDone = false;
    }
}
