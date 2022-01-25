using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldLogic : MonoBehaviour
{
    public GameObject animalPrefab;
    private int numberOfAnimals = 50;

    private List<GameObject> animals = new List<GameObject>();

    private int environmentTemperature = 20;

    private int startingSpeed = 30;
    private float startingVisionRange = 3f;
    private float startingSize = 1f;
    private int startingIdealTemp = 10;

    private float reproductionPercentage = 0.5f;
    private float standardDeviation = 5f;

    public Text populationText;

    public Text speedText;
    public Text sizeText;
    public Text vrText;
    public Text tempText;

    Slider speedSlider;
    Slider sizeSlider;
    Slider VRslider;
    Slider tempSlider;

    public Text captionText;

    bool isDisplayingAverages = true;
    GameObject currentSelection;

    float meanSpeed = 30f;
    float meanSize = 1f;
    float meanVR = 3f;
    float meanTemp = 10f;

    public LineGraphManager graphManagerScript;
    public UserInterface UIscript;

    Animal animalStatsToDisplay;
    

    // Start is called before the first frame update
    void Start()
    {
        SpawnAnimals();
        UIscript = gameObject.GetComponent<UserInterface>();

        speedSlider = speedText.GetComponentInChildren<Slider>();
        sizeSlider = sizeText.GetComponentInChildren<Slider>();
        VRslider = vrText.GetComponentInChildren<Slider>();
        tempSlider = tempText.GetComponentInChildren<Slider>();
        
    }

    private void SpawnAnimals()
    {
        for (int i = 0; i < numberOfAnimals; i++)
        {
            GameObject animal = Instantiate(animalPrefab);
            animals.Add(animal);

            AnimalLogic animalScript = animal.GetComponent<AnimalLogic>();
            animalScript.SetValues(startingSpeed, startingVisionRange, startingSize, startingIdealTemp, 100f);
            animalScript.ToggleEnergyDeduction(false);

            animalScript.isFirstGeneration = true;
        }
    }

    public void SetSelection(GameObject obj)
    {
        currentSelection = obj;
        isDisplayingAverages = false;
        captionText.text = "Individual:";
        UIscript.SetSelectionMode(true, currentSelection);
        animalStatsToDisplay = currentSelection.GetComponent<AnimalLogic>().GetAnimalStats();
    }
    public void TurnOffSelection()
    {
        isDisplayingAverages = true;
        captionText.text = "Average:";
        UIscript.SetSelectionMode(false, null);
    }

    private void Update()
    {
        populationText.text = animals.Count.ToString();


        meanSpeed = 0f;
        meanSize = 0f;
        meanVR = 0f;
        meanTemp = 0f;
        for (int i = 0; i < animals.Count; i++)
        {
            int speed = animals[i].GetComponent<AnimalLogic>().GetSpeed();
            float size = animals[i].GetComponent<AnimalLogic>().GetSize();
            float visionRange = animals[i].GetComponent<AnimalLogic>().GetVisionRange();
            int temperature = animals[i].GetComponent<AnimalLogic>().GetTemperature();

            meanSpeed += speed;
            meanSize += size;
            meanVR += visionRange;
            meanTemp += temperature;
        }

        meanSpeed = meanSpeed / animals.Count;
        meanSize = meanSize / animals.Count;
        meanVR = meanVR / animals.Count;
        meanTemp = meanTemp / animals.Count;



        if (isDisplayingAverages)
        {
            speedText.text = "Speed: " + Math.Round(meanSpeed, 2).ToString();
            sizeText.text = "Size: " + Math.Round(meanSize, 2).ToString();
            vrText.text = "Sight Range: " + Math.Round(meanVR, 2).ToString();
            tempText.text = "Ideal\nTemperature: " + Math.Round(meanTemp, 2).ToString();
        }
        else
        {
            if (currentSelection == null)
            {
                TurnOffSelection();
            }
            else
            {
                float maxSpeedDifference = 0;
                float maxSizeDifference = 0;
                float maxVRdifference = 0;
                float maxTempDifference = 0;

                speedText.text = "Speed: " + animalStatsToDisplay.GetSpeed();
                sizeText.text = "Size: " + Math.Round(animalStatsToDisplay.GetSize(), 2).ToString();
                vrText.text = "Sight Range: " + Math.Round(animalStatsToDisplay.GetVisionRange(), 2).ToString();
                tempText.text = "Ideal\nTemperature: " + animalStatsToDisplay.GetIdealTemp().ToString();

                for (int i = 0; i < animals.Count; i++)
                {
                    int speed = animals[i].GetComponent<AnimalLogic>().GetSpeed();
                    float size = animals[i].GetComponent<AnimalLogic>().GetSize();
                    float visionRange = animals[i].GetComponent<AnimalLogic>().GetVisionRange();
                    int temperature = animals[i].GetComponent<AnimalLogic>().GetTemperature();

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

                float speedDisplayValue = (animalStatsToDisplay.GetSpeed()-meanSpeed) / maxSpeedDifference;
                speedDisplayValue = 0.5f + 0.5f*(speedDisplayValue);
                speedSlider.value = speedDisplayValue;

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

    public void Reproduce()
    {
        int population = animals.Count;
        for (int i = 0; i < population; i++)
        {
            float randomNumber = UnityEngine.Random.Range(0, 100) / 100f;
            //Debug.Log(randomNumber);
            if (randomNumber < reproductionPercentage && animals[i] != null)
            {
                AnimalLogic animalScript = animals[i].GetComponent<AnimalLogic>();
                animalScript.Reproduce(standardDeviation);
            }
        }
    }

    public float GetMeanSpeed()
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
    public float GetMeanSize()
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
    public float GetMeanVisionRange()
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
    public float GetMeanIdealTemp()
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

    public float GetStartingSpeed()
    {
        return startingSpeed;
    }
    public float GetStartingSize()
    {
        return startingSize;
    }
    public float GetStartingVisionRange()
    {
        return startingVisionRange;
    }
    public float GetStartingIdealTemp()
    {
        return startingIdealTemp;
    }

    public int GetTemperature()
    {
        return environmentTemperature;
    }
    public void AddAnimal(GameObject animal)
    {
        animals.Add(animal);
    }
    public void RemoveAnimal(GameObject animal)
    {
        animals.Remove(animal);
    }

    public void Restart()
    {
        int population = animals.Count;
        for (int i = population-1; i >= 0; i--)
        {
            Destroy(animals[i]);
            animals.RemoveAt(i);
        }

        SpawnAnimals();
        UserInterface UI_script = gameObject.GetComponent<UserInterface>();
        UI_script.Pause();
        GenerationLogic generationScript = gameObject.GetComponent<GenerationLogic>();
        generationScript.Reset();

        graphManagerScript.Reset();
    }

    public void SetNumberOfAnimalsToSpawn(int number)
    {
        numberOfAnimals = number;
    }
    public void SetWorldTemperature(int number)
    {
        environmentTemperature = number;
    }
    public void SetReproductionPercentage(float number)
    {
        reproductionPercentage = number;
    }

    public void SetStandardDeviation(float number)
    {
        standardDeviation = number;
    }


    public void SetStartingSpeed(int number)
    {
        startingSpeed = number;
    }
    public void SetStartingVision(float number)
    {
        startingVisionRange = number;
    }
    public void SetStartingSize(float number)
    {
        startingSize = number;
    }
    public void SetStartingIdealTemp(int number)
    {
        startingIdealTemp = number;
    }


    public void SetDisplayStats(Animal stats)
    {
        animalStatsToDisplay = stats;
    }

}
