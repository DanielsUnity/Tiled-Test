using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GoToSceneButton : MonoBehaviour {

    public string sceneName;

	public void OnClick () {
        SceneManager.LoadSceneAsync(sceneName);
    }
}
