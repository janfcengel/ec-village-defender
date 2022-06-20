using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text dialogueName;
    public Text dialgoueText;
    private Queue<string> sentences;

    public static DialogueManager instance; 
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
        dialogueName.text = dialogue.name;
        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return; 
        }
        string sentence = sentences.Dequeue();

        dialgoueText.text = sentence; 
    }

    public void EndDialogue()
    {
        Debug.Log("End Dialogue");
    }
}
