using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGraphManager : MonoBehaviour
{
    WorldLogic worldScript;

    float graphHeight = 3f;
    float graphWidth = 10f;

    GraphLogic speedLine;
    GraphLogic sizeLine;
    GraphLogic visionRangeLine;
    GraphLogic idealTempLine;

    bool graphActive = false;

    // Start is called before the first frame update
    void Start()
    {
        worldScript = GameObject.Find("SimulationManager").GetComponent<WorldLogic>();

        speedLine = transform.Find("Speed").gameObject.GetComponent<GraphLogic>();
        speedLine.SetStartingValue(worldScript.GetStartingSpeed());

        sizeLine = transform.Find("Size").gameObject.GetComponent<GraphLogic>();
        sizeLine.SetStartingValue(worldScript.GetStartingSize());

        visionRangeLine = transform.Find("VisionRange").gameObject.GetComponent<GraphLogic>();
        visionRangeLine.SetStartingValue(worldScript.GetStartingVisionRange());

        idealTempLine = transform.Find("IdealTemp").gameObject.GetComponent<GraphLogic>();
        idealTempLine.SetStartingValue(worldScript.GetStartingIdealTemp());
    }

    public void UpdateValues()
    {
        speedLine.PlotGraphPoint(worldScript.GetMeanSpeed());
        sizeLine.PlotGraphPoint(worldScript.GetMeanSize());
        visionRangeLine.PlotGraphPoint(worldScript.GetMeanVisionRange());
        idealTempLine.PlotGraphPoint(worldScript.GetMeanIdealTemp());
    }


    public float GetGraphHeight()
    {
        return graphHeight;
    }
    public float GetGraphWidth()
    {
        return graphWidth;
    }

    public void Reset()
    {
        speedLine.Reset();
        speedLine.SetStartingValue(worldScript.GetStartingSpeed());

        sizeLine.Reset();
        sizeLine.SetStartingValue(worldScript.GetStartingSize());

        visionRangeLine.Reset();
        visionRangeLine.SetStartingValue(worldScript.GetStartingVisionRange());

        idealTempLine.Reset();
        idealTempLine.SetStartingValue(worldScript.GetStartingIdealTemp());
    }

    public void ToggleVisibility()
    {
        if (graphActive == false)
        {
            graphActive = true;
            LineRenderer[] renderers = gameObject.GetComponentsInChildren<LineRenderer>();
            foreach (LineRenderer lr in renderers)
            {
                lr.enabled = true;
            }
        }
        else
        {
            graphActive = false;
            LineRenderer[] renderers = gameObject.GetComponentsInChildren<LineRenderer>();
            foreach (LineRenderer lr in renderers)
            {
                lr.enabled = false;
            }

        }
    }
}
