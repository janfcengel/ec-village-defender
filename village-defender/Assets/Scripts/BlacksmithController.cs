using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlacksmithController : NPCBehaviour
{
    public GameObject Player;
    [SerializeField]
    public List<DialogueTrigger> dialogues; //0: quest start, 1: normal, 2: quest during

    public Button talkToNPC;

    public Button actionButton; 

    public List<Quest> availableQuest;

    private Quest currentQuest;

    void Start()
    {
        
        if (availableQuest.Count > 0)
        {
            activeDialogue = (DialogueTrigger)dialogues.ToArray().GetValue(0);
            Debug.Log("Quest da");
        }
        else
        {
            activeDialogue = (DialogueTrigger)dialogues.ToArray().GetValue(1);
            Debug.Log("Quest nicht da");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player)
        {
            talkToNPC.gameObject.SetActive(true);
            DialogueManager.instance.SetActiveNPC(this); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Player)
        {
            talkToNPC.gameObject.SetActive(false);
        }
    }

    public override void ActivateActionButton()
    {
        Debug.Log("testactviate");
        if( activeDialogue == (DialogueTrigger)dialogues.ToArray().GetValue(0) ) // vllt 1
        {
            actionButton.gameObject.SetActive(true);
            Text actionButtontext = actionButton.gameObject.GetComponentInChildren<Text>();
            actionButtontext.text = "Accept Quest";
        }
    }

    public void OnActionButton()
    {
        if (activeDialogue == (DialogueTrigger)dialogues.ToArray().GetValue(0))
        {
            OnQuestAccept();
        }
    }

    public void OnQuestAccept()
    {
        currentQuest = (Quest) availableQuest.ToArray().GetValue(0);
        PlayerController pc = Player.gameObject.GetComponent<PlayerController>();
        pc.SetQuest(currentQuest); 
    }

    public override void DeactivateActionButton()
    {
        actionButton.gameObject.SetActive(false);
    }


}
