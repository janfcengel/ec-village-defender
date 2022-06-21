using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text dialogueName;
    public Text dialogueText;
    public GameObject dialogueContinueButton;
    public GameObject dialogueBox;
    private Queue<string> sentences;

    public static DialogueManager instance;

    private NPCBehaviour activeNPC;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this; 
    }

    private void Start()
    {
        sentences = new Queue<string>(); 
    }

    public void StartDialogue(Dialogue dialogue)
    {
        SetDialogueUIElementsActive(true);
        dialogueName.text = dialogue.name;
        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void StartDialogueFromActiveNPC(Dialogue dialogue)
    {
        activeNPC.StartDialogueTrigger();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            activeNPC.DeactivateActionButton();
            return; 
        }
        if (sentences.Count == 1)
        {
            activeNPC.ActivateActionButton();
        }
        string sentence = sentences.Dequeue();

        dialogueText.text = sentence; 
    }

    public void EndDialogue()
    {
        Debug.Log("End Dialogue");
        SetDialogueUIElementsActive(false);
    }

    public void SetActiveNPC(NPCBehaviour npcBehaviour)
    {
        activeNPC = npcBehaviour; 
    }

    private void SetDialogueUIElementsActive(bool isActive)
    {
        dialogueBox.SetActive(isActive);
        dialogueContinueButton.SetActive(isActive);
        dialogueText.gameObject.SetActive(isActive);
        dialogueName.gameObject.SetActive(isActive);
    }
}
