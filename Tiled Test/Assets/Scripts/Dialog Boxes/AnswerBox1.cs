using UnityEngine;
using System.Collections;

public class AnswerBox1 : AnswerBoxBase
{

    public static AnswerBox1 instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

}
