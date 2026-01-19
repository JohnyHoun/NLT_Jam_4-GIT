using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldManager : MonoBehaviour
{
    [Header("Worlds:")]
    [SerializeField] private List<GameObject> BaseWorlds = new List<GameObject>();
    [SerializeField] private List<GameObject> TrollWorlds = new List<GameObject>();

    [Header("Scene to load number:")]
    [SerializeField] private int sceneNumber;

    public event Action OnWorldChangeAction; // Global world change event

    public static WorldManager Instance;

    private bool _onBaseWorld = true;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            ChangeWorld();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(sceneNumber);
        }
    }

    private void ChangeWorld()
    {
        if(_onBaseWorld)
        {
            ChangeStateListObjects(BaseWorlds, false);
            ChangeStateListObjects(TrollWorlds, true);
        }
        else
        {
            ChangeStateListObjects(BaseWorlds, true);
            ChangeStateListObjects(TrollWorlds, false);
        }

        _onBaseWorld = !_onBaseWorld;

        OnWorldChangeAction?.Invoke(); 
    }

    private void ChangeStateListObjects(List<GameObject> objects, bool activate)
    {
        foreach(GameObject obj in objects)
        {
            if(activate)
                obj.SetActive(true);
            else
                obj.SetActive(false);
        }
    }
}
