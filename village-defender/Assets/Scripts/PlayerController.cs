using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private float speed = 0.1f;
    [SerializeField]
    HealthBarSystem healthBar;

    public Quest currentQuest;

    public Animator animator;
    Vector3 moveDirection;
    public Rigidbody rigidbody;
    public float rotationFactor = 4f;

    public ParticleSystem footstepPS;

    private bool _deubgMode = false; 
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
        if(CanvasController.GetBuildingMode())
        {
            moveDirection = Vector3.zero;
            animator.SetBool("isMoving", false);
            return; 
        }

        float xDirection = Input.GetAxis("Horizontal");
        float zDirection = Input.GetAxis("Vertical");

        moveDirection = new Vector3(xDirection, 0f, zDirection);

        if (moveDirection.magnitude > 1)
        {
            moveDirection = moveDirection.normalized;
        }

        if(moveDirection != Vector3.zero)
        {
            animator.SetBool("isMoving", true);
            triggerFootsteps();
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            InventoryUIManager.instance.ToggleVisible();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            QuestUIManager.instance.ToggleVisible();
        }
        else if (Input.GetKeyDown(KeyCode.H) && _deubgMode)
        {
            healthBar.AddHealth(10);
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            healthBar.ReduceHealth(10);
        }
        else if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            animator.SetTrigger("OnPunch");
        }

        if(getHealthBar().GetCurrentHealth() == 0)
        {
            
            animator.SetBool("isDead", true);
        }
        handleRotation();
    }

    private void FixedUpdate()
    {
        Debug.Log(rigidbody.position);
        rigidbody.drag = 0;
        transform.position += moveDirection * speed;
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
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
