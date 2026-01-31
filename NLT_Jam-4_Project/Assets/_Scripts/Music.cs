using UnityEngine;

public class Music : MonoBehaviour
{
    public static Music Instance;

    [Header("Configurações")]
    public AudioSource audioSource;

    private bool _musicOn = true;
    private float _initialVolume;

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

        _initialVolume = audioSource.volume;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            if (_musicOn)
                audioSource.volume = 0f;
            else
                audioSource.volume = _initialVolume;

            _musicOn = !_musicOn;
        }
    }
}
