using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockController : MonoBehaviour
{
    public int amountFlocklings = 10;

    public float spawnRadius = 5;

    public Vector3 centerPos;

    public GameObject flockling;

    private Vector3 startPos;
    private Vector3 companionPos;
    public  CompanionBehaviour companion;

    List<GameObject> flock;

    // Start is called before the first frame update
    void Start()
    {
        flock = new List<GameObject>();
        startPos = transform.position;
        //companionPos = Vector3.zero;
        spawnFlocks();
        centerPos = CalculateCenterPos();
    }
    
    // Update is called once per frame
    void Update()
    {
        centerPos = CalculateCenterPos();
        companionPos = companion.GetPosition();
    }

    public void spawnFlocks()
    {
        int phi =  360 / amountFlocklings;
        int currentPhi;
        Vector3 flocklingSpawnPos;
        GameObject temp;

        for (int i = 0; i < amountFlocklings; i++)
        {
            currentPhi = phi * i;
            flocklingSpawnPos = new Vector3(spawnRadius * Mathf.Cos(currentPhi), 0, spawnRadius * Mathf.Sin(currentPhi));
            Debug.Log("phi: " + phi + "cos: " + Mathf.Cos(currentPhi));
            temp = Instantiate(flockling, startPos + flocklingSpawnPos, Quaternion.identity);
            temp.GetComponent<FlocklingBehaviour>().flock = this;     
            flock.Add(temp); 
        }
    }

    public Vector3 CalculateCenterPos()
    {
        Vector3 calcVector = Vector3.zero; 
        foreach(GameObject tempflockling in flock)
        {
            calcVector += tempflockling.transform.position;
        }
        return calcVector / flock.Count;

    }

    public List<GameObject> GetFlock()
    {
        return flock; 
    }

    public Vector3 GetCenterPos()
    {
        return centerPos;
    }

    public Vector3 GetComponionPos()
    {
        return companionPos;
    }
}
