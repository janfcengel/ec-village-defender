using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlacksmithController : NPCBehaviour
{
    public GameObject Player;
    [SerializeField]
    public List<DialogueTrigger> dialogues;

    public Button talkToNPC; 

    
    void Start()
    {
        activeDialogue = (DialogueTrigger) dialogues.ToArray().GetValue(1); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player)
        {
            Debug.Log("Enter blacksmith");
            talkToNPC.gameObject.SetActive(true);
            DialogueManager.instance.SetActiveNPC(this); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Player)
        {
            Debug.Log("Exit blacksmith");
            talkToNPC.gameObject.SetActive(false);
        }
    }
}
