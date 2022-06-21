using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Region : MonoBehaviour
{
    public RegionType region;

    private void OnTriggerEnter(Collider other)
    {
        RegionObserver.instance.SetCurrentRegionType(region);
    }
}

public enum RegionType
{
    Village,
    Forest,
}