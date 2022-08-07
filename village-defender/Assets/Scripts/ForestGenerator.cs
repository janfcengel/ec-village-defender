using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestGenerator : MonoBehaviour
{
    public int startingPointX = 0;
    public int startingPointY = 0;

    public float radius = 1;
    public Vector2 regionSize = Vector2.one;
    public int rejectionSample = 20;
    public float displayRadius = 1; 

    List<Vector2> points;

    [SerializeField]
    GameObject treeType1;

    [SerializeField]
    GameObject treeType2;

    [SerializeField]
    GameObject treeType3;

    // Start is called before the first frame update
    void Start()
    {
        points = PoissonDiscSampling.GeneratePoints(radius, regionSize, rejectionSample);
        if (points != null)
        {
            GameObject currentTree;
            GameObject temp;
            foreach (Vector2 point in points)
            {
                currentTree = getTreeType(Random.Range(0, 2));
                temp = Instantiate(currentTree,
                    new Vector3(point.x + startingPointX - (regionSize.x / 2), 0, point.y + startingPointY- (regionSize.y / 2)),
                    Quaternion.identity);
                temp.GetComponent<Transform>().SetParent(this.GetComponent<Transform>());
                
            }
        }
    }

    private void OnValidate()
    {
        points = PoissonDiscSampling.GeneratePoints(radius, regionSize, rejectionSample);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(new Vector3((regionSize.x  / 2) + startingPointX, 0, (regionSize.y / 2) + startingPointY), new Vector3(regionSize.x, 0, regionSize.y));
        if(points != null)
        {
            GameObject currentTree;
            foreach (Vector2 point in points)
            {
                Gizmos.DrawSphere(new Vector3(point.x + startingPointX, 0, point.y + startingPointY), displayRadius);
                currentTree = getTreeType(Random.Range(0, 2));
                //Instantiate(currentTree,
                  //  new Vector3(point.x + startingPointX, 0, point.y + startingPointY),
                   // Quaternion.Euler(0, Random.Range(0, 360), 0));
            }
        }
    }

    private GameObject getTreeType(int number)
    {
        switch(number)
        {
            case 0:
                return treeType1;
            case 1:
                return treeType2;
            case 2:
                return treeType3;
            default:
                return treeType1;
        }
    }
}
