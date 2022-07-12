using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;


public class CompanionBehaviour : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;

    public Animator animationController;

    public FlockController flockController;

    public List<GameObject> flock;

    private AnimatorStateInfo stateInfo;
    private AnimatorStateInfo lastState;
    // Start is called before the first frame update
    void Start()
    {
        stateInfo = animationController.GetCurrentAnimatorStateInfo(0);
        lastState = stateInfo;
        flock = flockController.GetFlock();
    }

    // Update is called once per frame
    void Update()
    {
        bool newState = false;
        
        
        //Vector3 moveDirection = new Vector3(xDirection, 0f, zDirection);

        //transform.position += moveDirection * speed;

        if (Input.GetKeyDown(KeyCode.F1))
        {
            animationController.SetBool("isWandering", false);
            animationController.SetBool("isHunting", false);
            animationController.SetBool("isSeeking", true);
            newState = true;
            stateInfo = animationController.GetCurrentAnimatorStateInfo(0);
            Debug.Log("Current State: Seeking");
        }
        else if(Input.GetKeyDown(KeyCode.F2))
        {
            animationController.SetBool("isHunting", false);
            animationController.SetBool("isSeeking", false);
            animationController.SetBool("isWandering", true);
            newState = true;
            stateInfo = animationController.GetCurrentAnimatorStateInfo(0);
            Debug.Log("Current State: Wandering");
        } 
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            animationController.SetBool("isWandering", false);
            animationController.SetBool("isSeeking", false);
            animationController.SetBool("isHunting", true);
            newState = true;
            stateInfo = animationController.GetCurrentAnimatorStateInfo(0);
            Debug.Log("Current State: Hunting");
        }

        if (getStateName(stateInfo) != getStateName(lastState))
        {
            Debug.Log(getStateName(stateInfo));
        }

        if (newState)
        {
            Vector3 movement = MovementDecision();
            navMeshAgent.SetDestination(movement);
        }
        if(!navMeshAgent.hasPath)
        {
            Vector3 movement = MovementDecision();
            navMeshAgent.SetDestination(movement);
        }

    }

    private Vector3 MovementDecision()
    {
        if(stateInfo.IsName("Wander"))
        {
            return Wander();
        }
        else if(stateInfo.IsName("Hunt"))
        {
            return Hunt();
        }
        else if (stateInfo.IsName("Seek"))
        {
            return Seek();
        }
        return Vector3.zero; 
    }

    private string getStateName(AnimatorStateInfo animatorStateInfo)
    {
        if (animatorStateInfo.IsName("Wander"))
        {
            return "Wander";
        }
        else if (animatorStateInfo.IsName("Hunt"))
        {
            return "Hunt";
        }
        else if (animatorStateInfo.IsName("Seek"))
        {
            return "Seek";
        }
        return "";
    }

    Vector3 lastPos = Vector3.zero;

    private Vector3 Wander()
    {
        if(lastPos == Vector3.zero)
        {
            int phi = Random.Range(0, 1);
            lastPos = transform.position + new Vector3(5 * Mathf.Cos(phi), 0, 5 * Mathf.Sin(phi));
        }
        int newPhi = Random.Range(-10, 10);
        Vector3 newPos = new Vector3(
            lastPos.x * Mathf.Cos(newPhi) - lastPos.y * Mathf.Sin(newPhi),
            0, 
            lastPos.x * Mathf.Sin(newPhi) + lastPos.y * Mathf.Cos(newPhi));
        int walkLength = Random.Range(5, 10);

        lastPos = newPos;
        return transform.position + newPos * walkLength;
    }
    private Vector3 Hunt()
    {
        Dictionary<GameObject, float> flockMap = new Dictionary<GameObject, float>();
        float distanceToCompanion;
        foreach (GameObject f in flock)
        {
            distanceToCompanion = new Vector2(transform.position.x - f.transform.position.x, transform.position.z - f.transform.position.z).magnitude;
            flockMap.Add(f, distanceToCompanion);
        }
        GameObject flockling = flockMap.FirstOrDefault(x => x.Value == flockMap.Values.Min()).Key;

        return flockling.transform.position;
    }
    private Vector3 Seek()
    {
        Debug.Log("Player calc Seek");
        if (lastPos == Vector3.zero)
        {
            int phi = Random.Range(0, 1);
            lastPos = transform.position + new Vector3(5 * Mathf.Cos(phi), 0, 5 * Mathf.Sin(phi));
        }
        int newPhi = 90;
        Vector3 newPos = new Vector3(
            lastPos.x * Mathf.Cos(newPhi) - lastPos.y * Mathf.Sin(newPhi),
            0,
            lastPos.x * Mathf.Sin(newPhi) + lastPos.y * Mathf.Cos(newPhi));
        int walkLength = 10;

        lastPos = newPos;
       
        return transform.position + newPos * walkLength;
    }

    public Vector3 GetPosition()
    {
        if(transform.position == null)
        {
            Debug.Log("null");
        }
        return transform.position;
    }
}
