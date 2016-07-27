using UnityEngine;
using System.Collections;

public class AnswerBox3 : AnswerBoxBase
{

    public static AnswerBox3 instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


}
