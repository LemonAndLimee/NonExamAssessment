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
    public Text captionText;

    bool isDisplayingAverages = true;
    GameObject currentSelection;

    float meanSpeed = 30f;
    float meanSize = 1f;
    float meanVR = 3f;
    float meanTemp = 10f;

    public LineGraphManager graphManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        SpawnAnimals();
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
    }
    public void TurnOffSelection()
    {
        isDisplayingAverages = true;
        captionText.text = "Average:";
    }

    private void Update()
    {
        populationText.text = animals.Count.ToString();

        if (isDisplayingAverages)
        {
            meanSpeed = 0f;
            meanSize = 0f;
            meanVR = 0f;
            meanTemp = 0f;
            for (int i = 0; i < animals.Count; i++)
            {
                meanSpeed += animals[i].GetComponent<AnimalLogic>().GetSpeed();
                meanSize += animals[i].GetComponent<AnimalLogic>().GetSize();
                meanVR += animals[i].GetComponent<AnimalLogic>().GetVisionRange();
                meanTemp += animals[i].GetComponent<AnimalLogic>().GetTemperature();
            }
            meanSpeed = meanSpeed / animals.Count;
            meanSize = meanSize / animals.Count;
            meanVR = meanVR / animals.Count;
            meanTemp = meanTemp / animals.Count;

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
                AnimalLogic animalScript = currentSelection.GetComponent<AnimalLogic>();
                speedText.text = "Speed: " + animalScript.GetSpeed().ToString();
                sizeText.text = "Size: " + Math.Round(animalScript.GetSize(), 2).ToString();
                vrText.text = "Sight Range: " + Math.Round(animalScript.GetVisionRange(), 2).ToString();
                tempText.text = "Ideal\nTemperature: " + animalScript.GetTemperature().ToString();
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
}
