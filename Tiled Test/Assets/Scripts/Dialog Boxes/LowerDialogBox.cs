using UnityEngine;
using System.Collections;

public class LowerDialogBox : DialogBox {

    public static LowerDialogBox instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
