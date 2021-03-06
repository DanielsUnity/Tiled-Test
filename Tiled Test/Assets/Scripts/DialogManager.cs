using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour {

    public string defaultText;

    private bool isLastInteraction = false;
    private Camera gameCamera;
    private TextImporter textImporter;
    private StateManager stateManager;
    private bool isUsingTextImporter = false;
    private bool isFreshLine = true;
    private bool inAnswerMode = false;
    private int answerSelected = 0;
    private int numberOfAnswers = 4;
    private string reactionState = null;
    private float cameraPlayerOffset = 0;

    void Awake()
    {
        gameCamera = FindObjectOfType<Camera>();
        textImporter = GetComponent<TextImporter>();
        stateManager = GetComponent<StateManager>();
        GameObject mobileUI = GameObject.FindGameObjectWithTag("Mobile UI");
        if (mobileUI)
        {
            float UIScreenVerticalPercentage = mobileUI.GetComponent<RectTransform>().anchorMax.y;
            float UIHeight = gameCamera.orthographicSize * 2 * UIScreenVerticalPercentage;
            float gameplayViewportCenter = (gameCamera.orthographicSize * 2 - UIHeight) / 2;
            cameraPlayerOffset = gameCamera.orthographicSize - gameplayViewportCenter;
        }
    }

    void Start()
    {
        if (textImporter) { isUsingTextImporter = true; }
    }

    void OnEnable()
    {
        AnswerBoxButton.OnAnswerPressed += SelectBoxAndSubmit;
    }

    void OnDisable()
    {
        AnswerBoxButton.OnAnswerPressed -= SelectBoxAndSubmit;
    }

    void SelectBoxAndSubmit(int boxNumber)
    {
        if (inAnswerMode)
        {
            DeselectAnswer(answerSelected + 1);//answerBoxSelected = answerSelected + 1
            answerSelected = boxNumber - 1;//answer selected goes from 0 to 3 and boxNumber from 1 to 4
            SelectAnswer(boxNumber);
            SubmitAnswer();
        }
    }

    void Update()
    {
        if (inAnswerMode)
        {
            if (Input.GetButtonDown("Keyboard Vertical")) //TODO Refer to CharacterBaseController instead
            {
                float vertical = Input.GetAxisRaw("Keyboard Vertical");
                if (vertical > 0)
                {
                    DeselectAnswer(answerSelected + 1);//answerBoxSelected = answerSelected + 1
                    answerSelected = (answerSelected - 1 + numberOfAnswers) % numberOfAnswers;// + numberOfAnswers to avoid negative
                    SelectAnswer(answerSelected + 1);
                }
                else if (vertical < 0)
                {
                    DeselectAnswer(answerSelected + 1);
                    answerSelected = (answerSelected + 1) % numberOfAnswers;
                    SelectAnswer(answerSelected + 1);
                }
            }
        }
    }

    public void Manage()
    {
        if (!isLastInteraction)
        {
            if (!inAnswerMode)
            {
                ShowMessage();
            }
            else
            {
                SubmitAnswer();
            }
        }
        else
        {
            Reset();
        }
    }

    public void Reset()
    {
        isLastInteraction = false;
        isFreshLine = true;
        DeselectAnswer(answerSelected + 1);
        answerSelected = 0;
        DisableBoxes();
        Time.timeScale = 1f;
        GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBehaviorModel>().Unfreeze();

        if (reactionState != null && stateManager)
        {
            stateManager.SetCurrentState(reactionState);
        }
        reactionState = null;
    }


    public void SpecialReset()
    {
        isLastInteraction = false;
        isFreshLine = true;
        DeselectAnswer(answerSelected + 1);
        answerSelected = 0;
        if (reactionState != null && stateManager)
        {
            stateManager.SetCurrentState(reactionState);
        }
        reactionState = null;
    }


    void DisableBoxes()
    {
        //LowerDialogBox.instance.Hide();
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
        GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBehaviorModel>().Freeze();
        Time.timeScale = 0f;
        if (!isUsingTextImporter)
        {
            WriteInCorrectBox(defaultText);
            isLastInteraction = true;
        }
        else
        {
            DisableAnswerBoxes();
            if (isFreshLine)
            {
                if (stateManager)
                {
                    bool hasTheStateSomeDialog = textImporter.SetCurrentLine(stateManager.GetCurrentStateString());
                    if (!hasTheStateSomeDialog)
                    {
                        Reset();
                        return;
                    }
                }
                else
                {
                    Debug.LogError("No state manager found", this);
                }
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
            else if (sentenceType == TextImporter.TextType.Reaction)
            {
                HandleReactionText();
            }

            if (!textImporter.AdvanceToNextSentence())
            {
                isLastInteraction = true;
            }

        }
    }



    void HandleReactionText()
    {
        //As we reset to 0 answerSelected in Reset(), there can be a line of text with one reaction and no answers
        bool continueTalking;
        string sentenceToShow = textImporter.GetCorrectReaction(answerSelected, out reactionState, out continueTalking);
        WriteInCorrectBox(sentenceToShow);
        isLastInteraction = true;
        if (continueTalking)
        {
            SpecialReset();
        }
    }

    void HandleAnswerText()
    {
        //Write in answer boxes
        numberOfAnswers = 0;
        string sentenceToShow = textImporter.GetSentence();
        do
        {
            WriteInCorrectAnswerBox(sentenceToShow);
            sentenceToShow = textImporter.GetNextAnswer(out isLastInteraction);
            numberOfAnswers++;

        } while (sentenceToShow != null); //We want all the answers to load at once

        inAnswerMode = true;
    }

    void DeselectAnswer(int answerBoxSelected)
    {
        switch (answerBoxSelected)
        {
            case 1:
                AnswerBox1.instance.Deselect();
                break;
            case 2:
                AnswerBox2.instance.Deselect();
                break;
            case 3:
                AnswerBox3.instance.Deselect();
                break;
            case 4:
                AnswerBox4.instance.Deselect();
                break;
            default:
                Debug.LogWarning("Answer Box selected out of range, should go from 1 to " + numberOfAnswers + ". Check your code in Update.");
                break;
        }
    }

    void SelectAnswer(int answerBoxSelected)
    {
        switch (answerBoxSelected)
        {
            case 1:
                AnswerBox1.instance.Select();
                break;
            case 2:
                AnswerBox2.instance.Select();
                break;
            case 3:
                AnswerBox3.instance.Select();
                break;
            case 4:
                AnswerBox4.instance.Select();
                break;
            default:
                Debug.LogWarning("Answer Box selected out of range, should go from 1 to " + numberOfAnswers + ". Check your code in Update.");
                break;
        }
    }

    void SubmitAnswer()
    {
        inAnswerMode = false;
        Manage();
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
        }
        else if (!AnswerBox3.instance.GetComponent<Image>().enabled)
        {
            AnswerBox3.instance.Show(sentenceToShow);
        }
        else if (!AnswerBox4.instance.GetComponent<Image>().enabled)
        {
            AnswerBox4.instance.Show(sentenceToShow);
        }
    }

    public void WriteInCorrectBox(string line)
    {
        //if (transform.position.y < gameCamera.transform.position.y + cameraPlayerOffset)
        //{
            UpperDialogBox.instance.Show(line);
        /*}
        else
        {
            LowerDialogBox.instance.Show(line);
        }*/
    }

    public void WriteAlternateMessage(string line)
    {
        if (!isLastInteraction)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBehaviorModel>().Freeze();
            Time.timeScale = 0f;
            WriteInCorrectBox(line);
            isLastInteraction = true;
        }
        else
        {
            Reset();
        }
    }
}

                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    