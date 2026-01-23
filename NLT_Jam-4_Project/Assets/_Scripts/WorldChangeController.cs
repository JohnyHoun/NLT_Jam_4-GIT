using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class WorldChangeController : MonoBehaviour
{
    public static WorldChangeController Instance;
    public event Action OnWorldChangeAction;

    [Header("Checkpoints:")]
    public Transform ActualCheckpointPosition;

    [Header("Worlds:")]
    [SerializeField] private TilemapRenderer doubleWorldRenderer;
    [SerializeField] private TilemapRenderer decorationRenderer;
    [Space]
    [SerializeField] private GameObject baseWorld;
    [SerializeField] private GameObject trollWorld;

    [Header("Materials:")]
    [SerializeField] private Material baseWorldMaterial;
    [SerializeField] private Material trollWorldMaterial;
    [Space]
    [SerializeField] private Material trollplayerMaterial;

    [Header("Variables:")]
    [SerializeField] private float worldChangeDelay = 0.2f;

    private bool _onBaseWorld = true;
    private bool _canChangeWorld = true;
    private SpriteRenderer _playerRenderer;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);      
    }

    private void Start()
    {
        ResetWorld();
        _playerRenderer = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            ChangeWorld();

        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // --------------------------------------------------
    // World Logic
    // --------------------------------------------------

    public void ChangeWorld()
    {
        if (!_canChangeWorld) return;
        StartCoroutine(WorldChangeDelay());

        if (_onBaseWorld)
        {
            _playerRenderer.material = trollplayerMaterial;
            decorationRenderer.material = trollWorldMaterial;
            doubleWorldRenderer.material = trollWorldMaterial;
            baseWorld.SetActive(false);
            trollWorld.SetActive(true);
            ChangeAllSpritesWithTag("Base_Spike", false);
            ChangeAllSpritesWithTag("Troll_Spike", true);
        }
        else
        {
            _playerRenderer.material = baseWorldMaterial;
            decorationRenderer.material = baseWorldMaterial;
            doubleWorldRenderer.material = baseWorldMaterial;
            baseWorld.SetActive(true);
            trollWorld.SetActive(false);
            ChangeAllSpritesWithTag("Base_Spike", true);
            ChangeAllSpritesWithTag("Troll_Spike", false);
        }

        _onBaseWorld = !_onBaseWorld;

        OnWorldChangeAction?.Invoke();
    }

    public void ResetWorld()
    {
        //StartCoroutine(WorldChangeDelay());
        doubleWorldRenderer.material = baseWorldMaterial;
        doubleWorldRenderer.material = baseWorldMaterial;
        baseWorld.SetActive(true);
        trollWorld.SetActive(false);
        ChangeAllSpritesWithTag("Base_Spike", true);
        ChangeAllSpritesWithTag("Troll_Spike", false);

        _onBaseWorld = true;
    }

    private IEnumerator WorldChangeDelay()
    {
        _canChangeWorld = false;
        yield return new WaitForSeconds(worldChangeDelay);
        _canChangeWorld = true;
    }

    // --------------------------------------------------
    // Spikes Visual Logic
    // --------------------------------------------------

    public void ChangeAllSpritesWithTag(string tag, bool appear)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject obj in objects)
        {
            SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
            if (sr == null) continue;

            Color c = sr.color;
            if(appear)
                c.a = 1f;
            else
                c.a = 0f;

            sr.color = c;
        }
    }
}
