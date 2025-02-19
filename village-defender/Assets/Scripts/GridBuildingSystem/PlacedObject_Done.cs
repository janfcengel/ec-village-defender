﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedObject_Done : MonoBehaviour {
     //Fertig platzierte Building
    public static PlacedObject_Done Create(Vector3 worldPosition, Vector2Int origin, PlacedObjectTypeSO.Dir dir, PlacedObjectTypeSO placedObjectTypeSO) {
        Transform placedObjectTransform = Instantiate(placedObjectTypeSO.prefab, worldPosition, Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0));
        
        PlacedObject_Done placedObject = placedObjectTransform.GetComponent<PlacedObject_Done>();
        placedObject.Setup(placedObjectTypeSO, origin, dir);

        CanvasController.Instance.AddBuildingCount(placedObjectTypeSO, 1);

        return placedObject;
    }

    public static PlacedObject_Done Create(Vector3 worldPosition, Vector2Int origin, PlacedObjectTypeSO.Dir dir, PlacedObjectTypeSO placedObjectTypeSO, Transform parent)
    {
        Transform placedObjectTransform = Instantiate(placedObjectTypeSO.prefab, worldPosition, Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0));
        placedObjectTransform.SetParent(parent);

        PlacedObject_Done placedObject = placedObjectTransform.GetComponent<PlacedObject_Done>();
        placedObject.Setup(placedObjectTypeSO, origin, dir);

        CanvasController.Instance.AddBuildingCount(placedObjectTypeSO, 1);

        return placedObject;
    }


    private PlacedObjectTypeSO placedObjectTypeSO;
    private Vector2Int origin;
    private PlacedObjectTypeSO.Dir dir;

    private void Setup(PlacedObjectTypeSO placedObjectTypeSO, Vector2Int origin, PlacedObjectTypeSO.Dir dir) {
        this.placedObjectTypeSO = placedObjectTypeSO;
        this.origin = origin;
        this.dir = dir;
    }

    public List<Vector2Int> GetGridPositionList() {
        return placedObjectTypeSO.GetGridPositionList(origin, dir);
    }

    public void DestroySelf() {
        Destroy(gameObject);
        CanvasController.Instance.AddBuildingCount(placedObjectTypeSO, -1);
    }

    public override string ToString() {
        return placedObjectTypeSO.nameString;
    }

    public PlacedObjectTypeSO GetPlacedObjectType()
    {
        return placedObjectTypeSO;
    }

    public PlacedObjectTypeSO.Dir GetDir()
    {
        return dir;
    }
}
