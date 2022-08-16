using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Yarn;
using Yarn.Unity;

public class DialogueEventSystem : MonoBehaviour
{

    [System.Serializable]
    public class PositionHolder
    {
        public GameObject npc;
        public Transform npcInitialPosition;
        public Transform npcFinalPosition;
    }

    public List<PositionHolder> npcPositionToScreen = new List<PositionHolder>();

    [Tooltip("list of npc, The event order is based on this list [1st event = 1st npc]")] 
    [SerializeField] private List<ScriptableEventSystem> eventQueue;

    [SerializeField] private GameObject lemonadeJar;

    public new Camera camera;
    private bool MouseOnLemonade { get; set;}
    private bool LemonadeJarPressed { get; set; }
    
    // The dialogue runner we want to load the program into
    private DialogueRunner _dialogueRunner;
    //Keeping track of the character name
    public TextMeshProUGUI characterName;
    //check previous speaker
    private string _previousSpeaker;
    //bool to avoid async func
    private bool _callItOnce;
    //place holders to destroy 
    private GameObject[] spawnHolder = {null, null};
    
    //jar outlined or not
    private Renderer _jarRend;

    public GameObject immortalObject;
    [SerializeField] private GameObject eventSystem;

    // Start is called before the first frame update
    void Start()
    {
        //Defining stuff
        _dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
        MouseOnLemonade = false;
        _jarRend = lemonadeJar.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //keep track of mouse over lemonade
        if (!_dialogueRunner.IsDialogueRunning)
        {
           CheckForMouseOnJar(); 
        }

        if (eventQueue.Count == 0)
        {
            EndGame();
        }
    }

    void CheckForMouseOnJar()
    {
        //Simple raycast, once find the lemonade Jar. do something
        RaycastHit2D hit = Physics2D.Raycast(camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit == lemonadeJar)
        {
            MouseOnLemonade = true;
            _jarRend.material.SetFloat("_OutlineWidth", 5);
            if (Input.GetMouseButtonDown(0) && MouseOnLemonade)
            {
                LemonadeJarPressed = true;
                CallForEvent();
                _jarRend.material.SetFloat("_OutlineWidth", 0);
                MouseOnLemonade = false;
            }
        }
        else
        {
            MouseOnLemonade = false;
            _jarRend.material.SetFloat("_OutlineWidth", 0);
        }
        
    }

    void CallForEvent()
    {
        //Just a start conversation + spawn npc.
        if (!_dialogueRunner.IsDialogueRunning && eventQueue.First().eventNameToStart != null)
        {
            _dialogueRunner.StartDialogue(eventQueue.First().eventNameToStart);
            CheckNpcConversation();
            _callItOnce = true;
        }
    }
    public void BoolNextDialogueChart()
    {
        //being called by continue button. I prefer this than a async call. xD
        _callItOnce = true;
    }
    public void CheckNpcConversation()
    {
        //This is being call on the Line view (event function character typed . That is called a few times ... )
        
        //Check for new character name. If true -> spawn it where's available .
        if (_callItOnce)
        {
            //No npc yet on this holder. Spawn it
            if (npcPositionToScreen[0].npc == null)
            {
                for(int i = 0; i < eventQueue.First().Variables.Count; i++)
                {
                    if (characterName.text == eventQueue.First().Variables[i].Npc.name)
                    {
                        SpawnDestroyNpc(i, 0);
                        break;
                    }
                }
            }
            //npc on previous holder but not this one. Spawn it
            else if(npcPositionToScreen[1].npc == null && npcPositionToScreen[0].npc.name != characterName.text)
            {
                for(int i = 0; i < eventQueue.First().Variables.Count; i++)
                {
                    if (characterName.text == eventQueue.First().Variables[i].Npc.name)
                    {
                        SpawnDestroyNpc(i, 1);
                        break;
                    }
                }
            }
            //npc everywhere OMG. Get the first that spoke so it can have the previous + new npc
            else
            {
                if (npcPositionToScreen[0].npc.name != characterName.text &&
                    npcPositionToScreen[1].npc.name != characterName.text)
                {
                    for (int i = 0; i < eventQueue.First().Variables.Count; i++)
                    {
                        if (characterName.text == eventQueue.First().Variables[i].Npc.name)
                        {
                            if (npcPositionToScreen[0].npc.name == _previousSpeaker)
                            {
                                SpawnDestroyNpc(i, 1);
                            }
                            else
                            {
                                SpawnDestroyNpc(i, 0);
                            }
                        }
                    }
                }
            }
        }
        _callItOnce = false;
        _previousSpeaker = characterName.text;
        
    }

    void EndGame()
    {
        eventSystem.GetComponent<YarnCommands>().SendDataToTextFile();
        immortalObject.GetComponent<DontDestroyOnLoadScript>().preserveData(eventSystem.GetComponent<YarnCommands>());
        SceneManager.LoadScene("EndResultScene");
    }

    public void EndEvent()
    {
        Destroy(spawnHolder[0]);
        Destroy(spawnHolder[1]);
        eventQueue.Remove(eventQueue.First());
        npcPositionToScreen[0].npc = null;
        npcPositionToScreen[1].npc = null;
    }

    void SpawnDestroyNpc(int forInt, int leftOrRight)
    {
        // 0 is left | 1 is right (Just because spawnAholder
        if (spawnHolder[leftOrRight] != null)
        {
            MoveNpcAtSpawn(leftOrRight, true);
        }
        spawnHolder[leftOrRight] = Instantiate(eventQueue.First().Variables[forInt].Npc, npcPositionToScreen[leftOrRight].npcInitialPosition);
        npcPositionToScreen[leftOrRight].npc = eventQueue.First().Variables[forInt].Npc;
        
        //if npc is on the right, change scale to -1 so It looks at the right direction
        if (leftOrRight == 1)
        {
            spawnHolder[leftOrRight].transform.localScale = new Vector3(-spawnHolder[leftOrRight].transform.localScale.x, spawnHolder[leftOrRight].transform.localScale.y, spawnHolder[leftOrRight].transform.localScale.z);
        }
        
        MoveNpcAtSpawn(leftOrRight, false);
    }

    void MoveNpcAtSpawn(int leftOrRight, bool oldNpc)
    {
        //check for old character 
        if (oldNpc)
        {
            spawnHolder[leftOrRight].GetComponent<NpcScript>().positionToBe =
                npcPositionToScreen[leftOrRight].npcInitialPosition.transform.position;
            spawnHolder[leftOrRight].GetComponent<NpcScript>().npcToBeDestroyed = true;
        }
        else
        {
            spawnHolder[leftOrRight].GetComponent<NpcScript>().positionToBe =
                npcPositionToScreen[leftOrRight].npcFinalPosition.transform.position;
        }
        
    }
    
}
