using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;


public class WorldChangeController : MonoBehaviour
{
    public static WorldChangeController Instance;
    public event Action OnWorldChangeAction;
    [NonSerialized] private bool CanChangeWorld = false;

    [Header("Checkpoints:")]
    public int ActualCheckpointNumber;
    public List<Checkpoint> CheckpointsList;

    //[Header("Worlds:")]
    //[SerializeField] private TilemapRenderer doubleWorldRenderer;
    //[SerializeField] private TilemapRenderer decorationRenderer;
    //[Space]
    //[SerializeField] private GameObject baseWorld;
    //[SerializeField] private GameObject trollWorld;

    [Header("Materials:")]
    [SerializeField] private Material baseWorldMaterial;
    [SerializeField] private Material trollWorldMaterial;
    [Space]
    [SerializeField] private Material trollplayerMaterial;
    [Space]
    [SerializeField] private Material trollSpikesMaterial;

    [Header("Variables:")]
    [SerializeField] private float worldChangeDelay = 0.2f;

    private List<GameObject> _interactableObjects = new List<GameObject>();

    private List<GameObject> _baseWorldObjects = new List<GameObject>();
    private List<GameObject> _trollWorldObjects = new List<GameObject>();
    private List<TilemapRenderer> _doubleWorldTilemapRenderers = new List<TilemapRenderer>();
    private List<TilemapRenderer> _decorationTilemapRenderers = new List<TilemapRenderer>();
    private List<SpriteRenderer> _doubleWorldRenderers = new List<SpriteRenderer>();

