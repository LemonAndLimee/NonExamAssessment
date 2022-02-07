using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script manages an individual graph line
public class GraphLogic : MonoBehaviour
{
    List<float> numbers = new List<float>(); //stores all the values to be represented by the graph

    LineRenderer lr; //the LineRenderer of the gameObject attached to the script

    float startingValue = 0f; //the starting value of the line
    //used to store the highest difference between the starting value and any of the points
    //this is used for calculating what height all the points are at
    float highestDifference = 0f; 

    LineGraphManager graphManagerScript; //graph manager script

    //called at the start
    void Start()
    {
        lr = gameObject.GetComponent<LineRenderer>(); //assigns lr to the LineRenderer of the gameObject attached to the script
        graphManagerScript = GetComponentInParent<LineGraphManager>(); //assigns the graph manager script
    }

    public void SetStartingValue(float value) //used to set the starting value of the line
    {
        startingValue = value;
        numbers.Add(value); //adds starting value to the list of values
    }

    //called every frame
    void Update()
    {
        //if there are more than one values in numbers
        if (numbers.Count > 1)
        {
            lr.positionCount = numbers.Count; //sets the positionCount of the LineRenderer to the length of numbers
            float interval = graphManagerScript.GetGraphWidth() / (numbers.Count - 1f); //calculates the interval on the x-axis between points
            for (int i = 0; i < numbers.Count; i++) //for every value in numbers
            {
                float x = i * interval; //calculates the x value
                float y = 0f; //declares y value
                if (highestDifference != 0) //if the values of the graph are not all equal to starting value
                {
                    //calculates y position:
                    //set y position equal to graph height * (difference between current value and starting value as a percentage out of highestDifference)
                    y = graphManagerScript.GetGraphHeight() * ((numbers[i] - startingValue) / highestDifference);
                }
                //plots the point on the LineRenderer
                lr.SetPosition(i, new Vector3(x, y, 0f));
            }
        }
    }

    //used to add a new value to the graph
    public void PlotGraphPoint(float value)
    {
        numbers.Add(value); //adds value to numbers
        CheckDifference(value); //checks to see if it has a new highest difference
    }


    //checks value agains current highest difference value
    void CheckDifference(float value)
    {
        //calculates difference between value and starting value of the graph line
        float difference = Mathf.Abs(startingValue - value);
        //if this difference is greater than highestDifference, set highestDifference to the new difference
        if (difference > highestDifference)
        {
            highestDifference = difference;
        }
    }

    //resets graph
    public void Reset()
    {
        numbers.Clear(); //clears list of numbers
        lr.positionCount = 0; //clears all the LineRenderer points
    }

    //toggles the visibility of the graph line
    public void ToggleVisibility()
    {
        //if line renderer is enabled, disable line renderer
        if (lr.enabled == true)
        {
            lr.enabled = false;
        }
        //if line renderer is disabled, enable line renderer
        else
        {
            lr.enabled = true;
        }
    }
}
