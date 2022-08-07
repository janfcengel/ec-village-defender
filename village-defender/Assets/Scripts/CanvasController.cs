using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public bool buildingMode;
    public Button BuildingModeButton;
    public GameObject[] BuidingButton;
    public Text[] ButtonCountTexts;
    public GameObject[] Cameras;
    public GameObject BuildingInfoText;
    public GameObject buildingGrid;

    public GameObject[] UIDisableList; 

    public static CanvasController Instance;

    private const int MAX_HOUSES01 = 4;
    private const int MAX_HOUSES02 = 4;
    private const int MAX_WALLS = 20;
    private const int MAX_GATES = 6;

    private int _currentHouses01;
    private int _currentHouses02;
    private int _currentWalls;
    private int _currentGates;

    #region Unity Methods

    private void Awake()
    {
        Instance = this;
        _currentHouses01 = 0;
        _currentHouses02 = 0;
        _currentWalls = 0;
        _currentGates = 0; 
    }
    // Start is called before the first frame update
    void Start()
    {
        SetBuildingCountTexts();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion
    #region BuildMode
    public void OnBuildModeClick()
    {
        buildingMode = !buildingMode;
        foreach (GameObject button in BuidingButton)
        {
            button.SetActive(buildingMode);
        }
        SetBuildingCamActive(buildingMode);
        if (buildingMode)
        {
            BuildingModeButton.GetComponentInChildren<Text>().text = "Player Mode";
        }
        else
        {
            BuildingModeButton.GetComponentInChildren<Text>().text = "Build Mode";
        }
        buildingGrid.SetActive(buildingMode);
        BuildingInfoText.SetActive(buildingMode);
        foreach (GameObject uielement in UIDisableList)
        {
            uielement.SetActive(!buildingMode);
        }
    }
    void SetBuildingCamActive(bool enabled)
    {
        foreach (GameObject cam in Cameras)
        {
            if (cam.name == "Building Camera")
            {
                cam.SetActive(enabled);
            }
            else
            {
                cam.SetActive(!enabled);
            }
        }
    }
    #endregion

    public static bool GetBuildingMode() => Instance.Instance_GetBuildingMode();

    private bool Instance_GetBuildingMode()
    {
        return buildingMode; 
    }

    private void SetBuildingCountTexts()
    {
        ButtonCountTexts[0].text = "House 01: \n" + _currentHouses01 + "/" + MAX_HOUSES01;
        ButtonCountTexts[1].text = "House 02: \n" + _currentHouses02 + "/" + MAX_HOUSES02;
        ButtonCountTexts[2].text = "Walls: \n" + _currentWalls + "/" + MAX_WALLS;
        ButtonCountTexts[3].text = "Gates: \n" + _currentGates + "/" + MAX_GATES;
    }

    public static bool GetCurrentHouse01() => Instance.Instance_GetCurrentHouse01() == MAX_HOUSES01;

    private int Instance_GetCurrentHouse01()
    {
        return _currentHouses01;
    }
    public bool AddCurrentHouses01(int count)
    {
        if(count + _currentHouses01 > MAX_HOUSES01 || count + _currentHouses01 < 0)
        {
            return false;
        }
        _currentHouses01 += count;
        SetBuildingCountTexts();
        return true; 
    }
    public static bool GetCurrentHouse02() => Instance.Instance_GetCurrentHouse02() == MAX_HOUSES02;

    private int Instance_GetCurrentHouse02()
    {
        return _currentHouses02;
    }
    public bool AddCurrentHouses02(int count)
    {
        if (count + _currentHouses02 > MAX_HOUSES02 || count + _currentHouses02 < 0)
        {
            return false;
        }
        _currentHouses02 += count;
        SetBuildingCountTexts();
        return true;
    }
    public static bool GetCurrentWall() => Instance.Instance_GetCurrentWall() == MAX_WALLS;

    private int Instance_GetCurrentWall()
    {
        return _currentWalls;
    }
    public bool AddCurrentWall(int count)
    {
        if (count + _currentWalls > MAX_WALLS|| count + _currentWalls < 0)
        {
            return false;
        }
        _currentWalls += count;
        SetBuildingCountTexts();
        return true;
    }
    public static bool GetCurrentGate() => Instance.Instance_GetCurrentGate() == MAX_GATES;

    private int Instance_GetCurrentGate()
    {
        return _currentGates;
    }
    public bool AddCurrentGate(int count)
    {
        if (count + _currentGates > MAX_GATES|| count + _currentGates < 0)
        {
            return false;
        }
        _currentGates += count;
        SetBuildingCountTexts();
        return true;
    }


    public void AddBuildingCount(PlacedObjectTypeSO placedObjectTypeSO, int count)
    {
        bool countSuccess = true; 
        if(placedObjectTypeSO.nameString == "House01") { countSuccess = AddCurrentHouses01(count); }
        else if (placedObjectTypeSO.nameString == "House02") { countSuccess = AddCurrentHouses02(count); }
        else if (placedObjectTypeSO.nameString == "Wall") { countSuccess = AddCurrentWall(count); }
        else if (placedObjectTypeSO.nameString == "Gate") { countSuccess = AddCurrentGate(count); }
       
        if(!countSuccess)
        {
            Debug.Log("CountSuccess False");
        }
    }
}