    private bool _onBaseWorld = true;
    [SerializeField] private bool _canChangeWorld = true;
    private SpriteRenderer _playerRenderer;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        ActualCheckpointNumber = PlayerPrefs.GetInt("Actual_Level_Number");
    }

    private void Start()
    {       
        Camera.main.gameObject.transform.position = new Vector3(CheckpointsList[ActualCheckpointNumber].CameraPosition.x, 
            CheckpointsList[ActualCheckpointNumber].CameraPosition.y, -10);

        _playerRenderer = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();

        FillGameObjectListWithTag(_interactableObjects, "Interactable");
        FillGameObjectListWithTag(_baseWorldObjects, "Base_World");
        FillGameObjectListWithTag(_trollWorldObjects, "Troll_World");
        FillTilemapRendererListWithTag(_doubleWorldTilemapRenderers, "Double_World");
        FillSpriteRendererListWithTag(_doubleWorldRenderers, "Double_World");
        FillTilemapRendererListWithTag(_decorationTilemapRenderers, "Decoration");

        ResetWorld();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            ChangeWorld();

        if (Input.GetKeyDown(KeyCode.Backspace))
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

            ChangeAllTilemapsRenderersListMaterials(_doubleWorldTilemapRenderers, trollWorldMaterial);
            //decorationRenderer.material = trollWorldMaterial;
            ChangeAllTilemapsRenderersListMaterials(_decorationTilemapRenderers, trollWorldMaterial);
            //doubleWorldRenderer.material = trollWorldMaterial;
            ActivateGameObjectsList(_baseWorldObjects, false);
            //baseWorld.SetActive(false);
            ActivateGameObjectsList(_trollWorldObjects, true);
            //trollWorld.SetActive(true);
            ChangeAllSpriteRenderersListMaterials(_doubleWorldRenderers, trollSpikesMaterial);

            FadeAllSpritesWithTag("Base_Spike", false);
            FadeAllSpritesWithTag("Troll_Spike", true);
        }
        else
        {
            _playerRenderer.material = baseWorldMaterial;

            ChangeAllTilemapsRenderersListMaterials(_doubleWorldTilemapRenderers, baseWorldMaterial);
            //decorationRenderer.material = baseWorldMaterial;
            ChangeAllTilemapsRenderersListMaterials(_decorationTilemapRenderers, baseWorldMaterial);
            //doubleWorldRenderer.material = baseWorldMaterial;
            ActivateGameObjectsList(_baseWorldObjects, true);
            //baseWorld.SetActive(true);
            ActivateGameObjectsList(_trollWorldObjects, false);
            //trollWorld.SetActive(false);
            ChangeAllSpriteRenderersListMaterials(_doubleWorldRenderers, baseWorldMaterial);

            FadeAllSpritesWithTag("Base_Spike", true);
            FadeAllSpritesWithTag("Troll_Spike", false);
        }

        _onBaseWorld = !_onBaseWorld;

        OnWorldChangeAction?.Invoke();
    }

    public void ResetWorld()
    {
        StartCoroutine(WorldChangeDelay());

        _playerRenderer.material = baseWorldMaterial;

        ChangeAllTilemapsRenderersListMaterials(_doubleWorldTilemapRenderers, baseWorldMaterial);
        //doubleWorldRenderer.material = baseWorldMaterial;
        ChangeAllTilemapsRenderersListMaterials(_decorationTilemapRenderers, baseWorldMaterial);
        //decorationRenderer.material = baseWorldMaterial;

        ActivateGameObjectsList(_baseWorldObjects, true);
        //baseWorld.SetActive(true);
        ActivateGameObjectsList(_trollWorldObjects, false);
        //trollWorld.SetActive(false);

        ChangeAllSpriteRenderersListMaterials(_doubleWorldRenderers, baseWorldMaterial);

        FadeAllSpritesWithTag("Base_Spike", true);
        FadeAllSpritesWithTag("Troll_Spike", false);

        foreach(GameObject movableObject in _interactableObjects)
        {
            InteractableObject interactablScript = movableObject.GetComponent<InteractableObject>();

            if(interactablScript != null)
                interactablScript.ResetPosition();
        }

        _onBaseWorld = true;        
    }

    private IEnumerator WorldChangeDelay()
    {
        _canChangeWorld = false;
        yield return new WaitForSeconds(worldChangeDelay);
        _canChangeWorld = true;
    }

    // --------------------------------------------------
    // List Fill Logics
    // --------------------------------------------------

    private void FillGameObjectListWithTag(List<GameObject> listToFill, string tag) // GameObject list fill
    {
        listToFill.Clear();

        GameObject[] foundObjects = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject obj in foundObjects)
        {
            listToFill.Add(obj);
        }
    }

    private void FillTilemapRendererListWithTag(List<TilemapRenderer> listToFill, string tag)  // TilmepaRenderer list fill
    {
        listToFill.Clear();

        GameObject[] foundObjects = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject obj in foundObjects)
        {
            TilemapRenderer tr = obj.GetComponent<TilemapRenderer>();
            if (tr != null)
                listToFill.Add(tr);
        }
    }

    private void FillSpriteRendererListWithTag(List<SpriteRenderer> listToFill, string tag)  // TilmepaRenderer list fill
    {
        listToFill.Clear();

        GameObject[] foundObjects = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject obj in foundObjects)
        {
            SpriteRenderer tr = obj.GetComponent<SpriteRenderer>();
            if (tr != null)
                listToFill.Add(tr);
        }
    }

    // --------------------------------------------------
    // Visual Logics
    // --------------------------------------------------

    private void FadeAllSpritesWithTag(string tag, bool appear)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject obj in objects)
        {
            SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
            if (renderer == null) continue;

            Color c = renderer.color;
            if (appear)
                c.a = 1f;
            else
                c.a = 0f;

            renderer.color = c;
        }
    }
    
    private void ChangeAllTilemapsRenderersListMaterials(List<TilemapRenderer> tilemapRendererList, Material newMaterial)
    {
        foreach (TilemapRenderer tilemapRenderer in tilemapRendererList)
        {
            if (tilemapRenderer == null)
                continue;

            tilemapRenderer.material = newMaterial;
        }
    }

    private void ChangeAllSpriteRenderersListMaterials(List<SpriteRenderer> tilemapRendererList, Material newMaterial)
    {
        foreach (SpriteRenderer tilemapRenderer in tilemapRendererList)
        {
            if (tilemapRenderer == null)
                continue;

            tilemapRenderer.material = newMaterial;
        }
    }

    private void ActivateGameObjectsList(List<GameObject> gameObjectList, bool activate)
    {
        foreach (GameObject obj in gameObjectList)
        {
            if (gameObjectList == null)
                continue;

            if (activate)
                obj.SetActive(true);
            else
                obj.SetActive(false);
        }
    }
}
