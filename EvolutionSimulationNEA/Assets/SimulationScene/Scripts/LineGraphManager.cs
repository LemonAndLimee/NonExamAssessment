using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//this script manages the overall graph with all 4 lines
public class LineGraphManager : MonoBehaviour
{
    WorldLogic worldScript; //stores the WorldLogic script

    float graphHeight = 2.6f; //height of the graph
    float graphWidth = 15f; //width of the graph

    GraphLogic speedLine; //refers to the line plotting speed
    GraphLogic sizeLine; //refers to the line plotting size
    GraphLogic visionRangeLine; //refers to the line plotting vision range
    GraphLogic idealTempLine; //refers to the line plotting ideal temperature

    //checkboxes used to toggle visibility of each line
    public Toggle speedCheckbox;
    public Toggle sizeCheckbox;
    public Toggle VRcheckbox;
    public Toggle tempCheckbox;

    bool graphActive = false; //sets default graph state to disabled

    //called at the start
    void Start()
    {
        worldScript = GameObject.Find("SimulationManager").GetComponent<WorldLogic>(); //assigns WorldLogic script to worldScript

        speedLine = transform.Find("Speed").gameObject.GetComponent<GraphLogic>(); //assigns speed graph
        //sets the starting value of the speed graph to the simulation starting speed
        speedLine.SetStartingValue(worldScript.GetStartingSpeed());

        sizeLine = transform.Find("Size").gameObject.GetComponent<GraphLogic>(); //assigns size graph
        //sets the starting value of the size graph to the simulation starting size
        sizeLine.SetStartingValue(worldScript.GetStartingSize());

        visionRangeLine = transform.Find("VisionRange").gameObject.GetComponent<GraphLogic>(); //assigns vision range graph
        //sets the starting value of the vision range graph to the simulation starting vision range
        visionRangeLine.SetStartingValue(worldScript.GetStartingVisionRange());

        idealTempLine = transform.Find("IdealTemp").gameObject.GetComponent<GraphLogic>(); //assigns ideal temperature graph
        //sets the starting value of the ideal temperature graph to the simulation starting ideal temperature
        idealTempLine.SetStartingValue(worldScript.GetStartingIdealTemp());
    }

    //plots a new point on each graph at the moment it is called
    public void UpdateValues()
    {
        //plots the mean value of each trait on each of the four graphs
        speedLine.PlotGraphPoint(worldScript.GetMeanSpeed());
        sizeLine.PlotGraphPoint(worldScript.GetMeanSize());
        visionRangeLine.PlotGraphPoint(worldScript.GetMeanVisionRange());
        idealTempLine.PlotGraphPoint(worldScript.GetMeanIdealTemp());
    }

    //returns graph height
    public float GetGraphHeight()
    {
        return graphHeight;
    }
    //returns graph width
    public float GetGraphWidth()
    {
        return graphWidth;
    }

    //resets graph
    public void Reset()
    {
        speedLine.Reset(); //resets speed graph
        speedLine.SetStartingValue(worldScript.GetStartingSpeed()); //resets speed graph starting value

        sizeLine.Reset(); //resets size graph
        sizeLine.SetStartingValue(worldScript.GetStartingSize()); //resets size graph starting value

        visionRangeLine.Reset(); //resets vision range graph
        visionRangeLine.SetStartingValue(worldScript.GetStartingVisionRange()); //resets vision range graph starting value

        idealTempLine.Reset(); //resets ideal temperature graph
        idealTempLine.SetStartingValue(worldScript.GetStartingIdealTemp()); //resets ideal temperature graph starting value
    }

    //toggles overall graph visibility
    public void ToggleVisibility()
    {
        //if graph is disabled
        if (graphActive == false)
        {
            graphActive = true;
            //enables the LineRenderer of each of the graphs
            LineRenderer[] renderers = gameObject.GetComponentsInChildren<LineRenderer>();
            foreach (LineRenderer lr in renderers)
            {
                lr.enabled = true;
            }

            //if the checkbox corresponding to the line is off, turn off the line
            if (speedCheckbox.isOn == false)
            {
                ToggleSpeedLine();
            }
            if (sizeCheckbox.isOn == false)
            {
                ToggleSizeLine();
            }
            if (VRcheckbox.isOn == false)
            {
                ToggleVisionRangeLine();
            }
            if (tempCheckbox.isOn == false)
            {
                ToggleTemperatureLine();
            }
        }
        else //if graph is enabled
        {
            graphActive = false;
            //disables the LineRenderer of each of the graphs
            LineRenderer[] renderers = gameObject.GetComponentsInChildren<LineRenderer>();
            foreach (LineRenderer lr in renderers)
            {
                lr.enabled = false;
            }
        }
    }

    public void ToggleSpeedLine() //toggles speed graph visibility
    {
        speedLine.ToggleVisibility();
    }
    public void ToggleSizeLine() //toggles size graph visibility
    {
        sizeLine.ToggleVisibility();
    }
    public void ToggleVisionRangeLine() //toggles vision range graph visibility
    {
        visionRangeLine.ToggleVisibility();
    }
    public void ToggleTemperatureLine() //toggles idea temperature graph visibility
    {
        idealTempLine.ToggleVisibility();
    }
}
