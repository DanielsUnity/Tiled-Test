using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AnswerBoxBase : DialogBox {

    public virtual void Select()
    {
        GetComponent<Image>().color = Color.gray;
    }

    public virtual void Deselect()
    {
        GetComponent<Image>().color = Color.white;
    }
}
