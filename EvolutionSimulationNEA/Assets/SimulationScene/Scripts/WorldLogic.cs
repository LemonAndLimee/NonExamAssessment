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

    float meanSpeed = 30f;
    float meanSize = 1f;
    float meanVR = 3f;
    float meanTemp = 10f;

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
        }
    }

    private void Update()
    {
        populationText.text = "Population: " + animals.Count.ToString();

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

        speedText.text = "Speed: " + meanSpeed.ToString();
        sizeText.text = "Size: " + meanSize.ToString();
        vrText.text = "VR: " + meanVR.ToString();
        tempText.text = "Temp: " + meanTemp.ToString();
    }

    public void Reproduce()
    {
        int population = animals.Count;
        for (int i = 0; i < population; i++)
        {
            float randomNumber = Random.Range(0, 100) / 100f;
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

    public int GetStartingSpeed()
    {
        return startingSpeed;
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
