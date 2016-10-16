using UnityEngine;
using System.Collections;

public class AnswerBoxButton : MonoBehaviour {

    public delegate void AnswerBoxDelegate(int boxNumber);
    public static event AnswerBoxDelegate OnAnswerPressed;

    [Range(1, 4)]
    public int boxNumber = 1;

    public void OnClick()
    {
        if (OnAnswerPressed != null)
        {
            OnAnswerPressed(boxNumber);
        }
    }
}
