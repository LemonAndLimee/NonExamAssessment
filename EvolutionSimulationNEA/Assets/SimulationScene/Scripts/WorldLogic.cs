using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//this script manages the spawning of animals, the storing of simulation variables, and the calculating and displaying of stats
public class WorldLogic : MonoBehaviour
{
    public GameObject animalPrefab; //the prefab of the animal game object
    private int numberOfAnimals = 50; //number of animals to be spawned at the start of the simulation, default value 50

    private List<GameObject> animals = new List<GameObject>(); //list storing all the alive animals

    private int environmentTemperature = 20; //environment temperature, default value 20

    private int startingSpeed = 30; //starting animal speed, default value 30
    private float startingVisionRange = 3f; //starting animal vision range, default value 3.0
    private float startingSize = 1f; //starting animal size, default value 1.0
    private int startingIdealTemp = 20; //starting animal ideal temperature, default value 20

    private float reproductionPercentage = 0.5f; //proportion of animals to reproduce each generation, default value 0.5
    private float standardDeviation = 5f; //standard deviation for the mutation of characteristics, default value 5.0

    public Text populationText; //text component used to display current population

    public Text speedText; //text component for displaying speed
    public Text sizeText; //text component for displaying size
    public Text vrText; //text component for displaying vision range
    public Text tempText; //text component for displaying ideal temperature

    Slider speedSlider; //slider component for displaying speed
    Slider sizeSlider; //slider component for displaying size
    Slider VRslider; //slider component for displaying vision range
    Slider tempSlider; //slider component for displaying ideal temperature

    public Text captionText; //caption of the left data panel

    bool isDisplayingAverages = true; //true if displaying population average, false if displaying stats of the selected animal
    GameObject currentSelection; //currently selected animal

    float meanSpeed = 30f; //mean speed value
    float meanSize = 1f; //mean size value
    float meanVR = 3f; //mean vision range value
    float meanTemp = 10f; //mean ideal temperature value

    public LineGraphManager graphManagerScript; //graph manager script
    public UserInterface UIscript; //UI manager script

    Animal animalStatsToDisplay; //Animal data type - stores stats to be displayed

    //called at the start
    void Start()
    {
        SpawnAnimals(); //spawns the animals
        UIscript = gameObject.GetComponent<UserInterface>(); //assigns UI script

        //assigns slider components
        speedSlider = speedText.GetComponentInChildren<Slider>(); 
        sizeSlider = sizeText.GetComponentInChildren<Slider>();
        VRslider = vrText.GetComponentInChildren<Slider>();
        tempSlider = tempText.GetComponentInChildren<Slider>();
        
    }

    private void SpawnAnimals() //spawns animals
    {
        for (int i = 0; i < numberOfAnimals; i++) //repeats for the number of animals to spawn
        {
            GameObject animal = Instantiate(animalPrefab); //instantiates a copy of the animal prefab, assigns it to gameObject animal
            animals.Add(animal); //adds animal to the list of animals

            //sets animalScript to the AnimalLogic script of the instantiated animal object
            AnimalLogic animalScript = animal.GetComponent<AnimalLogic>();
            //sets animal values to the starting simulation values
            animalScript.SetValues(startingSpeed, startingVisionRange, startingSize, startingIdealTemp, 100f);
            //as this is the starting generation, sets isFirstGeneration to true
            animalScript.isFirstGeneration = true;
        }
    }

    public void SetSelection(GameObject obj) //sets selection to the animal clicked on
    {
        currentSelection = obj; //sets currentSelection to the object clicked on
        isDisplayingAverages = false; //sets left data panel to individual displaying mode
        captionText.text = "Individual:"; //changes caption to individual mode
        UIscript.SetSelectionMode(true, currentSelection); //sets selection mode to individual, with currentSelection stats
        //sets animalStatsToDisplay to the Animal storing stats of the currently selected animal
        animalStatsToDisplay = currentSelection.GetComponent<AnimalLogic>().GetAnimalStats();
    }
    public void TurnOffSelection() //turns off selection
    {
        isDisplayingAverages = true; //sets left data panel to averages mode
        captionText.text = "Average:"; //changes caption to averages mode
        UIscript.SetSelectionMode(false, null); //turns off selection mode

        //sets slider values to default
        speedSlider.value = 0.5f;
        sizeSlider.value = 0.5f;
        VRslider.value = 0.5f;
        tempSlider.value = 0.5f;
    }

