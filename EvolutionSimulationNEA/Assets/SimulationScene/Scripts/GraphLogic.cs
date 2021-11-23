using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GraphLogic : MonoBehaviour
{

    List<float> numbers = new List<float>();
    LineRenderer lr;

    WorldLogic worldScript;

    float graphWidth = 10f;
    float graphHeight = 3f;

    float startingValue = 0f;
    float highestDifference = 0f;


    // Start is called before the first frame update
    void Start()
    {
        lr = gameObject.GetComponent<LineRenderer>();
        worldScript = GameObject.Find("SimulationManager").GetComponent<WorldLogic>();
        
        
    }

    public void SetStartingValue(float value)
    {
        startingValue = value;
    }

    // Update is called once per frame
    void Update()
    {
        if (numbers.Count > 0)
        {
            float maxValue = startingValue;
            float minValue = startingValue;

            highestDifference = startingValue;

            for (int i = 0; i < numbers.Count; i++)
            {
                if (numbers[i] > maxValue)
                {
                    maxValue = numbers[i];
                }
                if (numbers[i] < minValue)
                {
                    minValue = numbers[i];
                }
            }

            if (startingValue - minValue > maxValue - startingValue)
            {
                highestDifference = startingValue - minValue;
            }
            else
            {
                highestDifference = maxValue - startingValue;
            }

            lr.positionCount = numbers.Count;
            float interval = graphWidth;
            if (numbers.Count > 1)
            {
                interval = graphWidth / (numbers.Count - 1);
            }
            for (int i = 0; i < numbers.Count; i++)
            {
                float x = i * interval;
                float y = 0f;
                if (numbers[i] > startingValue)
                {
                    y = (numbers[i]-startingValue) / highestDifference * graphHeight;
                }
                else if (numbers[i] < startingValue)
                {
                    y = -((startingValue-numbers[i]) / highestDifference * graphHeight);
                }
                lr.SetPosition(i, new Vector3(x, y));
                //Debug.Log(numbers[i]);
            }
        }
        else
        {
            lr.positionCount = 1;
            lr.SetPosition(0, new Vector3(0, 0f));
        }
    }

    public void PlotMeanSpeed()
    {
        float meanSpeed = worldScript.GetMeanSpeed();
        PlotGraphPoint(meanSpeed);
    }

    void PlotGraphPoint(float value)
    {
        numbers.Add(value);
        Debug.Log(value);
    }
}
