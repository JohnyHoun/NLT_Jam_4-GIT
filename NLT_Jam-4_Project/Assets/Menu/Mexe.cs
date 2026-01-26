using UnityEngine;

public class Mexe : MonoBehaviour
{
    public float amplitude = 0.5f; // Quão alto o texto sobe/desce
    public float frequency = 1f;   // Velocidade da oscilação

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.localPosition = startPos + new Vector3(0f, yOffset, 0f);
    }
}