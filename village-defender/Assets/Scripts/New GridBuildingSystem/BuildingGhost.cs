using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhost : MonoBehaviour {

    private Transform visual;
    private PlacedObjectTypeSO placedObjectTypeSO;
    private bool lastCanBuild = true;
     
    private void Start() {
        RefreshVisual();
        GridBuildingSystem3D.Instance.OnSelectedChanged += Instance_OnSelectedChanged;
    }

    private void Instance_OnSelectedChanged(object sender, System.EventArgs e) {
        RefreshVisual();
    }

    private void LateUpdate() {
        Vector3 targetPosition = GridBuildingSystem3D.Instance.GetMouseWorldSnappedPosition();
        targetPosition.y = 1f;

        if(lastCanBuild != GridBuildingSystem3D.Instance.CanBuildBuilding())
        {
            RefreshVisual();
        }
        lastCanBuild = GridBuildingSystem3D.Instance.CanBuildBuilding();
        transform.position = targetPosition;
        transform.rotation = Quaternion.Lerp(transform.rotation, GridBuildingSystem3D.Instance.GetPlacedObjectRotation(), Time.deltaTime * 15f);
    }

        private void RefreshVisual() {
        if (visual != null) {
            Destroy(visual.gameObject);
            visual = null;
        }

        PlacedObjectTypeSO placedObjectTypeSO = GridBuildingSystem3D.Instance.GetPlacedObjectTypeSO();
        
        if (placedObjectTypeSO != null) {
            if(GridBuildingSystem3D.Instance.CanBuildBuilding())
            {
                visual = Instantiate(placedObjectTypeSO.visual, Vector3.zero, Quaternion.identity);
            }
            else
            {
                visual = Instantiate(placedObjectTypeSO.visual_red, Vector3.zero, Quaternion.identity);
            }
            visual.parent = transform;
            visual.localPosition = Vector3.zero;
            visual.localEulerAngles = Vector3.zero;
            SetLayerRecursive(visual.gameObject, 11);
        }
    }

    private void SetLayerRecursive(GameObject targetGameObject, int layer) {
        targetGameObject.layer = layer;
        foreach (Transform child in targetGameObject.transform) {
            SetLayerRecursive(child.gameObject, layer);
        }
    }

}

