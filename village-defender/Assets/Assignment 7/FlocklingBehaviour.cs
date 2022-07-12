using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FlocklingBehaviour : MonoBehaviour
{

    public FlockController flock { get; set; }

    public Animator animator;

    public NavMeshAgent agent;


    private AnimatorStateInfo stateInfo;
    private AnimatorStateInfo lastState;
    // Start is called before the first frame update
    void Start()
    {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        lastState = stateInfo;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        
        float distanceToCompanion = new Vector2(transform.position.x - flock.GetComponionPos().x, transform.position.z - flock.GetComponionPos().z).magnitude;
        Vector3 movement; 
        if (distanceToCompanion < 4)
        {
            animator.SetBool("isGrouping", false);
            animator.SetBool("isFleeing", true);
        }
        else
        {
            animator.SetBool("isFleeing", false);
            animator.SetBool("isGrouping", true);
        }
        
        if (agent.hasPath && animator.GetBool("isGrouping"))
        {
            if (distanceToCompanion < 5)
            {
                movement = MovementDecision();
                agent.SetDestination(movement);
            }
            //Debug.Log("has path and is grouping");
            return;
        }
        if(animator.GetBool("isGrouping"))
        {
           // Debug.Log("isGrouping calc");
        }
        else
        {
//Debug.Log("isFleeing calc");
        }

        movement = MovementDecision();
        agent.SetDestination(movement);
    }

    private Vector3 MovementDecision()
    {
        if (stateInfo.IsName("Group"))
        {
            return Group();
        }
        else if (stateInfo.IsName("Flee"))
        {
            return Flee();
        }
        return Vector3.zero;
    }
    Vector3 lastPos = Vector3.zero;
    private Vector3 Group()
    {
        float distanceToCenter = new Vector2(transform.position.x-flock.GetCenterPos().x, transform.position.z-flock.GetCenterPos().z).magnitude;
        if(distanceToCenter > 3)
        {
            return new Vector3(transform.position.x - flock.GetCenterPos().x, 0, transform.position.z - flock.GetCenterPos().z);
        }

        if (distanceToCenter < 1)
        {
            return -(new Vector3(transform.position.x - flock.GetCenterPos().x, 0, transform.position.z - flock.GetCenterPos().z)) ;
        }

        if (lastPos == Vector3.zero)
        {
            int phi = Random.Range(0, 360);
            lastPos = transform.position + new Vector3(5 * Mathf.Cos(phi), 0, 5 * Mathf.Sin(phi));
        }
        int newPhi = Random.Range(-10, 10);
        Vector3 newPos = new Vector3(
            lastPos.x * Mathf.Cos(newPhi) - lastPos.y * Mathf.Sin(newPhi),
            0,
            lastPos.x * Mathf.Sin(newPhi) + lastPos.y * Mathf.Cos(newPhi));
        int walkLength = Random.Range(1, 5);

        lastPos = newPos;
        return transform.position + newPos * walkLength;
    }

    private Vector3 Flee()
    {
        //Vector away from companion + towards flock center
        return new Vector3(transform.position.x - flock.GetComponionPos().x, 0, transform.position.z - flock.GetComponionPos().z)
            + flock.GetCenterPos();
    }
}
