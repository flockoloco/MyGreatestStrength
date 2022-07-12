using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class NpcScript : MonoBehaviour
{
    // do not worry ...
    public Vector3 positionToBe;
    public bool npcToBeDestroyed;
    //controls the spped the npc moves
    public float movementSpeed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (positionToBe != null && transform.position != positionToBe)
        {
            transform.position = Vector3.Lerp(transform.position, positionToBe, movementSpeed * Time.deltaTime);
        }

        if (npcToBeDestroyed)
        {
            Destroy(gameObject);
        }
    }

    [YarnCommand("disable")]
    public void DisableChat()
    {
        Debug.Log("is jumping!");
    }
}
