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

    public DialogueTrigger activeDialogue;

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
        if(QuestObserver.instance.GetQuest() == null) { return; }
        if (QuestObserver.instance.GetQuest() != null)
        {
            activeDialogue = (DialogueTrigger)dialogues.ToArray().GetValue(2);
            
        }
        if(QuestObserver.instance.GetQuest().isQuestComplete())
        {
            activeDialogue = (DialogueTrigger)dialogues.ToArray().GetValue(1);
        }
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

    public override void StartDialogueTrigger()
    {
        activeDialogue.TriggerDialogue();
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
        if (activeDialogue == (DialogueTrigger)dialogues.ToArray().GetValue(2)) 
        {
            if (QuestObserver.instance.GetQuest().questGoals[0].isDone == true &&
               QuestObserver.instance.GetQuest().questGoals[1].isDone == true &&
               QuestObserver.instance.GetQuest().isQuestComplete() == false)
            {
                actionButton.gameObject.SetActive(true);
                Text actionButtontext = actionButton.gameObject.GetComponentInChildren<Text>();
                actionButtontext.text = "Give Wood";
                
            }
        }
    }

    public void OnActionButton()
    {
        if (activeDialogue == (DialogueTrigger)dialogues.ToArray().GetValue(0))
        {
            OnQuestAccept();
        }
        if(activeDialogue == (DialogueTrigger)dialogues.ToArray().GetValue(2))
        {
            OnGiveWood();
        }
    }

    public void OnGiveWood()
    {
        QuestObserver.instance.GetQuest().questGoals[2].isDone = true;
        QuestObserver.instance.GetQuest().SetQuestComplete(true);
        QuestUIManager.instance.SetQuestTexts(QuestObserver.instance.GetQuest());
        DeactivateActionButton();
    }
    public void OnQuestAccept()
    {
        currentQuest = (Quest) availableQuest.ToArray().GetValue(0);
        PlayerController pc = Player.gameObject.GetComponent<PlayerController>();
        pc.SetQuest(currentQuest);
        DeactivateActionButton();
    }

    public override void DeactivateActionButton()
    {
        actionButton.gameObject.SetActive(false);
    }


}
