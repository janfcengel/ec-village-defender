using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameOverBehaviour : MonoBehaviour
{
    public static GameOverBehaviour instance; 
    public Transform box;
    public CanvasGroup background;
    public Button button; 

    public void Awake()
    {
        instance = this; 
    }

    public void Start()
    {
        background.alpha = 0;
        box.localScale = Vector2.zero;
        background.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    public void OnGameOver()
    {
        background.gameObject.SetActive(true);
        button.gameObject.SetActive(true);
        background.LeanAlpha(0.9f, 2f);
        box.LeanScale(Vector2.one, 1f).delay = 2f; 
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
