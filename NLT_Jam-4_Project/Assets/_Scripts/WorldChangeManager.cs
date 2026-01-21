using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldChangeManager : MonoBehaviour
{
    [Header("Visual Debug:")]
    [SerializeField, Range(0f, 1f)]
    private float spriteFadeAlphaValue = 0f;

    [Header("Checkponts:")]
    [SerializeField] private Transform firstCheckpoint;

    [Header("Materials:")]
    [SerializeField] private Material baseWorldMaterial;
    [SerializeField] private Material trollWorldMaterial;

    public event Action OnWorldChangeAction;

    public static WorldChangeManager Instance;

    private bool _onBaseWorld = true;

    private Vector2 _actualCheckpoint;

    // World renderers
    private readonly List<Renderer> baseWorldRenderers = new();
    private readonly List<Renderer> trollWorldRenderers = new();

    // Spikes renderers
    private readonly List<Renderer> baseSpikesRenderers = new();
    private readonly List<Renderer> trollSpikesRenderers = new();

    private static readonly int ColorID = Shader.PropertyToID("_Color");

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        CollectAllRenderers();
        JustBaseWorld();

        spriteFadeAlphaValue = 0f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            ChangeWorld();

        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // --------------------------------------------------
    // World logic
    // --------------------------------------------------

    public void ChangeWorld()
    {
        CollectAllRenderers();

        UpdateWorldsVisuals();

        _onBaseWorld = !_onBaseWorld;

        OnWorldChangeAction?.Invoke();
    }

    // --------------------------------------------------
    // Debug / preview modes
    // --------------------------------------------------

    public void JustBaseWorld()
    {
        CollectAllRenderers();

        SetRenderersAlpha(baseWorldRenderers, 1f);
        SetRenderersAlpha(trollWorldRenderers, 0f);

        SetRenderersAlpha(baseSpikesRenderers, 1f);
        SetRenderersAlpha(trollSpikesRenderers, 0f);

        _onBaseWorld = true;
    }

    public void JustTrollWorld()
    {
        CollectAllRenderers();

        SetRenderersAlpha(trollWorldRenderers, 1f);
        SetRenderersAlpha(baseWorldRenderers, 0f);

        SetRenderersAlpha(trollSpikesRenderers, 1f);
        SetRenderersAlpha(baseSpikesRenderers, 0f);

        _onBaseWorld = false;
    }

    public void TwoWorlds()
    {
        CollectAllRenderers();

        SetRenderersAlpha(baseWorldRenderers, 1f);
        SetRenderersAlpha(trollWorldRenderers, 1f);

        AllSpikes();
    }

    public void AllSpikes()
    {
        CollectAllRenderers();

        // Both spike groups become visible
        SetRenderersAlpha(baseSpikesRenderers, 1f);
        SetRenderersAlpha(trollSpikesRenderers, 1f);
    }

    // --------------------------------------------------
    // Visual update
    // --------------------------------------------------

    private void UpdateWorldsVisuals()
    {
        if (!_onBaseWorld)
        {
            // Base world visible
            SetRenderersAlpha(baseWorldRenderers, 1f);
            SetRenderersAlpha(trollWorldRenderers, spriteFadeAlphaValue);

            SetRenderersAlpha(baseSpikesRenderers, 1f);
            SetRenderersAlpha(trollSpikesRenderers, spriteFadeAlphaValue);
        }
        else
        {
            // Troll world visible
            SetRenderersAlpha(trollWorldRenderers, 1f);
            SetRenderersAlpha(baseWorldRenderers, spriteFadeAlphaValue);

            SetRenderersAlpha(trollSpikesRenderers, 1f);
            SetRenderersAlpha(baseSpikesRenderers, spriteFadeAlphaValue);
        }
    }

    private void SetRenderersAlpha(List<Renderer> renderers, float targetAlpha)
    {
        foreach (Renderer renderer in renderers)
        {
            if (renderer == null)
                continue;

            CompositeCollider2D tilemapCollider = renderer.GetComponent<CompositeCollider2D>();

            if(targetAlpha == 1f && tilemapCollider != null)
                tilemapCollider.isTrigger = false;
            else if(targetAlpha == 0f && tilemapCollider != null)
                tilemapCollider.isTrigger = true;

            MaterialPropertyBlock block = new();
            renderer.GetPropertyBlock(block);

            Color color = Color.white;

            if (block.HasColor(ColorID))
                color = block.GetColor(ColorID);
            else if (renderer.sharedMaterial.HasProperty(ColorID))
                color = renderer.sharedMaterial.color;

            color.a = targetAlpha;

            block.SetColor(ColorID, color);
            renderer.SetPropertyBlock(block);

#if UNITY_EDITOR
            if (!Application.isPlaying)
                EditorUtility.SetDirty(renderer);
#endif
        }
    }

    // --------------------------------------------------
    // Renderer collection
    // --------------------------------------------------

    private void CollectAllRenderers()
    {
        baseWorldRenderers.Clear();
        trollWorldRenderers.Clear();
        baseSpikesRenderers.Clear();
        trollSpikesRenderers.Clear();

        CollectByTag("Base_World", baseWorldRenderers);
        CollectByTag("Troll_World", trollWorldRenderers);
        CollectByTag("Base_Spike", baseSpikesRenderers);
        CollectByTag("Troll_Spike", trollSpikesRenderers);
    }

    private void CollectByTag(string tag, List<Renderer> list)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject obj in objects)
        {
            Renderer[] found = obj.GetComponentsInChildren<Renderer>(true);
            list.AddRange(found);
        }
    }
}

// --------------------------------------------------
// Editor
// --------------------------------------------------

[CustomEditor(typeof(WorldChangeManager))]
public class WorldChangeManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        WorldChangeManager manager = (WorldChangeManager)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Change World"))
            manager.ChangeWorld();
        GUILayout.Space(5);
        if (GUILayout.Button("Just Base World"))
            manager.JustBaseWorld();
        if (GUILayout.Button("Just Troll World"))
            manager.JustTrollWorld();
        GUILayout.Space(5);
        if (GUILayout.Button("Two Worlds"))
            manager.TwoWorlds();
        GUILayout.Space(5);
        if (GUILayout.Button("All Spikes"))
            manager.AllSpikes();
    }
}
