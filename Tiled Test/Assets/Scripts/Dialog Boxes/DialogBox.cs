using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DialogBox : MonoBehaviour {

    public virtual void Show(string displayText)
    {
        GetComponent<Image>().enabled = true;

        Text textComponent = GetComponentInChildren<Text>();
        textComponent.enabled = true;
        textComponent.text = displayText;
    }

    public virtual void Hide()
    {
        GetComponent<Image>().enabled = false;
        GetComponentInChildren<Text>().enabled = false;
    }
}
