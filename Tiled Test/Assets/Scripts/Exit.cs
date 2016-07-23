using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour {

    public string sceneToLoad;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            gameManager.SaveCurrentSceneDictionary();
            gameManager.SetExitPoint(GetComponentInParent<ExitPoint>().exitPointID);
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
