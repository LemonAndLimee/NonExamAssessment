using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//this script manages the passing of generations, the timescale, and the spawning of food
public class GenerationLogic : MonoBehaviour
{
    //sets the boundaries for spawning food on the environment
    const float xMin = -3.77f;
    const float xMax = 3.77f;
    const float yMin = -2.97f;
    const float yMax = 4.55f;

    //stores number of food objects that will spawn each generation
    private int numberOfFoodPerGeneration = 100; 
    private int generationDuration = 5; //length of generation in seconds
    private float timer = 0f; //timer value
    private int generationCounter = 0; //generation counter 

    private int numberOfGenerations = 50; //number of generations to be carried out

    public Text generationCounterText; //the text component that displays the generation counter value

    public GameObject foodPrefab; //stores food prefab

    public LineGraphManager graphManagerScript; //stores graph manager script

    //called at the start
    void Start()
    {
        generationCounterText.text = "-"; //sets generation counter to a non-number value
    }

    //called every frame
    void Update()
    {
        if (generationCounter <= numberOfGenerations) //if generation counter has not reached number of generations to be carried out
        {
            timer += Time.deltaTime; //increases timer in real time
            if (timer >= generationDuration) //if timer surpasses generation length
            {
                SpawnFood(xMin, xMax, yMin, yMax); //spawns food objects

                gameObject.GetComponent<WorldLogic>().Reproduce(); //calls the Reproduce subroutine in the WorldLogic script

                timer = 0f; //resets timer
                generationCounter++; //increments generation counter by 1
                generationCounterText.text = generationCounter.ToString(); //sets generation counter text to the number stored in generationCounter

                //updates the graph values
                graphManagerScript.UpdateValues();
            }
        }
        else //if generation counter reaches number of generations to be carried out
        {
            Time.timeScale = 0f; //freezes the time scale
            generationCounterText.text = numberOfGenerations.ToString(); //updates the generation counter text
        }
    }

    //spawns wave of food object
    void SpawnFood(float xBoundary_negative, float xBoundary_positive, float yBoundary_negative, float yBoundary_positive)
    {
        for (int i = 0; i < numberOfFoodPerGeneration; i++) //repeats the number of times specified by numberOfFoodPerGeneration
        {
            GameObject foodObject = Instantiate(foodPrefab); //creates a food object as a clone of the food prefab
            //creates a random vector3 position within the specified boundaries
            Vector3 position = new Vector3(Random.Range(xBoundary_negative, xBoundary_positive), Random.Range(yBoundary_negative, yBoundary_positive), foodObject.transform.position.z);
            //sets the food object position to the position created
            foodObject.transform.position = position;
        }
    }

    //returns generation length
    public int GetGenerationDuration()
    {
        return generationDuration;
    }

    //resets the simulation
    public void Reset()
    {
        timer = 0f; //resets timer
        generationCounter = 0; //resets generation counter

        GameObject[] foodObjects = GameObject.FindGameObjectsWithTag("Food"); //creates list of all food objects
        for (int i = foodObjects.Length - 1; i >= 0; i--) //for every food object, destroy said object
        {
            Destroy(foodObjects[i]);
        }

        generationCounterText.text = "-"; //sets generation counter text to non-number value
    }

    //used to set the numberOfGenerations value
    public void SetNumberOfGenerations(int number)
    {
        numberOfGenerations = number;
    }
    //used to set the generationDuration value
    public void SetGenerationDuration(int number)
    {
        generationDuration = number;
    }
    //used to set the numberOfFoodPerGeneration value
    public void SetFoodPerGeneration(int number)
    {
        numberOfFoodPerGeneration = number;
    }
}
