using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionRangeDetection : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Food")
        {
            AnimalMovement movementScript = GetComponentInParent<AnimalMovement>();
            movementScript.TargetFood(collision.gameObject);
        }
    }
}
