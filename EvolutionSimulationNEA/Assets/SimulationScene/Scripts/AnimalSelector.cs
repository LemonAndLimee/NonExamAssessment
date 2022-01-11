using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSelector : MonoBehaviour
{
    GameObject currentSelection;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //if (currentSelection != null)
            //{
                //currentSelection.GetComponent<AnimalLogic>().ToggleSelectedMode();
                //currentSelection = null;
            //}

            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePosition2D = new Vector2(worldMousePosition.x, worldMousePosition.y);

            RaycastHit2D raycastHit = Physics2D.Raycast(mousePosition2D, Vector2.zero);

            if (raycastHit.collider != null && raycastHit.collider.gameObject.tag == "Animal")
            {
                currentSelection = raycastHit.collider.gameObject;
                currentSelection.GetComponent<AnimalLogic>().ToggleSelectedMode();
            }
            
        }
    }

}
