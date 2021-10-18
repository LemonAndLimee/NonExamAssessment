using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


        int worldTemperature = GameObject.Find("SimulationManager").GetComponent<WorldLogic>().GetTemperature();
        float cost = (speed + speed*size) + visionRange + Mathf.Abs(worldTemperature - idealTemperature);
        Debug.Log((cost * 0.5f).ToString());
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
        childObject.GetComponent<AnimalLogic>().SetValues(childSpeed, childVisionRange, childSize, childIdealTemp, energy);
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
        float cost = (speed + speed * size) + visionRange + Mathf.Abs(worldTemperature - idealTemperature);
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


}