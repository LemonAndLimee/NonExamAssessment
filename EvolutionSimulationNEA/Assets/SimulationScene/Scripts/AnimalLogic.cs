using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Animal
{
    GameObject animalObject;
    int speed;
    float visionRange;
    float size;
    int idealTemp;

    public Animal(GameObject obj, int animalSpeed, float vision, float animalSize, int temp)
    {
        animalObject = obj;
        speed = animalSpeed;
        visionRange = vision;
        size = animalSize;
        idealTemp = temp;
    } 

    public GameObject GetObject()
    {
        return animalObject;
    }
    public int GetSpeed()
    {
        return speed;
    }
    public float GetVisionRange()
    {
        return visionRange;
    }
    public float GetSize()
    {
        return size;
    }
    public int GetIdealTemp()
    {
        return idealTemp;
    }
}

public class AnimalLogic : MonoBehaviour
{
    const float defaultSize = 0.172f;
    const float energyCap = 200f;

    public int speed;
    public float visionRange;
    public float size;
    public int idealTemperature;

    public float energy = 100f;
    private bool deductEnergy = true;
    private float timer = 0f;
    private int generationTime;

    public List<Animal> parents = new List<Animal>();

    public bool isFirstGeneration;

    public bool isSelected;

    private void Start()
    {
        generationTime = GameObject.Find("SimulationManager").GetComponent<GenerationLogic>().GetGenerationDuration();
    }
    public void SetValues(int speedValue, float visionRangeValue, float sizeValue, int idealTemperatureValue, float energyValue)
    {
        speed = speedValue;
        gameObject.GetComponent<AnimalMovement>().SetSpeed(speed);

        visionRange = visionRangeValue;
        transform.GetChild(0).localScale = new Vector3(visionRange, visionRange, 1);

        size = sizeValue;
        transform.localScale = new Vector3(size*defaultSize, size*defaultSize, 1f);
        gameObject.GetComponent<AnimalMovement>().SetSize(size);

        idealTemperature = idealTemperatureValue;

        energy = energyValue;


    }

    public void Reproduce(float standardDeviation)
    {
        int childSpeed = Mathf.RoundToInt(GenerateNormalValue((float)speed, standardDeviation, 1f, 100f));
        float childSize = GenerateNormalValue(size, standardDeviation, 0.25f, 5f);
        float childVisionRange = GenerateNormalValue(visionRange, standardDeviation, 1f, 10f);
        int childIdealTemp = Mathf.RoundToInt(GenerateNormalValue(idealTemperature, standardDeviation, -20f, 50f));

        //Debug.Log(childSpeed.ToString() + " " + childSize.ToString() + " " + childVisionRange.ToString() + " " + childIdealTemp.ToString());

        energy = energy / 2f;

        GameObject childObject = Instantiate(gameObject);
        AnimalLogic animalScript = childObject.GetComponent<AnimalLogic>();
        animalScript.SetValues(childSpeed, childVisionRange, childSize, childIdealTemp, energy);
        animalScript.isFirstGeneration = false;

        Animal animal = new Animal(gameObject, speed, visionRange, size, idealTemperature);
        animalScript.parents.Add(animal);

        if (parents.Count > 0)
        {
            for (int index = 0; index < parents.Count; index++)
            {
                if (index < 2)
                {
                    animalScript.parents.Add(parents[index]);
                }
            }
        }

        GameObject.Find("SimulationManager").GetComponent<WorldLogic>().AddAnimal(childObject);
    }

    private float GenerateNormalValue(float mean, float standardDeviation, float min, float max)
    {
        float result;

        do
        {
            float x = 0f, y = 0f, s = 0f;
            while (s >= 1f || s <= 0f)
            {
                x = Random.Range(-1f, 1f);
                y = Random.Range(-1f, 1f);
                s = Mathf.Pow(x, 2f) + Mathf.Pow(y, 2f);
            }

            result = Mathf.Sqrt((-2f * Mathf.Log(s)) / s); //Mathf.Log = natural log
            result = x * result;

            result = mean + (result * standardDeviation * (mean*0.03f));
        }
        while (result < min || result > max);

        return result;
    }

    private void Update()
    {
        if (deductEnergy == true)
        {
            DeductEnergy();
            if (energy <= 0f)
            {
                Die();
            }
        }
        else
        {
            timer += Time.deltaTime;
            if (timer >= generationTime)
            {
                deductEnergy = true;
            }
        }
    }

    private void DeductEnergy()
    {
        int worldTemperature = GameObject.Find("SimulationManager").GetComponent<WorldLogic>().GetTemperature();
        float cost = (speed + speed * size) + visionRange + (1f/12f)*Mathf.Pow(Mathf.Abs(worldTemperature - idealTemperature), 2);
        //Debug.Log((cost).ToString());
        energy -= cost * Time.deltaTime * 0.3f;
        //energy -= Time.deltaTime * 15;
    }
    private void Die()
    {
        GameObject.Find("SimulationManager").GetComponent<WorldLogic>().RemoveAnimal(gameObject);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Food")
        {
            EatFood(collision.gameObject);
        }
    }
    void EatFood(GameObject foodObject)
    {
        Destroy(foodObject);
        if (energy <= (energyCap-30f))
        {
            energy += 30f;
        }
    }

    public void ToggleEnergyDeduction(bool doDeductEnergy)
    {
        deductEnergy = doDeductEnergy;
    }

    public int GetSpeed()
    {
        return speed;
    }
    public float GetSize()
    {
        return size;
    }
    public float GetVisionRange()
    {
        return visionRange;
    }
    public int GetTemperature()
    {
        return idealTemperature;
    }

    public void ToggleSelectedMode()
    {
        if (isSelected)
        {
            isSelected = false;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0.2352941f, 0.4352942f, 0.5490196f, 1f);
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0.2352941f, 0.4352942f, 0.5490196f, 0.09803922f);
        }
        else
        {
            isSelected = true;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0.9433962f, 0.909963f, 0.3871485f, 1f);
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0.9433962f, 0.909963f, 0.3871485f, 0.09803922f);
        }
    }

}