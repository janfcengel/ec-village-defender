using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarSystem : MonoBehaviour
{
    private Transform bar;

    [SerializeField]
    int maxHealth;
    private int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        bar = transform.Find("Bar");
        currentHealth = maxHealth;
        Debug.Log("curr: " + currentHealth);
        if(maxHealth == 0)
        {
            maxHealth = 1; // Geteilt durch 0 vermeiden 
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float fPercentHealth = Convert.ToSingle(currentHealth) / Convert.ToSingle(maxHealth);
        //float fPercentHealth = Convert.ToSingle(percentHealth);
        SetSize(fPercentHealth);
    }

    public void SetSize(float percentSize)
    {
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
}
