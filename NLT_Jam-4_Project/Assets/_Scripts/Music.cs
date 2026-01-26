using UnityEngine;

public class Music : MonoBehaviour
{
    public static Music Instance;

    [Header("Configurações")]
    public AudioSource audioSource;

    void Awake()
    {
        // Garante que só exista um Music no jogo
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        audioSource.loop = true; // repete automaticamente
        audioSource.Play();
    }
}
