using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class WorldChangeController : MonoBehaviour
{
    public static WorldChangeController Instance;
    public event Action OnWorldChangeAction;

    [Header("Checkponts:")]
    [SerializeField] private Transform firstCheckpoint;

    [Header("Worlds:")]
    [SerializeField] private TilemapRenderer doubleWorldRenderer;
    [Space]
    [SerializeField] private GameObject baseWorld;
    [SerializeField] private GameObject trollWorld;

    [Header("Materials:")]
    [SerializeField] private Material baseWorldMaterial;
    [SerializeField] private Material trollWorldMaterial;

    [Header("Variables:")]
    [SerializeField] private float worldChangeDelay = 0.2f;

    private bool _onBaseWorld = true;
    private bool _canChangeWorld = true;

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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            ChangeWorld();

        if (Input.GetKeyDown(KeyCode.R))
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
            doubleWorldRenderer.material = trollWorldMaterial;
            baseWorld.SetActive(false);
            trollWorld.SetActive(true);
            ChangeAllSpritesWithTag("Base_Spike", false);
            ChangeAllSpritesWithTag("Troll_Spike", true);
        }
        else
        {
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
        StartCoroutine(WorldChangeDelay());
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
