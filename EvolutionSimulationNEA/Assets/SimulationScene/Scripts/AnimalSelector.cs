using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script manages the selection of animals
public class AnimalSelector : MonoBehaviour
{
    GameObject currentSelection; //used for storing currently selected animal
    GameObject previousSelection; //stores previous selection

    WorldLogic worldScript; //stores WorldLogic script

    //called at the start
    void Start()
    {
        worldScript = gameObject.GetComponent<WorldLogic>(); //assigns WorldLogic script
    }

    //called every frame
    private void Update()
    {
        //if left mouse button is clicked down
        if (Input.GetMouseButtonDown(0))
        {
            //converts mouse position into world coordinates
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //changes the position from vector3 to vector2
            Vector2 mousePosition2D = new Vector2(worldMousePosition.x, worldMousePosition.y);

            //sends raycast ray from the mouse position at the screen
            RaycastHit2D raycastHit = Physics2D.Raycast(mousePosition2D, Vector2.zero);

            //if the raycast hits something
            if (raycastHit.collider != null)
            {
                if (raycastHit.collider.gameObject.tag == "Animal") //if it hits an animal
                {
                    previousSelection = currentSelection; //sets previous selection to object stored in current selection
                    currentSelection = raycastHit.collider.gameObject; //sets current selection to the object the raycast hit
                }
                else if (raycastHit.collider.gameObject.tag == "AnimalVision") //if it hits an animal vision object
                {
                    previousSelection = currentSelection; //sets previous selection to object stored in current selection
                    //sets current selection to the animal parent object of the vision range object
                    currentSelection = raycastHit.collider.transform.parent.gameObject; 
                }

                //if selection is an animal
                if (currentSelection.tag == "Animal")
                {
                    //tells AnimalLogic script to toggle the selection mode of the animal
                    currentSelection.GetComponent<AnimalLogic>().ToggleSelectedMode();
                    //sets the selection variable in the WorldLogic script to the current selection object
                    worldScript.SetSelection(currentSelection);

                    //if the previous selection was a unique object to the current selection
                    if (previousSelection != null && previousSelection != currentSelection)
                    {
                        //turns off selected mode in the previous selection
                        previousSelection.GetComponent<AnimalLogic>().ToggleSelectedMode();
                    }
                    else if (previousSelection == currentSelection) //if the user clicked the animal that was selected
                    {
                        //turn off selection in WorldLogic script
                        worldScript.TurnOffSelection();
                        //clears currentSelection variable
                        currentSelection = null;
                    }

                }
                
            }
            
        }
    }


}
