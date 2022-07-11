using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoadScript : MonoBehaviour
{
    public void preserveData(YarnCommands commands)
    {
        YarnCommands.Choices data = new YarnCommands.Choices();
        data = commands.choices;
        Debug.Log(data.Teamwork);
    }
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
