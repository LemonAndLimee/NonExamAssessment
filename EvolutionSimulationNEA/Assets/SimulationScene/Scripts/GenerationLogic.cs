using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationLogic : MonoBehaviour
{
    const float xMin = -3.77f;
    const float xMax = 3.77f;
    const float yMin = -2.97f;
    const float yMax = 4.55f;

    private int numberOfFoodPerGeneration = 500; //temp variable
    private int generationDuration = 5;
    private float timer = 0f;
    private int generationCounter = 0;

    public GameObject foodPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= generationDuration)
        {
            SpawnFood(xMin, xMax, yMin, yMax);

            gameObject.GetComponent<WorldLogic>().Reproduce();

            timer = 0f;
        }
    }

    void SpawnFood(float xBoundary_negative, float xBoundary_positive, float yBoundary_negative, float yBoundary_positive)
    {
        for (int i = 0; i < numberOfFoodPerGeneration; i++)
        {
            GameObject foodObject = Instantiate(foodPrefab);
            Vector3 position = new Vector3(Random.Range(xBoundary_negative, xBoundary_positive), Random.Range(yBoundary_negative, yBoundary_positive), 0f);
            foodObject.transform.position = position;
        }
    }

    public int GetGenerationDuration()
    {
        return generationDuration;
    }
}
