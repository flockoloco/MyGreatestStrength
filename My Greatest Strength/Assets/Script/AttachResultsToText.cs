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
        _immortableObject = GameObject.Find("InvencibleGameObject").GetComponent<DontDestroyOnLoadScript>();
        Debug.Log(_immortableObject.data.Gratitude);
        UpdateNamesToTheirValues();
    }

    private void UpdateNamesToTheirValues()
    {
        dataTime.text = DateTime.Now.ToString();

        bunchOfText[0].text = _immortableObject.data.Gratitude.ToString() ?? "0";
        bunchOfText[1].text = _immortableObject.data.Bravery.ToString() ?? "0";
        bunchOfText[2].text = _immortableObject.data.Curiosity.ToString() ?? "0";
        bunchOfText[3].text = _immortableObject.data.Prudence.ToString() ?? "0";
        bunchOfText[4].text = _immortableObject.data.Teamwork.ToString() ?? "0";
        bunchOfText[5].text = _immortableObject.data.Kindness.ToString() ?? "0";
    }
}
