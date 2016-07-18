using UnityEngine;
using System.Collections;

public class DialogManager : MonoBehaviour {

    public string defaultText;

    private bool isLastInteraction = false;
    private Camera gameCamera;
    private TextImporter textImporter;
    private bool isUsingTextImporter = false;

    void Awake()
    {
        gameCamera = FindObjectOfType<Camera>();
        textImporter = GetComponent<TextImporter>();
    }

    void Start()
    {
        if (textImporter) { isUsingTextImporter = true; }
    }

    public void Manage()
    {
        if (!isLastInteraction)
        {
            ShowMessage();
        }
        else
        {
            DisableDialog();
        }
    }

    void DisableDialog()
    {
        isLastInteraction = false;
        LowerDialogBox.instance.Hide();
        UpperDialogBox.instance.Hide();
        Time.timeScale = 1f;
    }

    void ShowMessage()
    {
        Time.timeScale = 0f;
        if (!isUsingTextImporter)
        {
            WriteInCorrectBox(defaultText);
            isLastInteraction = true;
        }
        else
        {

            WriteInCorrectBox(textImporter.GetCurrentLine());
            if (!textImporter.AdvanceToNextLine())
            {
                isLastInteraction = true;
            }
        }
    }

    public void WriteInCorrectBox(string line)
    {
        if (transform.position.y < gameCamera.transform.position.y)
        {
            UpperDialogBox.instance.Show(line);
        }
        else
        {
            LowerDialogBox.instance.Show(line);
        }
    }

    public void WriteAlternateMessage(string line)
    {
        if (!isLastInteraction)
        {
            Time.timeScale = 0f;
            WriteInCorrectBox(line);
            isLastInteraction = true;
        }
        else
        {
            DisableDialog();
        }
    }
}
