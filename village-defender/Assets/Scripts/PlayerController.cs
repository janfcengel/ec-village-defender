using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private float speed = 0.1f;
    //public GameObject inventory;
    [SerializeField]
    HealthBarSystem healthBar;

    public Quest currentQuest; 
    // Start is called before the first frame update
    void Start()
    {
        healthBar.SetSize(.5f);
    }

    //private bool invUIVisible;
    //private bool questUIVisible;
    // Update is called once per frame
    void Update()
    {
        float xDirection = Input.GetAxis("Horizontal");
        float zDirection = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(xDirection, 0f, zDirection);

        transform.position += moveDirection * speed;

        if (Input.GetKeyDown(KeyCode.E))
        {
            InventoryUIManager.instance.ToggleVisible();
            //invUIVisible = !invUIVisible;
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            QuestUIManager.instance.ToggleVisible();
            //questUIVisible = !questUIVisible;
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            healthBar.AddHealth(10);
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            healthBar.ReduceHealth(10);
        }

    }

    public HealthBarSystem getHealthBar()
    {
        return healthBar; 
    }

    public void SetQuest(Quest quest)
    {
        currentQuest = quest;
        QuestUIManager.instance.SetQuestTexts(quest);
    }

    public Quest GetQuest()
    {
        return currentQuest; 
    }

}
