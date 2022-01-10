using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSelector : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePosition2D = new Vector2(worldMousePosition.x, worldMousePosition.y);

            RaycastHit2D raycastHit = Physics2D.Raycast(mousePosition2D, Vector2.zero);

            if (raycastHit.collider != null && raycastHit.collider.gameObject.tag == "Animal")
            {
                AnimalLogic animalScript = raycastHit.collider.gameObject.GetComponent<AnimalLogic>();
                Debug.Log(animalScript.parents[0].GetSpeed());
            }
            
        }
    }

}
