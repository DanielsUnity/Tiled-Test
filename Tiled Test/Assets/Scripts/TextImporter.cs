using UnityEngine;
using System.Collections;

public class TextImporter : MonoBehaviour {

    public TextAsset textFile;
    public string[] textLines;

    private int currentLine = 0;
    private int currentStartLine = 0;//TODO Figure out how to write different messages in different sections of the game (Maybe writing the files with [Chapter X] and [State X])
    private int currentEndLine;

	void Start () {
        if (textFile)
        {
            textLines = textFile.text.Split('\n');
            currentEndLine = textLines.Length - 1;
        }
        else
        {
            Debug.LogError("No text to import",this);
        }
	}

    public string GetCurrentLine()
    {
        return textLines[currentLine];
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
