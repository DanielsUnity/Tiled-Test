using UnityEngine;
using System.Collections;

public class AnswerBox4 : AnswerBoxBase
{

    public static AnswerBox4 instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

}
