using UnityEngine;
using System.Collections;

public class UpperDialogBox : DialogBox {

    public static UpperDialogBox instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public override void Show(string displayText)
    {
        base.Show(displayText);
    }

    public override void Hide()
    {
        base.Hide();
    }
}
