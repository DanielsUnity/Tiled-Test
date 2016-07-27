using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AnswerBoxBase : DialogBox {

    public virtual void Select()
    {
        GetComponent<Image>().color = Color.green;
    }

    public virtual void Deselect()
    {
        GetComponent<Image>().color = Color.white;
    }

    public virtual void Submit()
    {
        //could return its panel number in the actual instance
    }
}
