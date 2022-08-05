using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public bool placed { get; private set; }
    public BoundsInt area;
    public bool rotated = false;
    // Start is called before the first frame update

    #region Build Methods

    public bool CanBePlaced()
    {
        Vector3Int positionInt = GridBuildingSystem.instance.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        if (GridBuildingSystem.instance.CanTakeArea(areaTemp))
        {
            return true;
        }
        return false;
    }

    public void Place()
    {
        Vector3Int positionInt = GridBuildingSystem.instance.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;
        placed = true;
        GridBuildingSystem.instance.TakeArea(areaTemp);
    }
    #endregion
}
