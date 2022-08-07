using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public bool buildingMode;
    public Button BuildingModeButton;
    public GameObject[] BuidingButton;
    public GameObject[] Cameras;
    public GameObject BuildingInfoText;
    public GameObject buildingGrid;

    public static CanvasController Instance;


    #region Unity Methods

    private void Awake()
    {
        Instance = this; 
    }
    // Start is called before the first frame update
    void Start()
    {
        
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
        //Bugs: MousePos wird nicht richtig in CellPos übersetzt in GridBuildingSystem 
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
}
