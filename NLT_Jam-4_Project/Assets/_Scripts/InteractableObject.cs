using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class InteractableAction
{
    public bool Move = false;
    public Vector2Int TilesToMove;
    public float MovementDelay = 0.1f;
    public float MovementTime = 0.2f;   
    [Space]
    public bool Rotate = false;
    public Vector2 DegressToRotate;
    public float RotationDelay = 0.1f;
    public float RotationTime = 0.2f;
    [Space]
    public UnityEvent InteractEvent;
}

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private List<InteractableAction> interactions = new List<InteractableAction>();
    [Space]
    [SerializeField] private List<SpriteRenderer> renderers = new List<SpriteRenderer>();
    [SerializeField] private Material ObjectMain_Material;

    private bool _alreadyDone = false;
    private Vector3 _originalPosition;
    private Quaternion _originalRotation;

    private void Start()
    {
        _originalPosition = transform.position;
        _originalRotation = transform.rotation;

        SpriteRenderer renderer = gameObject.GetComponentInParent<SpriteRenderer>();       

        if(renderer != null)
            renderer.material = ObjectMain_Material;

        foreach (SpriteRenderer rend in renderers)
        {
            if(rend != null)
                rend.material = ObjectMain_Material;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_alreadyDone || !other.CompareTag("Player"))
            return;

        _alreadyDone = true;

        CallActions();
    }

    private void CallActions()
    {
        foreach(InteractableAction action in interactions)
        {
            if(action == null) continue;

            if(action.Move)
                StartCoroutine(MoveWithDelay(action.MovementDelay, action.TilesToMove.x, action.TilesToMove.y, action.MovementTime));
            if(action.Rotate)
                StartCoroutine(RotateWithDelay(action.RotationDelay, action.DegressToRotate.x, action.DegressToRotate.y, action.RotationTime));

            action.InteractEvent?.Invoke();
        }
    }

    public IEnumerator MoveWithDelay(float delayTime, int tilesInX, int tilesInY, float actionTime)
    {
        yield return new WaitForSeconds(delayTime);

        gameObject.transform.parent.DOMove(new Vector3(transform.position.x + tilesInX, transform.position.y + tilesInY), actionTime).SetEase(Ease.InOutSine);
    }

    public IEnumerator RotateWithDelay(float delayTime, float degreesInX, float degreesInZ, float actionTime)
    {
        yield return new WaitForSeconds(delayTime);

        //gameObject.transform.parent.DORotate(new Vector3(transform.rotation.x + degreesInX, transform.rotation.z + degreesInZ), actionTime).SetEase(Ease.InOutSine);
    }

    public void ResetPosition()
    {
        if (_originalPosition != Vector3.zero)
            gameObject.transform.parent.position = _originalPosition;

        transform.rotation = _originalRotation;

        _alreadyDone = false;
    }
}
