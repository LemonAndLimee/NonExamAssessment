using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GraphLogic : MonoBehaviour
{

    List<float> numbers = new List<float>();

    LineRenderer lr;

    float startingValue = 0f;
    float highestDifference = 0f;

    LineGraphManager graphManagerScript;


    // Start is called before the first frame update
    void Start()
    {
        lr = gameObject.GetComponent<LineRenderer>();
        graphManagerScript = GetComponentInParent<LineGraphManager>();
    }

    public void SetStartingValue(float value)
    {
        startingValue = value;
        numbers.Add(value);
    }

    // Update is called once per frame
    void Update()
    {
        if (numbers.Count > 1)
        {
            lr.positionCount = numbers.Count;
            float interval = graphManagerScript.GetGraphWidth() / (numbers.Count - 1f);
            for (int i = 0; i < numbers.Count; i++)
            {
                float x = i * interval;
                float y = 0f;
                if (highestDifference != 0)
                {
                    y = graphManagerScript.GetGraphHeight() * ((numbers[i] - startingValue) / highestDifference);
                }
                lr.SetPosition(i, new Vector3(x, y, 0f));
            }
        }
    }

    public void PlotGraphPoint(float value)
    {
        numbers.Add(value);
        CheckDifference(value);
        Debug.Log(value);
    }

    void CheckDifference(float value)
    {
        float difference = Mathf.Abs(startingValue - value);
        if (difference > highestDifference)
        {
            highestDifference = difference;
        }
    }

    public void Reset()
    {
        numbers.Clear();
        lr.positionCount = 0;
    }
}
