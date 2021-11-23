using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGraphManager : MonoBehaviour
{
    WorldLogic worldScript;

    int startingSpeed;
    GameObject speedLine;

    // Start is called before the first frame update
    void Start()
    {
        worldScript = GameObject.Find("SimulationManager").GetComponent<WorldLogic>();
        startingSpeed = worldScript.GetStartingSpeed();

        speedLine = transform.Find("Speed").gameObject;
        speedLine.GetComponent<GraphLogic>().SetStartingValue((float)startingSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
