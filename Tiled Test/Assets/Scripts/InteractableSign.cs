using UnityEngine;
using System.Collections;

public class InteractableSign : InteractableBase {

    public string text;

    private bool hasBeenInterctedWith = false;
    private Camera gameCamera;

    void Awake()
    {
        gameCamera = FindObjectOfType<Camera>();
    } 

    public override void OnInteractFromBelow(Character character)
    {
        if (!hasBeenInterctedWith)
        {
            hasBeenInterctedWith = true;
            if (transform.position.y < gameCamera.transform.position.y)
            {
                UpperDialogBox.instance.Show(text);
            }
            else
            {
                LowerDialogBox.instance.Show(text);
            }
            Time.timeScale = 0f;
        }
        else
        {
            hasBeenInterctedWith = false;
            LowerDialogBox.instance.Hide();
            UpperDialogBox.instance.Hide();
            Time.timeScale = 1f;
        }
    }
}
