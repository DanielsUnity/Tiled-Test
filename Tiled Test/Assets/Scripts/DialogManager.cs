using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour {

    public string defaultText;

    private bool isLastInteraction = false;
    private Camera gameCamera;
    private TextImporter textImporter;
    private bool isUsingTextImporter = false;
    private bool isFreshLine = true;

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
        DisableBoxes();
        Time.timeScale = 1f;
    }

    void DisableBoxes()
    {
        LowerDialogBox.instance.Hide();
        UpperDialogBox.instance.Hide();
        DisableAnswerBoxes();
    }

    void DisableAnswerBoxes()
    {
        AnswerBox1.instance.Hide();
        AnswerBox2.instance.Hide();
        AnswerBox3.instance.Hide();
        AnswerBox4.instance.Hide();
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
            /*
            WriteInCorrectBox(textImporter.GetCurrentLine());/////////////////
            if (!textImporter.AdvanceToNextLine())
            {
                isLastInteraction = true;
            }
            */
            DisableAnswerBoxes();
            if (isFreshLine)
            {
                textImporter.ParseLine();
                isFreshLine = false;
            }
            TextImporter.TextType sentenceType = textImporter.GetSentenceType();
            if (sentenceType == TextImporter.TextType.Regular)
            {
                HandleRegularText();
            }
            else if (sentenceType == TextImporter.TextType.Answer)
            {
                HandleAnswerText();
            }

            if (!textImporter.AdvanceToNextSentence())
            {
                isLastInteraction = true;
                isFreshLine = true;
            }
        }
    }

    void HandleAnswerText()
    {
        //Write in answers box
        string sentenceToShow = textImporter.GetSentence();
        do
        {
            WriteInCorrectAnswerBox(sentenceToShow);
            sentenceToShow = textImporter.GetNextAnswer();
        } while (sentenceToShow != null); //We want all the answers to load at once
        
    }

    void HandleRegularText()
    {
        WriteInCorrectBox(textImporter.GetSentence());
    }

    public void WriteInCorrectAnswerBox(string sentenceToShow)
    {
        if (!AnswerBox1.instance.GetComponent<Image>().enabled)
        {
            AnswerBox1.instance.Show(sentenceToShow);
            AnswerBox1.instance.Select();
        }
        else if (!AnswerBox2.instance.GetComponent<Image>().enabled)
        {
            AnswerBox2.instance.Show(sentenceToShow);
            AnswerBox2.instance.Select();
        }
        else if (!AnswerBox3.instance.GetComponent<Image>().enabled)
        {
            AnswerBox3.instance.Show(sentenceToShow);
            AnswerBox3.instance.Select();
        }
        else if (!AnswerBox4.instance.GetComponent<Image>().enabled)
        {
            AnswerBox4.instance.Show(sentenceToShow);
            AnswerBox4.instance.Select();
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
