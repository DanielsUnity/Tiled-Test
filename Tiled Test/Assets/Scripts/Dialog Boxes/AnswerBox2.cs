using UnityEngine;
using System.Collections;

public class AnswerBox2 : AnswerBoxBase
{

    public static AnswerBox2 instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

}