    //called every frame
    private void Update()
    {
        populationText.text = animals.Count.ToString(); //sets populationText to the population count

        //sets mean values of each trait to zero
        meanSpeed = 0f;
        meanSize = 0f;
        meanVR = 0f;
        meanTemp = 0f;

        for (int i = 0; i < animals.Count; i++) //loops through every animal
        {
            //declares variables for each trait of the current animal
            int speed = animals[i].GetComponent<AnimalLogic>().GetSpeed();
            float size = animals[i].GetComponent<AnimalLogic>().GetSize();
            float visionRange = animals[i].GetComponent<AnimalLogic>().GetVisionRange();
            int temperature = animals[i].GetComponent<AnimalLogic>().GetTemperature();

            //adds the values from the current animal to the mean value
            meanSpeed += speed;
            meanSize += size;
            meanVR += visionRange;
            meanTemp += temperature;
        }

        //sets mean value equal to the current mean value (the total) divided by the population count
        meanSpeed = meanSpeed / animals.Count;
        meanSize = meanSize / animals.Count;
        meanVR = meanVR / animals.Count;
        meanTemp = meanTemp / animals.Count;

        //if left data panel is in averages mode
        if (isDisplayingAverages)
        {
            //sets all the text components to display the mean values to 2 decimal places
            speedText.text = "Speed: " + Math.Round(meanSpeed, 2).ToString();
            sizeText.text = "Size: " + Math.Round(meanSize, 2).ToString();
            vrText.text = "Sight Range: " + Math.Round(meanVR, 2).ToString();
            tempText.text = "Ideal\nTemperature: " + Math.Round(meanTemp, 2).ToString();
        }
        else //if left data panel is in individual selection mode
        {
            if (currentSelection == null) //if the current selection object does not exist (e.g. if the animal dies)
            {
                TurnOffSelection(); //turn off selection
            }  
            else //if the current selection animal exists
            {
                //declares maxDifference variables, sets them to zero
                float maxSpeedDifference = 0;
                float maxSizeDifference = 0;
                float maxVRdifference = 0;
                float maxTempDifference = 0;

                //sets the text components to display the currently selected animal stats to 2 decimal places
                speedText.text = "Speed: " + animalStatsToDisplay.GetSpeed();
                sizeText.text = "Size: " + Math.Round(animalStatsToDisplay.GetSize(), 2).ToString();
                vrText.text = "Sight Range: " + Math.Round(animalStatsToDisplay.GetVisionRange(), 2).ToString();
                tempText.text = "Ideal\nTemperature: " + animalStatsToDisplay.GetIdealTemp().ToString();

                for (int i = 0; i < animals.Count; i++) //loops through each animal
                {
                    //declares variables for each trait of the current animal
                    int speed = animals[i].GetComponent<AnimalLogic>().GetSpeed();
                    float size = animals[i].GetComponent<AnimalLogic>().GetSize();
                    float visionRange = animals[i].GetComponent<AnimalLogic>().GetVisionRange();
                    int temperature = animals[i].GetComponent<AnimalLogic>().GetTemperature();

                    //if the difference between the current animal trait and the mean value is higher than the maxDifference value:
                    //sets maxDifference value to equal to the current difference
                    if (Math.Abs(speed - meanSpeed) > maxSpeedDifference)
                    {
                        maxSpeedDifference = Math.Abs(speed - meanSpeed);
                    }
                    if (Math.Abs(size - meanSize) > maxSizeDifference)
                    {
                        maxSizeDifference = Math.Abs(size - meanSize);
                    }
                    if (Math.Abs(visionRange - meanVR) > maxVRdifference)
                    {
                        maxVRdifference = Math.Abs(visionRange - meanVR);
                    }
                    if (Math.Abs(temperature - meanTemp) > maxTempDifference)
                    {
                        maxTempDifference = Math.Abs(temperature - meanTemp);
                    }

                }

                //sets display value to the difference between the value and the mean value as a fraction over the maxDifference
                float speedDisplayValue = (animalStatsToDisplay.GetSpeed()-meanSpeed) / maxSpeedDifference;
                //calculates the slider value, as it is centred around 0.5f
                speedDisplayValue = 0.5f + 0.5f*(speedDisplayValue);
                //sets slider value to speedDisplayValue
                speedSlider.value = speedDisplayValue;

                //repeats process for other three characteristics
                float sizeDisplayValue = (animalStatsToDisplay.GetSize() - meanSize) / maxSizeDifference;
                sizeDisplayValue = 0.5f + 0.5f * sizeDisplayValue;
                sizeSlider.value = sizeDisplayValue;

                float VRdisplayValue = (animalStatsToDisplay.GetVisionRange() - meanVR) / maxVRdifference;
                VRdisplayValue = 0.5f + 0.5f * VRdisplayValue;
                VRslider.value = VRdisplayValue;

                float tempDisplayValue = (animalStatsToDisplay.GetIdealTemp() - meanTemp) / maxTempDifference;
                tempDisplayValue = 0.5f + 0.5f * tempDisplayValue;
                tempSlider.value = tempDisplayValue;
            }
        }
    }

