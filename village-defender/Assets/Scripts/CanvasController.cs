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
    public GameObject buildingGrid;

    #region Unity Methods
    
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
        SetBuildingCamActive(buildingMode);
        if (buildingMode)
        {
            BuildingModeButton.GetComponentInChildren<Text>().text = "Player Mode";
        }
        else
        {
            BuildingModeButton.GetComponentInChildren<Text>().text = "Build Mode";
        }
        //buildingGrid.SetActive(buildingMode);
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
}
