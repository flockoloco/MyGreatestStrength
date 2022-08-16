using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class YarnCommands : MonoBehaviour
{
    public class Choices
    {
        public int Gratitude;
        public int Bravery;
        public int Curiosity;
        public int Prudence;
        public int Teamwork;
        public int Kindness;
    }
    
    public Choices choices = new Choices();

    public GameObject textBackground;
    public Sprite[] setOfImages;

    public new Camera camera;
    public GameObject[] liquidAnimation;
    public GameObject[] valveAnimation;
    public AudioSource[] soundEffects;

    public bool canBePlayed;

    //get length
    public AnimationClip lemonadeAnimation;

    [YarnCommand("save")]
    public void Save(string choiceName, int choiceAmount)
    {
        switch (choiceName)
        {
            case "gratitude":
                choices.Gratitude = choiceAmount + choices.Gratitude;
                break;
            case "bravery":
                choices.Bravery = choiceAmount + choices.Bravery;
                break;
            case "curiosity":
                choices.Curiosity = choiceAmount + choices.Curiosity;
                break;
            case "prudence":
                choices.Prudence = choiceAmount + choices.Prudence;
                break;
            case "teamwork":
                choices.Teamwork = choiceAmount + choices.Teamwork;
                break;
            case "kindness":
                choices.Kindness = choiceAmount + choices.Kindness;
                break;
            default:
                Debug.LogWarning("Wrong value was written!");
                break;
        }
        
        StartCoroutine(fillCup());
    }

    private IEnumerator fillCup()
    {
        yield return new WaitForSeconds(2.0f);
    }

    [YarnCommand("moodBoard")]
    public void changeMoodBoard(string mood)
    {
        switch (mood)
        {
            case "angry":
                textBackground.GetComponent<Image>().sprite = setOfImages[0];
                break;
            case "happy":
                textBackground.GetComponent<Image>().sprite = setOfImages[1];
                break;
            case "narrator":
                textBackground.GetComponent<Image>().sprite = setOfImages[2];
                break;
            default:
                Debug.LogWarning("Wrong mood for text background was written!");
                break;
        }
    }
    
    public void SendDataToTextFile()
    {
        if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/ChoicesScoreboard.txt"))
        {
            string json = JsonUtility.ToJson(choices);
            File.AppendAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/ChoicesScoreboard.txt", DateTime.Now + " " + json + "\n");
        }
        else
        {
            string json = JsonUtility.ToJson(choices);
            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/ChoicesScoreboard.txt",  DateTime.Now + " " + json + "\n");
        }
    }

    public IEnumerator CupChoices(int choiceID)
    {
        if (choiceID.Equals(0))
        {
            soundEffects[0].Play();
            soundEffects[1].Play();

            valveAnimation[0].GetComponent<Animation>().Play("Rotate");
            yield return new WaitForSeconds(lemonadeAnimation.length);
        }
        else if(choiceID.Equals(1))
        {
            soundEffects[0].Play();
            soundEffects[1].Play();

            valveAnimation[1].GetComponent<Animation>().Play("Rotate");
            //both lemonade animation have the same length duration so should be alright 
            yield return new WaitForSeconds(lemonadeAnimation.length);
        }
        else
        {
            Debug.LogWarning("Error during the choice ID");
        }
        
    }
    
}
