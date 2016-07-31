using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(DialogManager))]
public class TextImporter : MonoBehaviour {

    public TextAsset rawTextFile;

    private string[] rawTextLines;
    private string[] rawLineSentences;
    private List<SentenceStruct> currentLineSentences = new List<SentenceStruct>();
    private int sentenceIndex = 0;

    private int currentLine = 0;
    private int currentStartLine = 0;//TODO Figure out how to write different messages in different sections of the game (Maybe writing the files with [Chapter X] and [State X])
    private int currentEndLine;
    private int endLine;

    private DialogManager dialogManager;
    
    public enum TextType
    {
        Regular,
        Answer,
        Reaction
    }

    private struct SentenceStruct
    {
        string sentence;
        TextType type;

        public SentenceStruct(string sentence, TextType type)
        {
            this.sentence = sentence;
            this.type = type;

        }

        public string Sentence
        {

            get { return sentence; }
            set { sentence = value; }

        }

        public TextType Type
        {
            get { return type; }
            set { type = value; }

        }
    }

    void Awake()
    {
        dialogManager = GetComponent<DialogManager>();
    }

	void Start () {
        if (rawTextFile)
        {
            rawTextLines = rawTextFile.text.Split('\n');
            endLine = rawTextLines.Length - 1;
            currentEndLine = endLine;
        }
        else
        {
            Debug.LogError("No text to import",this);
        }
	}

    public void ParseLine()
    {
        rawLineSentences = rawTextLines[currentLine].Split(';');
        currentLineSentences.Clear();
        foreach (string sentence in rawLineSentences)
        {
            if (!sentence.StartsWith(">"))
            {
                //Regular sentence
                currentLineSentences.Add(new SentenceStruct(sentence,TextType.Regular)); 
            }
            else
            {
                //Another type of sentence
                if (sentence.StartsWith(">Answer"))
                {
                    //TODO There should be an answer-state matching around here
                    int index = sentence.IndexOf(":");
                    string formattedSentence = sentence.Substring(index + 2);
                    currentLineSentences.Add(new SentenceStruct(formattedSentence, TextType.Answer));
                }
                if (sentence.StartsWith(">Reaction"))
                {
                    int index = sentence.IndexOf(":");
                    string formattedSentence = sentence.Substring(index + 2);
                    currentLineSentences.Add(new SentenceStruct(formattedSentence, TextType.Reaction));
                }
            }
        }
    }

    public TextType GetSentenceType()
    {
        return currentLineSentences[sentenceIndex].Type;
    }

    public string GetSentence()
    {
        return currentLineSentences[sentenceIndex].Sentence;
    }

    public bool AdvanceToNextSentence()
    {
        if (currentLineSentences.Count > sentenceIndex + 1)
        {
            sentenceIndex++;
            return true;
        }
        else
        {
            //TODO Decide if we want to keep repeating the line or advance to next (it may be done indirectly with states)
            sentenceIndex = 0;
            return false;
        }
    }

    public string GetNextAnswer(out bool isLastInteraction)
    {
        isLastInteraction = false;
        if (currentLineSentences.Count > sentenceIndex + 1)//If there's at least another sentence
        {
            if (currentLineSentences[sentenceIndex + 1].Type == TextType.Answer)//if it is an answer
            {
                sentenceIndex++;
                return currentLineSentences[sentenceIndex].Sentence;
            }
        }
        else
        {
            sentenceIndex = 0;
            isLastInteraction = true;
        }
        return null;//If there aren't more sentences or the next one is not an answer
    }

    public string GetCorrectReaction(int answerSelected)
    {
        //We assume all remaining sentences are reactions
        for (int i = 0; i < 4; i++)
        {
            if (i == answerSelected)
            {
                int aux = sentenceIndex;
                sentenceIndex = 0;
                return currentLineSentences[aux + i].Sentence;
            }
        }
        Debug.LogWarning("There's no matching reaction to selected answer");
        return null;
    }

    public string GetCurrentLine()
    {
        return rawTextLines[currentLine];
    }

    public bool AdvanceToNextLine()
    {
        if (currentLine >= currentEndLine)
        {
            ResetToFirstLine();
            return false;
        }
        else
        {
            currentLine++;
            return true;
        }
    }

    public void ResetToFirstLine()
    {
        currentLine = currentStartLine;
    }
}
