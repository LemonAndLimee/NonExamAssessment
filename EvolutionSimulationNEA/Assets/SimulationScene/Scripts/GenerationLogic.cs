using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerationLogic : MonoBehaviour
{
    const float xMin = -3.77f;
    const float xMax = 3.77f;
    const float yMin = -2.97f;
    const float yMax = 4.55f;

    private int numberOfFoodPerGeneration = 50; //temp variable
    private int generationDuration = 5;
    private float timer = 0f;
    private int generationCounter = 0;

    private int numberOfGenerations = 50;

    public Text generationCounterText;

    public GameObject foodPrefab;

    public LineGraphManager graphManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        generationCounterText.text = "-";
    }

    // Update is called once per frame
    void Update()
    {
        if (generationCounter <= numberOfGenerations)
        {
            timer += Time.deltaTime;
            if (timer >= generationDuration)
            {
                SpawnFood(xMin, xMax, yMin, yMax);

                gameObject.GetComponent<WorldLogic>().Reproduce();

                timer = 0f;
                generationCounter++;
                generationCounterText.text = generationCounter.ToString();

                graphManagerScript.UpdateValues();
            }
        }
        else
        {
            Time.timeScale = 0f;
            generationCounterText.text = numberOfGenerations.ToString();
        }
    }

    void SpawnFood(float xBoundary_negative, float xBoundary_positive, float yBoundary_negative, float yBoundary_positive)
    {
        for (int i = 0; i < numberOfFoodPerGeneration; i++)
        {
            GameObject foodObject = Instantiate(foodPrefab);
            Vector3 position = new Vector3(Random.Range(xBoundary_negative, xBoundary_positive), Random.Range(yBoundary_negative, yBoundary_positive), foodObject.transform.position.z);
            foodObject.transform.position = position;
        }
    }

    public int GetGenerationDuration()
    {
        return generationDuration;
    }

    public void Reset()
    {
        timer = 0f;
        generationCounter = 0;

        GameObject[] foodObjects = GameObject.FindGameObjectsWithTag("Food");
        for (int i = foodObjects.Length - 1; i >= 0; i--)
        {
            Destroy(foodObjects[i]);
        }

        generationCounterText.text = "-";
    }

    public void SetNumberOfGenerations(int number)
    {
        numberOfGenerations = number;
    }
    public void SetGenerationDuration(int number)
    {
        generationDuration = number;
    }
    public void SetFoodPerGeneration(int number)
    {
        numberOfFoodPerGeneration = number;
    }
}
