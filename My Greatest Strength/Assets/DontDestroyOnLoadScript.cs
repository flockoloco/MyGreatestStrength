using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoadScript : MonoBehaviour
{
    public YarnCommands.Choices data = new YarnCommands.Choices();
    public void preserveData(YarnCommands commands)
    {
        
        data = commands.choices;
        Debug.Log(data.Teamwork);
    }
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
