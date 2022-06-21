using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionObserver : MonoBehaviour
{
    private RegionType currentRegion;

    public static RegionObserver instance;

    private void Awake()
    {
        instance = this; 
    }

    // Start is called before the first frame update
    void Start()
    {
        currentRegion = RegionType.Village;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public RegionType GetCurrentRegionType()
    {
        return currentRegion;
    }

    public void SetCurrentRegionType(RegionType region)
    {
        Debug.Log("Region set to " + region.ToString());
        currentRegion = region;
    }
}
