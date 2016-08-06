using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(DialogManager))]
public class TextImporter : MonoBehaviour {

    public TextAsset rawTextFile;

    private List<SentenceStruct> currentLineSentences = new List<SentenceStruct>();
    private int sentenceIndex = 0;
    private Dictionary<string, List<SentenceStruct>> stateToSentencesDictionary = new Dictionary<string, List<SentenceStruct>>();

    //TODO Figure out how to write different messages in different sections of the game (Maybe writing the files with [Chapter X] and [State X])

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
        string state;

        public SentenceStruct(string sentence, TextType type)
        {
            this.sentence = sentence;
            this.type = type;
            this.state = null;
        }

        public SentenceStruct(string sentence, TextType type, string state)
        {
            this.sentence = sentence;
            this.type = type;
            this.state = state;
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

        public string State
        {
            get { return state; }
            set { state = value; }

        }
    }

	void Start () {
        if (rawTextFile)
        {
            string[] rawTextLines = rawTextFile.text.Split('\n');
            
            foreach (string rawTextLine in rawTextLines)
            {
                ParseLine(rawTextLine);
            }
            
        }
        else
        {
            Debug.LogError("No text to import",this);
        }
	}

    private void ParseLine(string rawTextLine)
    {
        string[] rawLineSentences = rawTextLine.Split(';');
        string formattedState = null;
        if (rawLineSentences[0].StartsWith(">State"))
        {
            int index = rawLineSentences[0].IndexOf(":");
            formattedState = rawLineSentences[0].Substring(index + 2);
            stateToSentencesDictionary.Add(formattedState, new List<SentenceStruct>());
        }
        else
        {
            Debug.LogError("All lines in the text file should start with the corresponding state", this);
        }

        foreach (string sentence in rawLineSentences)
        {
            if (!sentence.StartsWith(">"))
            {
                //Regular sentence
                stateToSentencesDictionary[formattedState].Add(new SentenceStruct(sentence,TextType.Regular)); 
            }
            else
            {
                //Another type of sentence
                if (sentence.StartsWith(">Answer"))
                {
                    int index = sentence.IndexOf(":");
                    string formattedSentence = sentence.Substring(index + 2);
                    stateToSentencesDictionary[formattedState].Add(new SentenceStruct(formattedSentence, TextType.Answer));
                }

                if (sentence.StartsWith(">Reaction"))
                {
                    int index = sentence.IndexOf(":");
                    string formattedSentence = sentence.Substring(index + 2);
                    int index2 = sentence.IndexOf("/");//Get the string from "/ " to ":"
                    string state = sentence.Substring(index2 + 2, index - index2 - 2);
                    stateToSentencesDictionary[formattedState].Add(new SentenceStruct(formattedSentence, TextType.Reaction, state));
                }
            }
        }
    }

    public bool SetCurrentLine(string state)
    {
        if (stateToSentencesDictionary.ContainsKey(state))
        {
            Debug.Log("Getting state: " + state);
            currentLineSentences = stateToSentencesDictionary[state];//currentLineSentences is really a pointer, so if we clear it we are clearing also the dictionary
            sentenceIndex = 0;
            Debug.Log(currentLineSentences[0].Sentence);
            return true;
        }
        else
        {
            Debug.Log("The current state doesn't have a matching line in the text file (can be intentional). State: " + state, this);
            return false;
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

    public string GetCorrectReaction(int answerSelected, out string reactionState)
    {
        //We assume all remaining sentences are reactions
        reactionState = null;
        for (int i = 0; i < 4; i++)
        {
            if (i == answerSelected)
            {
                int aux = sentenceIndex;
                sentenceIndex = 0;
                if (currentLineSentences[aux + i].State != null)
                {
                    reactionState = currentLineSentences[aux + i].State;
                }
                return currentLineSentences[aux + i].Sentence;
            }
        }
        Debug.LogWarning("There's no matching reaction to selected answer");
        return null;
    }
}
