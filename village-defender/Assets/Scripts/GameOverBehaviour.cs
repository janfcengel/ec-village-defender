using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverBehaviour : MonoBehaviour
{
    public static GameOverBehaviour instance; 
    public Transform box;
    public CanvasGroup background; 


    public void Awake()
    {
        instance = this; 
    }

    public void Start()
    {
        background.alpha = 0;
        box.localScale = Vector2.zero; 
    }

    // Start is called before the first frame update
    public void OnGameOver()
    {
        
        background.LeanAlpha(0.9f, 2f);

        box.LeanScale(Vector2.one, 1f).delay = 2f; 

    }
}
