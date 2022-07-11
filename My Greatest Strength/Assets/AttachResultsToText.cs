using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AttachResultsToText : MonoBehaviour
{
    private DontDestroyOnLoadScript _immortableObject;
    //order is equal scene hierarchy
    public TextMeshProUGUI[] bunchOfText;

    public TextMeshProUGUI dataTime;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoadScript _immortableObject = GameObject.Find("InvencibleGameObject").GetComponent<DontDestroyOnLoadScript>();
        UpdateNamesToTheirValues();
    }

    private void UpdateNamesToTheirValues()
    {
        dataTime.text = DateTime.Now.ToString();

        bunchOfText[0].text = _immortableObject.data.Gratitude;
        bunchOfText[1].text = _immortableObject.data.Bravery;
        bunchOfText[2].text = _immortableObject.data.Curiosity;
        bunchOfText[3].text = _immortableObject.data.Prudence;
        bunchOfText[4].text = _immortableObject.data.Teamwork;
        bunchOfText[5].text = _immortableObject.data.Kindness;
    }
}
