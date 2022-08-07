using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarSystem : MonoBehaviour
{
    private Transform bar;
    [SerializeField]
    GameObject barSprite;

    //[SerializeField] [Range(0f, 1f)] float lerpTime;

    Color color1 = new Color(164, 0, 0);
    Color color2 = new Color(245, 88, 88);

    Image image; 

    [SerializeField]
    int maxHealth;
    private int currentHealth;
    bool muteX;
    // Start is called before the first frame update
    void Start()
    {
        bar = transform.Find("Bar");
        currentHealth = maxHealth;
        if(maxHealth == 0)
        {
            maxHealth = 1; // Geteilt durch 0 vermeiden 
        }
        image = barSprite.GetComponent<Image>();
    }

    private void Update()
    {
        if(!muteX)
        {
            if(currentHealth < 30)
            {
                StartCoroutine("Pulse", 0.5f);
            }
            else
            {
                StartCoroutine("Pulse", 1f);
            }
        }

        if (currentHealth == 0)
        {
            GameOverBehaviour.instance.OnGameOver();
        }
       
        //Pulse();
    }
    // Update is called once per frame
    void LateUpdate()
    {
        float fPercentHealth = Convert.ToSingle(currentHealth) / Convert.ToSingle(maxHealth);
        //float fPercentHealth = Convert.ToSingle(percentHealth);
        if(fPercentHealth == null)
        {
            fPercentHealth = maxHealth;
        }
        SetSize(fPercentHealth);
    }

    public void SetSize(float percentSize)
    {
        if(bar == null) { return; }
        bar.localScale = new Vector3(percentSize, 1f);
    }

    public void ReduceHealth(int reducing)
    {
        currentHealth -= reducing;
        if(currentHealth < 0)
        {
            currentHealth = 0;
        }
    }

    public void AddHealth(int adding)
    {
        currentHealth += adding;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth; 
        }
    }

    public void SetHealth(int setTo)
    {
        currentHealth = setTo;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public int GetMaxHealth()
    {
        return maxHealth; 
    }
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
    
    IEnumerator Pulse(float time)
    {
        muteX = true;
        //new Color(212, 63, 63, 1), 1f
        if (image.color == color2)
        {
            image.color = color1;
        }
        else
        {
            image.color = color2; 
        }
        //Debug.Log("Color: r" + image.color.r + " g" + image.color.g + " b" + image.color.b);
       
        //Debugs the right color but puts it anyways on (248,248,248)...
        
        yield return new WaitForSeconds(time);
        muteX = false; 
    }
}