    public void Reproduce() //calls the animals to reproduce
    {
        int population = animals.Count; //sets variable population to the population count
        for (int i = 0; i < population; i++) //repeats for every animal in the population
        {
            float randomNumber = UnityEngine.Random.Range(0, 100) / 100f; //generates random float between 0 and 1
            //if the random number is less than the reproductionPercentage value AND the animal exists
            if (randomNumber < reproductionPercentage && animals[i] != null)
            {
                //sets animalScript to the AnimalLogic script of the current animal
                AnimalLogic animalScript = animals[i].GetComponent<AnimalLogic>();
                //calls for the animal to reproduce, using current standardDeviation value
                animalScript.Reproduce(standardDeviation);
            }
        }
    }

    public float GetMeanSpeed() //returns mean speed
    {
        if (animals.Count > 0)
        {
            return meanSpeed;
        }
        else
        {
            return 0f;
        }
    }
    public float GetMeanSize() //returns mean size
    {
        if (animals.Count > 0)
        {
            return meanSize;
        }
        else
        {
            return 0f;
        }
    }
    public float GetMeanVisionRange() //returns mean vision range
    {
        if (animals.Count > 0)
        {
            return meanVR;
        }
        else
        {
            return 0f;
        }
    }
    public float GetMeanIdealTemp() //returns mean ideal temperature
    {
        if (animals.Count > 0)
        {
            return meanTemp;
        }
        else
        {
            return 0f;
        }
    }

    public float GetStartingSpeed() //returns starting speed
    {
        return startingSpeed;
    }
    public float GetStartingSize() //returns starting size
    {
        return startingSize;
    }
    public float GetStartingVisionRange() //returns starting vision range
    {
        return startingVisionRange;
    }
    public float GetStartingIdealTemp() //returns starting ideal temperature
    {
        return startingIdealTemp;
    }

    public int GetTemperature() //returns environment temperature value
    {
        return environmentTemperature;
    }
    public void AddAnimal(GameObject animal) //adds animal to the list of animals
    {
        animals.Add(animal);
    }
    public void RemoveAnimal(GameObject animal) //removes animal from list of animals
    {
        animals.Remove(animal);
    }

    public void Restart() //restarts simulation
    {
        int population = animals.Count; //sets population variable equal to population count
        for (int i = population-1; i >= 0; i--) //loops through the list of animals, in reverse order so that it can destroy the animals
        {
            Destroy(animals[i]); //destroys animal game object
            animals.RemoveAt(i); //removes animal from list
        }

        SpawnAnimals(); //spawns starting wave of animals
        UIscript.Pause(); //pauses simulation
        GenerationLogic generationScript = gameObject.GetComponent<GenerationLogic>(); //sets generationScript to GenerationLogic script
        generationScript.Reset(); //resets generation count

        graphManagerScript.Reset(); //resets graph
    }

    public void SetNumberOfAnimalsToSpawn(int number)//sets starting number of animals
    {
        numberOfAnimals = number;
    }
    public void SetWorldTemperature(int number) //sets environment temperature
    {
        environmentTemperature = number;
    }
    public void SetReproductionPercentage(float number) //sets reproduction percentage
    {
        reproductionPercentage = number;
    }
    public void SetStandardDeviation(float number) //sets standard deviation value
    {
        standardDeviation = number;
    }


    public void SetStartingSpeed(int number) //sets starting speed
    {
        startingSpeed = number;
    }
    public void SetStartingVision(float number) //sets starting vision range
    {
        startingVisionRange = number;
    }
    public void SetStartingSize(float number) //sets starting size
    {
        startingSize = number;
    }
    public void SetStartingIdealTemp(int number) //sets starting ideal temperature
    {
        startingIdealTemp = number;
    }


    public void SetDisplayStats(Animal stats) //sets stats to display
    {
        animalStatsToDisplay = stats;
    }

}
