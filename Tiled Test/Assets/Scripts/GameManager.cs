using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

[RequireComponent(typeof(SpawnPointManager))]
public class GameManager : MonoBehaviour {

    public static GameManager instance;

    private SpawnPointManager spawnPointManager;
    private ExitPointID exitPointID = ExitPointID.EnterSouth1;
    private Dictionary<int,Dictionary<string,int>> scenesElementsAndStates= new Dictionary<int, Dictionary<string, int>>();
    private int currentLevel = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        spawnPointManager = GetComponent<SpawnPointManager>();
        DontDestroyOnLoad(this);
    }

    void OnLevelWasLoaded(int level)
    {
        if (this != instance) return; //To avoid duplicate GameManager to run this before being destroyed

        currentLevel = level;

        spawnPointManager.ManageSpawnPoint(exitPointID);

        CheckScenesDictionary();
    }

    public void SetExitPoint(ExitPointID ID)
    {
        exitPointID = ID;
    }


    //Called when entering scene
    public void CheckScenesDictionary()
    {
        if (!scenesElementsAndStates.ContainsKey(currentLevel))
        {
            SetCurrentSceneDictionary();
        }
        else
        {
            InitializeCurrentScene();
        }
    }


    void SetCurrentSceneDictionary()
    {
        Dictionary<string, int> currentSceneDictionary = new Dictionary<string, int>();
        scenesElementsAndStates.Add(currentLevel, currentSceneDictionary);
        PopulateDictionary();
    }


    //Called when leaving scene
    public void SaveCurrentSceneDictionary()
    {
        if (!scenesElementsAndStates.ContainsKey(currentLevel))
        {
            //SetCurrentSceneDictionary(); -> It gave errors because the current scene got updated before this finished (Strangely it works fine with PopulateDictionary)
            Debug.Log("This scene's objects' state has not been stored (Probably because OnLevelWasLoaded was not called for this scene, worry if it happens more than once)");
        }
        else
        {
            PopulateDictionary();
        }
    }


    void PopulateDictionary()
    {
        foreach (TrackedObject sceneObject in FindObjectsOfType<TrackedObject>())
        {
            if (CheckThereIsUniqueId(sceneObject))
            {
                string uniqueId = sceneObject.GetComponent<UniqueId>().uniqueId;
                if (!scenesElementsAndStates[currentLevel].ContainsKey(uniqueId))
                {
                    scenesElementsAndStates[currentLevel].Add(uniqueId, sceneObject.GetCurrentState()); //Used to record the scene object and its state for the first time
                    Debug.Log("Object being tracked for the first time: " + sceneObject.name + " in scene " + currentLevel + " with id: " + uniqueId);
                }
                else
                {
                    scenesElementsAndStates[currentLevel][uniqueId] = sceneObject.GetCurrentState(); //Used for saving state when leaving scene
                    Debug.Log("Object state being saved: " + sceneObject.name + " in scene " + currentLevel + " with id: " + uniqueId);
                }
            }
        }
    }


    void InitializeCurrentScene()
    {
        foreach (TrackedObject sceneObject in FindObjectsOfType<TrackedObject>())
        {
            if (CheckThereIsUniqueId(sceneObject))
            {
                string uniqueId = sceneObject.GetComponent<UniqueId>().uniqueId;
                Debug.Log("Object being initialized: " + sceneObject.name + " with id: " + uniqueId);
                sceneObject.SetCurrentState(scenesElementsAndStates[currentLevel][uniqueId]);
            }
        }
    }

    bool CheckThereIsUniqueId(TrackedObject sceneObject)
    {
        if (!sceneObject.GetComponent<UniqueId>())
        {
            Debug.LogWarning("Unique Id in this object not found: " + sceneObject.name, sceneObject);
            return false;
        }
        return true;
    }
}
