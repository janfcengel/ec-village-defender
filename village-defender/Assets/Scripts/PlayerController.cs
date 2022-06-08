using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private float speed = 0.1f;
    //public GameObject inventory;
    [SerializeField]
    HealthBarSystem healthBar; 

    // Start is called before the first frame update
    void Start()
    {
        healthBar.SetSize(.5f);
    }

    // Update is called once per frame
    void Update()
    {
        float xDirection = Input.GetAxis("Horizontal");
        float zDirection = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(xDirection, 0f, zDirection);

        transform.position += moveDirection * speed;

        if (Input.GetKeyDown(KeyCode.E))
        {

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
}
