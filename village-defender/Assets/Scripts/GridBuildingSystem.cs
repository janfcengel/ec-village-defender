using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class GridBuildingSystem : MonoBehaviour
{
    public static GridBuildingSystem instance;

    public GridLayout gridLayout;
    public Tilemap layoutTilemap;
    public Tilemap templayoutTilemap;

    private static Dictionary<TileType, TileBase> tileBases = new Dictionary<TileType, TileBase>();
    public TileBase tile_white;
    public TileBase tile_green;
    public TileBase tile_red;

    public GameObject villageObjects;
    private Building temp;
    private Vector3 prevPos;

    private BoundsInt prevArea;

    #region Unity Methods
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //string path = @"Assets\Flat Textures Pack\Textures\Water_D";
        tileBases.Add(TileType.Empty, null);
        //tileBases.Add(TileType.White, Resources.Load<TileBase>(path));
        tileBases.Add(TileType.White, tile_white);
        tileBases.Add(TileType.Green, tile_green);
        tileBases.Add(TileType.Red, tile_red);

        GetVillageObjects();
    }

    // Update is called once per frame
    void Update()
    {
        if (!temp)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject(0))
            {
                return;
            }
            if (!temp.placed)
            {
                //umschreiben der touch pos y=z
                var mousePos = Input.mousePosition;
                //Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //Vector3 touchPos3d = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, 0, mousePos.z));
                Vector3 touchPos3d = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0));
                Debug.Log("Input.mousePosition: x " + Input.mousePosition.x + " y " + Input.mousePosition.y + " z " + Input.mousePosition.z);
                // Das hier ist nicht ausreichend für die Building View 
                // Ich vermute, dass der Mittelpunkt der Camera auf das Grid zeigt, wodurch bei einer stationären camera nicht viel passiert
                // Ich müsste den Mouse Input Ray verwenden um die Cell des Grids zu ermittlen


                Vector3Int cellPos = gridLayout.LocalToCell(touchPos3d);

                if (prevPos != cellPos)
                {
                    temp.transform.localPosition = gridLayout.CellToLocalInterpolated(cellPos);
                    // + new Vector3(0.5f, 0.5f, 0));
                    prevPos = cellPos;
                    FollowBuilding();
                }

            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (temp.CanBePlaced())
            {
                temp.Place();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (temp.placed)
            {
                return;
            }
            else
            { 
            ClearArea();
            Destroy(temp.gameObject);
            }
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            ClearArea();
            Rotate();
        }

    }
    #endregion

    #region TileMap Management

    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;
        foreach (var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }
        return array;
    }

    private static void SetTilesBlock(BoundsInt area, Tilemap tilemap, TileType tiletype)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        FillTiles(array, tiletype);
        tilemap.SetTilesBlock(area, array);
    }

    private static void FillTiles(TileBase[] array, TileType tiletype)
    {
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = tileBases[tiletype];
        }
    }

    #endregion

    #region Building Placement

    public void InitializeWithBuilding(GameObject building)
    {
        temp = Instantiate(building, Vector3.zero, Quaternion.identity).GetComponent<Building>();
        // Debug.Log("building area: x " + temp.area.x + " y " + temp.area.y + " z " + temp.area.z);
        //Debug.Log("building szie: x " + temp.area.size.x + " y " + temp.area.size.y + " z " + temp.area.size.z);
        FollowBuilding();
    }

    private void ClearArea()
    {
        TileBase[] toClear = new TileBase[prevArea.size.x * prevArea.size.y * prevArea.size.z];
        FillTiles(toClear, TileType.Empty);
        // Debug.Log("PrevaArea: x " + prevArea.x + " y " + prevArea.y + " z " + prevArea.z);
        // Debug.Log("toClear: x " + toClear.Length  );
        templayoutTilemap.SetTilesBlock(prevArea, toClear);
    }

    private void FollowBuilding()
    {
        ClearArea();
        temp.area.position = gridLayout.WorldToCell(temp.gameObject.transform.position);
        BoundsInt buildingArea = temp.area;

        TileBase[] basearray = GetTilesBlock(buildingArea, layoutTilemap);
        int size = basearray.Length;

        TileBase[] tilearray = new TileBase[size];
        for (int i = 0; i < size; i++)
        {
            if (basearray[i] == tileBases[TileType.White])
            {
                tilearray[i] = tileBases[TileType.Green];
            }
            else
            {
                FillTiles(tilearray, TileType.Red);
                break;
            }
        }
        templayoutTilemap.SetTilesBlock(buildingArea, tilearray);
        prevArea = buildingArea;

    }

    public bool CanTakeArea(BoundsInt area)
    {
        TileBase[] basearray = GetTilesBlock(area, layoutTilemap);

        foreach (var tilebase in basearray)
        {
            if (tilebase != tileBases[TileType.White])
            {
                Debug.Log("Cant place here");
                return false;
            }
        }
        return true;
    }

    public void TakeArea(BoundsInt area)
    {
        SetTilesBlock(area, templayoutTilemap, TileType.Empty);
        SetTilesBlock(area, layoutTilemap, TileType.Green);
    }

    public void Rotate()
    {
        if(temp.placed)
        {
            return; 
        }
        if (!temp.rotated)
        {
            
            temp.transform.Rotate(0, 90, 0);
            moveChildFromHolder(true);
            temp.rotated = !temp.rotated;
        }
        else
        {
            moveChildFromHolder(false);
            temp.transform.Rotate(0, -90, 0);
            
            temp.rotated = !temp.rotated;
        }
        temp.area.size = new Vector3Int(temp.area.size.y, temp.area.size.x, 1);
        FollowBuilding();
    }
    private void moveChildFromHolder(bool isAdd)
    {
        float size_x = temp.area.size.x;
        float size_y = temp.area.size.y;
        Transform child = temp.transform.GetChild(0);

        if(isAdd)
        {
           child.transform.position += new Vector3(0, 0, size_x);
        }
        else
        {
           child.transform.position -= new Vector3(0, 0, size_y);
        }

        //Hole Children von Holder und verschiebe den x punkt um die  + länge Vec(x->z)
        //Beim -90 natürlich das ganze mit minus
        //temp.get
    }
    public void GetVillageObjects()
    {
        int children = villageObjects.transform.childCount;

        Transform[] transformChildren = new Transform[children];// = villageObjects.transform.GetChildCount();
        for (int i = 0; i < children; i++)
        {
            transformChildren[i] = villageObjects.transform.GetChild(i);
        }
        foreach (Transform obj in transformChildren)
        {
            var buildingObj = obj.GetComponent<Building>();
            if (buildingObj.rotated)
            {
                //obj.Rotate(0, 90, 0);
                // Es muss quasi nciht das GameObject selbst gedreht werden sondern die area 
                var x = buildingObj.area.size.x;
                var y = buildingObj.area.size.y;
                buildingObj.area.size = new Vector3Int(y, x, buildingObj.area.size.z);
                
            }
            buildingObj.area.position = gridLayout.LocalToCell(new Vector3Int(
                System.Convert.ToInt32(obj.position.x),
                System.Convert.ToInt32(obj.position.y),
                System.Convert.ToInt32(obj.position.z)
                ));
            TakeArea(buildingObj.area);
        }
    }

    #endregion
}

public enum TileType
{
    Empty,
    White, //Buildable
    Green, //Ready to place
    Red //Not Ready to place
}