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

    public Animator animator;
    Vector3 moveDirection;

    public float rotationFactor = 4f;

    public ParticleSystem footstepPS; 
    // Start is called before the first frame update
    void Start()
    {
        healthBar.SetSize(.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetBool("isDead"))
        {
            return;
        }

        float xDirection = Input.GetAxis("Horizontal");
        float zDirection = Input.GetAxis("Vertical");
        
        moveDirection = new Vector3(xDirection, 0f, zDirection);

        if(moveDirection != Vector3.zero)
        {
            animator.SetBool("isMoving", true);
            triggerFootsteps();
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

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
        else if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            //animator.SetBool("isGathering", true);
            animator.SetTrigger("OnPunch");
        }

        if(getHealthBar().GetCurrentHealth() == 0)
        {
            
            animator.SetBool("isDead", true);
            //this.enabled = false; 
        }
        handleRotation();
    }

    void handleRotation()
    {
        Vector3 posLookAt;

        posLookAt.x = moveDirection.x;
        posLookAt.y = 0f;
        posLookAt.z = moveDirection.z;

        Quaternion currentRotation = transform.rotation;

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(posLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactor * Time.deltaTime);
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

    public Animator GetAnimator()
    {
        return animator;
    }

    public void triggerFootsteps()
    {
        footstepPS.transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);

        footstepPS.Play(); 
    }
}
