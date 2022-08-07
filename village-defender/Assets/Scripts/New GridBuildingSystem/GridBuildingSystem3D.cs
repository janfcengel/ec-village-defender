using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GridBuildingSystem3D : MonoBehaviour {

    public static GridBuildingSystem3D Instance { get; private set; }

    public event EventHandler OnSelectedChanged;
    public event EventHandler OnObjectPlaced;


    private GridXZ<GridObject> grid;
    [SerializeField] private List<PlacedObjectTypeSO> placedObjectTypeSOList = null;
    private PlacedObjectTypeSO placedObjectTypeSO;
    private PlacedObjectTypeSO.Dir dir;

    [SerializeField]
    private Camera buildingCamera;
    [SerializeField]
    private Transform VillageObjects;
    private void Awake() {
        Instance = this;
        //Test
        int gridWidth = 72;
        int gridHeight = 72;
        float cellSize = 1f;
        Vector3 originOffset = new Vector3(-36, 0, -36);
        grid = new GridXZ<GridObject>(gridWidth, gridHeight, cellSize, originOffset, (GridXZ<GridObject> g, int x, int y) => new GridObject(g, x, y));

        
    }

    public class GridObject {

        private GridXZ<GridObject> grid;
        private int x;
        private int y;
        public PlacedObject_Done placedObject;

        public GridObject(GridXZ<GridObject> grid, int x, int y) {
            this.grid = grid;
            this.x = x;
            this.y = y;
            placedObject = null;
        }

        public override string ToString() {
            return x + ", " + y + "\n" + placedObject;
        }

        public void SetPlacedObject(PlacedObject_Done placedObject) {
            this.placedObject = placedObject;
            grid.TriggerGridObjectChanged(x, y);
        }

        public void ClearPlacedObject() {
            placedObject = null;
            grid.TriggerGridObjectChanged(x, y);
        }

        public PlacedObject_Done GetPlacedObject() {
            return placedObject;
        }

        public bool CanBuild() {
            return placedObject == null;
        }

    }

    private void Start()
    {
        PlaceStartBuildings();
        //placedObjectTypeSO = placedObjectTypeSOList[0];
        DeselectObjectType();
    }

    private void Update() {

        if(!CanvasController.GetBuildingMode()) { return; }

        if(Input.GetMouseButtonDown(0) && placedObjectTypeSO == null)
        {
            Vector3 mousePosition = Mouse3D.GetMouseWorldPosition(buildingCamera);
            //Vector3 mousePosition = GetMyMouseWorldPosition(Camera.main, Input.mousePosition);
            if (grid.GetGridObject(mousePosition) != null)
            {
                // Valid Grid Position
                PlacedObject_Done placedObject = grid.GetGridObject(mousePosition).GetPlacedObject();
                if (placedObject != null)
                {
                    // Reselect the building and place it again
                    placedObject.DestroySelf();
                    placedObjectTypeSO = placedObject.GetPlacedObjectType();
                    dir = placedObject.GetDir();
                    List<Vector2Int> gridPositionList = placedObject.GetGridPositionList();
                    foreach (Vector2Int gridPosition in gridPositionList)
                    {
                        grid.GetGridObject(gridPosition.x, gridPosition.y).ClearPlacedObject();
                    }
                    RefreshSelectedObjectType();
                }
            }
            Debug.Log("Selection Mode");
            return;
        }
        if (Input.GetMouseButtonDown(0) && placedObjectTypeSO != null) {
            Vector3 mousePosition = Mouse3D.GetMouseWorldPosition(buildingCamera);
            
            //Debug.Log(mousePosition.x + ", " + mousePosition.y + ", " + mousePosition.z);
            grid.GetXZ(mousePosition, out int x, out int z);

            Vector2Int placedObjectOrigin = new Vector2Int(x, z);
            placedObjectOrigin = grid.ValidateGridPosition(placedObjectOrigin);

            // Test Can Build
            List<Vector2Int> gridPositionList = placedObjectTypeSO.GetGridPositionList(placedObjectOrigin, dir);
            bool canBuild = true;
            foreach (Vector2Int gridPosition in gridPositionList) {
                GridObject gridObject = grid.GetGridObject(gridPosition.x, gridPosition.y);
                if(gridObject == default) {
                    canBuild = false;
                    break;
                }
                if (!gridObject.CanBuild()) { 
                    canBuild = false;
                    break;
                }
            }

            if (canBuild) {
                Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
                Vector3 placedObjectWorldPosition = grid.GetWorldPosition(placedObjectOrigin.x, placedObjectOrigin.y) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();

                PlacedObject_Done placedObject = PlacedObject_Done.Create(placedObjectWorldPosition, placedObjectOrigin, dir, placedObjectTypeSO);

                foreach (Vector2Int gridPosition in gridPositionList) {
                    grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
                }

                OnObjectPlaced?.Invoke(this, EventArgs.Empty);

                DeselectObjectType();  
            } else {
                // Cannot build here
                //Todo: Adjust camera 
                UtilsClass.CreateWorldTextPopup("Cannot Build Here!", mousePosition);
            }
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            dir = PlacedObjectTypeSO.GetNextDir(dir);
        }

        /*if (Input.GetKeyDown(KeyCode.Alpha1)) { placedObjectTypeSO = placedObjectTypeSOList[0]; RefreshSelectedObjectType(); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { placedObjectTypeSO = placedObjectTypeSOList[1]; RefreshSelectedObjectType(); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { placedObjectTypeSO = placedObjectTypeSOList[2]; RefreshSelectedObjectType(); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { placedObjectTypeSO = placedObjectTypeSOList[3]; RefreshSelectedObjectType(); }
        if (Input.GetKeyDown(KeyCode.Alpha5)) { placedObjectTypeSO = placedObjectTypeSOList[4]; RefreshSelectedObjectType(); }
        if (Input.GetKeyDown(KeyCode.Alpha6)) { placedObjectTypeSO = placedObjectTypeSOList[5]; RefreshSelectedObjectType(); }
        */
        if (Input.GetKeyDown(KeyCode.Escape)) { DeselectObjectType(); }


        if (Input.GetMouseButtonDown(1)) {
            Vector3 mousePosition = Mouse3D.GetMouseWorldPosition(buildingCamera);
            //Vector3 mousePosition = GetMyMouseWorldPosition(Camera.main, Input.mousePosition);
            if (grid.GetGridObject(mousePosition) != null) {
                // Valid Grid Position
                PlacedObject_Done placedObject = grid.GetGridObject(mousePosition).GetPlacedObject();
                if (placedObject != null) {
                    // Demolish
                    placedObject.DestroySelf();

                    List<Vector2Int> gridPositionList = placedObject.GetGridPositionList();
                    foreach (Vector2Int gridPosition in gridPositionList) {
                        grid.GetGridObject(gridPosition.x, gridPosition.y).ClearPlacedObject();
                    }
                }
            }
        }
    }

    private void DeselectObjectType() {
        placedObjectTypeSO = null; RefreshSelectedObjectType();
    }

    private void RefreshSelectedObjectType() {
        OnSelectedChanged?.Invoke(this, EventArgs.Empty);
    }


    public Vector2Int GetGridPosition(Vector3 worldPosition) {
        grid.GetXZ(worldPosition, out int x, out int z);
        return new Vector2Int(x, z);
    }

    public Vector3 GetMouseWorldSnappedPosition() {
        Vector3 mousePosition = Mouse3D.GetMouseWorldPosition(buildingCamera);
        grid.GetXZ(mousePosition, out int x, out int z);

        if (placedObjectTypeSO != null) {
            Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();
            return placedObjectWorldPosition;
        } else {
            return mousePosition;
        }
    }

    public Quaternion GetPlacedObjectRotation() {
        if (placedObjectTypeSO != null) {
            return Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0);
        } else {
            return Quaternion.identity;
        }
    }

    public PlacedObjectTypeSO GetPlacedObjectTypeSO() {
        return placedObjectTypeSO;
    }


    public bool CanBuildBuilding()
    {
        if(placedObjectTypeSO == null)
        {
            return false;
        }
        Vector3 mousePosition = Mouse3D.GetMouseWorldPosition(buildingCamera);
        grid.GetXZ(mousePosition, out int x, out int z);

        Vector2Int placedObjectOrigin = new Vector2Int(x, z);
        placedObjectOrigin = grid.ValidateGridPosition(placedObjectOrigin);
        List<Vector2Int> gridPositionList = placedObjectTypeSO.GetGridPositionList(placedObjectOrigin, dir);
        bool canBuild = true;
        foreach (Vector2Int gridPosition in gridPositionList)
        {
            GridObject gridObject = grid.GetGridObject(gridPosition.x, gridPosition.y);
            if (gridObject == default)
            {
                canBuild = false;
                break;
            }
            if (!gridObject.CanBuild())
            {
                canBuild = false;
                break;
            }
        }
        return canBuild;
    }

    private void PlaceStartBuildings()
    {
        //0 = H01, 1 = H02, 2 = Wall, 3 = Gate
        //placedObjectTypeSO = placedObjectTypeSOList[0];

        BuildPlacableObject(37, 16, placedObjectTypeSOList[0], PlacedObjectTypeSO.Dir.Down);
        BuildPlacableObject(20, 17, placedObjectTypeSOList[1], PlacedObjectTypeSO.Dir.Right);
        BuildPlacableObject(20, 45, placedObjectTypeSOList[1], PlacedObjectTypeSO.Dir.Down);
        BuildPlacableObject(48, 40, placedObjectTypeSOList[0], PlacedObjectTypeSO.Dir.Up);
        BuildPlacableObject(39, 51, placedObjectTypeSOList[2], PlacedObjectTypeSO.Dir.Right);
        BuildPlacableObject(48, 53, placedObjectTypeSOList[2], PlacedObjectTypeSO.Dir.Right);
        BuildPlacableObject(57, 43, placedObjectTypeSOList[2], PlacedObjectTypeSO.Dir.Up);
        BuildPlacableObject(57, 33, placedObjectTypeSOList[3], PlacedObjectTypeSO.Dir.Up);
        BuildPlacableObject(56, 23, placedObjectTypeSOList[2], PlacedObjectTypeSO.Dir.Up);
        BuildPlacableObject(46, 24, placedObjectTypeSOList[2], PlacedObjectTypeSO.Dir.Right);
        BuildPlacableObject(46, 14, placedObjectTypeSOList[2], PlacedObjectTypeSO.Dir.Down);
        BuildPlacableObject(36, 14, placedObjectTypeSOList[2], PlacedObjectTypeSO.Dir.Left);
        BuildPlacableObject(27, 12, placedObjectTypeSOList[3], PlacedObjectTypeSO.Dir.Left);
        BuildPlacableObject(18, 14, placedObjectTypeSOList[2], PlacedObjectTypeSO.Dir.Left);
        BuildPlacableObject(16, 14, placedObjectTypeSOList[2], PlacedObjectTypeSO.Dir.Up);
        BuildPlacableObject(14, 22, placedObjectTypeSOList[2], PlacedObjectTypeSO.Dir.Up);
        BuildPlacableObject(12, 30, placedObjectTypeSOList[3], PlacedObjectTypeSO.Dir.Up);
        BuildPlacableObject(14, 38, placedObjectTypeSOList[2], PlacedObjectTypeSO.Dir.Up);
        BuildPlacableObject(16, 47, placedObjectTypeSOList[2], PlacedObjectTypeSO.Dir.Up);
        BuildPlacableObject(18, 57, placedObjectTypeSOList[2], PlacedObjectTypeSO.Dir.Right);
        BuildPlacableObject(27, 59, placedObjectTypeSOList[3], PlacedObjectTypeSO.Dir.Right);
        BuildPlacableObject(37, 50, placedObjectTypeSOList[2], PlacedObjectTypeSO.Dir.Up);
    }

    private void BuildPlacableObject(int x, int z, PlacedObjectTypeSO objectTypeSO, PlacedObjectTypeSO.Dir dir)
    {
        Vector2Int placedObjectOrigin = new Vector2Int(x, z);
        placedObjectOrigin = grid.ValidateGridPosition(placedObjectOrigin);

        // Test Can Build
        List<Vector2Int> gridPositionList = objectTypeSO.GetGridPositionList(placedObjectOrigin, dir);
        bool canBuild = true;
        foreach (Vector2Int gridPosition in gridPositionList)
        {
            GridObject gridObject = grid.GetGridObject(gridPosition.x, gridPosition.y);
            if (gridObject == default)
            {
                canBuild = false;
                break;
            }
            if (!gridObject.CanBuild())
            {
                canBuild = false;
                break;
            }
        }

        if (canBuild)
        {
            Vector2Int rotationOffset = objectTypeSO.GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition = grid.GetWorldPosition(placedObjectOrigin.x, placedObjectOrigin.y) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();

            PlacedObject_Done placedObject = PlacedObject_Done.Create(placedObjectWorldPosition, placedObjectOrigin, dir, objectTypeSO, VillageObjects);
            
            foreach (Vector2Int gridPosition in gridPositionList)
            {
                grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
            }

            OnObjectPlaced?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Debug.Log("Placing Building Failed, cant build");
        }
    }

    public void SelectPlaceObjectTypeHouse01()
    {
        if (CanvasController.GetCurrentHouse01()) { Debug.Log("Too Many Buildings of that Type");  return; }
        placedObjectTypeSO = placedObjectTypeSOList[0];
        RefreshSelectedObjectType();
    }
    public void SelectPlaceObjectTypeHouse02()
    {
        if (CanvasController.GetCurrentHouse02()) { Debug.Log("Too Many Buildings of that Type"); return; }
        placedObjectTypeSO = placedObjectTypeSOList[1];
        RefreshSelectedObjectType();
    }
    public void SelectPlaceObjectTypeWall()
    {
        if (CanvasController.GetCurrentWall()) { Debug.Log("Too Many Buildings of that Type"); return; }
        placedObjectTypeSO = placedObjectTypeSOList[2];
        RefreshSelectedObjectType();
    }
    public void SelectPlaceObjectTypeGate()
    {
        if (CanvasController.GetCurrentGate()) { Debug.Log("Too Many Buildings of that Type"); return; }
        placedObjectTypeSO = placedObjectTypeSOList[3];
        RefreshSelectedObjectType();
    }
}
