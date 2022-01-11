using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSelector : MonoBehaviour
{
    GameObject currentSelection;
    GameObject previousSelection;

    WorldLogic worldScript;

    void Start()
    {
        worldScript = gameObject.GetComponent<WorldLogic>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePosition2D = new Vector2(worldMousePosition.x, worldMousePosition.y);

            RaycastHit2D raycastHit = Physics2D.Raycast(mousePosition2D, Vector2.zero);

            if (raycastHit.collider != null)
            {
                if (raycastHit.collider.gameObject.tag == "Animal")
                {
                    previousSelection = currentSelection;
                    currentSelection = raycastHit.collider.gameObject;
                }
                else if (raycastHit.collider.gameObject.tag == "AnimalVision")
                {
                    previousSelection = currentSelection;
                    currentSelection = raycastHit.collider.transform.parent.gameObject;
                }

                if (currentSelection.tag == "Animal" || currentSelection.tag == "AnimalVision")
                {
                    currentSelection.GetComponent<AnimalLogic>().ToggleSelectedMode();
                    worldScript.SetSelection(currentSelection);

                    if (previousSelection != null && previousSelection != currentSelection)
                    {
                        previousSelection.GetComponent<AnimalLogic>().ToggleSelectedMode();
                    }
                    else if (previousSelection == currentSelection)
                    {
                        worldScript.TurnOffSelection();
                        currentSelection = null;
                    }

                    previousSelection = currentSelection;
                }
                
            }
            
        }
    }


}
