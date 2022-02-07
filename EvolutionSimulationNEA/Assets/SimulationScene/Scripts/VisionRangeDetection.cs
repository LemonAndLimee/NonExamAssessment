using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script manages the vision range of the animal, and how it "sees" food
public class VisionRangeDetection : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) //when the vision range object collides with another collider
    {
        if (collision.gameObject.tag == "Food") //if the object collided with had a "Food" tag
        {
            //sets movementScript to the AnimalMovement script of the parent animal object
            // (this script is attached to the vision range object, not the animal itself)
            AnimalMovement movementScript = GetComponentInParent<AnimalMovement>();
            //calls on the movement script to target the food object
            movementScript.TargetFood(collision.gameObject);
        }
    }
}
