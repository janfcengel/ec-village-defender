using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBoxBehaviour : MonoBehaviour
{

    bool isOpen;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector2.zero;
        isOpen = false; 
    }

    public void Interact()
    {
        if(isOpen)
        {
            Close();
        }
        else
        {
            Open(); 
        }
    }

    public void Open()
    {
        transform.LeanScale(Vector2.one, 1f);
        isOpen = true; 
    }

    public void Close()
    {
        transform.LeanScale(Vector2.zero, 1f);
        isOpen = false; 
    }
}
