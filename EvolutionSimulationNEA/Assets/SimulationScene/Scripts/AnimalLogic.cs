using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Animal //struct used to store information about the animal
{
    GameObject animalObject; //used to store the gameObject
    int speed; //animal speed
    float visionRange; //animal vision range
    float size; //animal size
    int idealTemp; //animal ideal temperature

    //creates an Animal data type, and assigns the values
    public Animal(GameObject obj, int animalSpeed, float vision, float animalSize, int temp)
    {
        animalObject = obj;
        speed = animalSpeed;
        visionRange = vision;
        size = animalSize;
        idealTemp = temp;
    }

    public GameObject GetObject() //returns animal gameObject
    {
        return animalObject;
    }
    public int GetSpeed() //returns animal speed
    {
        return speed;
    }
    public float GetVisionRange() //returns animal vision range
    {
        return visionRange;
    }
    public float GetSize() //returns animal size
    {
        return size;
    }
    public int GetIdealTemp() //returns animal ideal temperature
    {
        return idealTemp;
    }
}

//this script manages the animal values, parents, energy cost and reproduction mechanics
public class AnimalLogic : MonoBehaviour
{
    const float defaultSize = 0.172f; //starting sprite size
    const float energyCap = 200f; //maximum energy value

    public int speed; //animal speed
    public float visionRange; //animal vision range
    public float size; //animal size
    public int idealTemperature; //animal ideal temperature

    public float energy = 100f; // energy value - starts at 100

    public List<Animal> parents = new List<Animal>(); // list of data type Animal, used to store the animal's parents
    Animal localStats; //data type Animal, stores values of current game object

    public bool isFirstGeneration; // true if the animal is of the first generation spawned

    public bool isSelected; // true if the player is selecting the animal

    //assigns default colours for the animal and its vision range
    Color defaultAnimalColour = new Color(0.2352941f, 0.4352942f, 0.5490196f, 1f);
    Color defaultVisionRangeColour = new Color(0.2352941f, 0.4352942f, 0.5490196f, 0.09803922f);

    //assigns colours for when the animal is being selected
    Color selectedAnimalColour = new Color(0.9433962f, 0.909963f, 0.3871485f, 1f);
    Color selectedVisionRangeColour = new Color(0.9433962f, 0.909963f, 0.3871485f, 0.09803922f);

    bool isDeductingEnergy = true; //determines if animal is deducting energy
    float timer = 0f; //timer is used to count until the first generation duration has completed
    int generationTime; //used to store duration value of a generation

    //called at the start
    private void Start()
    {
        //sets generation time to the generation duration stored in GenerationLogic script
        generationTime = GameObject.Find("SimulationManager").GetComponent<GenerationLogic>().GetGenerationDuration();

        //sets the object to default animal colour - this prevents offspring of the selected animal from being the selected colour
        gameObject.GetComponent<SpriteRenderer>().color = defaultAnimalColour;
        //sets the vision range object to default colour
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = defaultVisionRangeColour;

    }
    //assigns values to each of the character traits
    public void SetValues(int speedValue, float visionRangeValue, float sizeValue, int idealTemperatureValue, float energyValue)
    {
        speed = speedValue;
        gameObject.GetComponent<AnimalMovement>().SetSpeed(speed); //sends the speed value to the movement script

        visionRange = visionRangeValue;
        //sets the vision range object size to the value of the vision range
        transform.GetChild(0).localScale = new Vector3(visionRange, visionRange, 1);

        size = sizeValue;
        //sets animal size to the default size * size value
        transform.localScale = new Vector3(size * defaultSize, size * defaultSize, 1f);
        //sends size value to the movement script
        gameObject.GetComponent<AnimalMovement>().SetSize(size);

        idealTemperature = idealTemperatureValue;

        energy = energyValue;

        //assigns the values to the variable localStats, data type Animal
        localStats = new Animal(gameObject, speed, visionRange, size, idealTemperature);
    }

    //animal reproduces, creating a clone of itself with slightly different values
    public void Reproduce(float standardDeviation)
    {
        //creates slightly different values for each trait, using normal distribution function
        int childSpeed = Mathf.RoundToInt(GenerateNormalValue(speed, standardDeviation, 1f, 100f));
        float childSize = GenerateNormalValue(size, standardDeviation, 0.25f, 5f);
        float childVisionRange = GenerateNormalValue(visionRange, standardDeviation, 1f, 10f);
        int childIdealTemp = Mathf.RoundToInt(GenerateNormalValue(idealTemperature, standardDeviation, -20f, 50f));


        energy = energy / 2f; // halves the energy and gives the same value to the child - this simulates giving the child half the energy

        //creates clone of itself
        GameObject childObject = Instantiate(gameObject);
        //sets animalScript to the AnimalLogic script of the clone
        AnimalLogic animalScript = childObject.GetComponent<AnimalLogic>();
        //sets clone values to the created values in this subroutine
        animalScript.SetValues(childSpeed, childVisionRange, childSize, childIdealTemp, energy);
        //since the clone has a parent, it is not of the first generation
        animalScript.isFirstGeneration = false;

        //add current animal stats to the clone's parents list
        animalScript.parents.Add(localStats);

        //if current animal has parents
        if (parents.Count > 0)
        {
            //loop through parents list
            for (int index = 0; index < parents.Count; index++)
            {
                //addsnext two parents from current animal to the clone's parents list
                if (index < 2)
                {
                    animalScript.parents.Add(parents[index]);
                }
                //the parents list will start with the immediate parent and end with the oldest relative
            }
        }

        //adds clone object to the list recording all alive animals
        GameObject.Find("SimulationManager").GetComponent<WorldLogic>().AddAnimal(childObject);
    }

    //used to generate a value similar to an original value, using normal distribution
    //see design for for information on the algorithm
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

            result = Mathf.Sqrt((-2f * Mathf.Log(s)) / s); //Mathf.Log finds the natural log of the value
            result = x * result;

            result = mean + (result * standardDeviation * (mean * 0.03f));
        }
        while (result < min || result > max);

        return result;
    }

    //called every frame
    private void Update()
    {
        if (isDeductingEnergy) //if current deducting energy
        {
            //deduct energy cost
            DeductEnergy();
            if (energy <= 0f) //if energy reaches zero or lower, the animal dies
            {
                Die();
            }
        }
        else //if not deducting energy
        {
            timer += Time.deltaTime; //increase the timer value in real time
            if (timer >= generationTime) //if the timer surpasses the generation lenght
            {
                isDeductingEnergy = true; //sets deduction mode to true
            }
        }
    }

    private void DeductEnergy()
    {
        //fetches world temperature
        int worldTemperature = GameObject.Find("SimulationManager").GetComponent<WorldLogic>().GetTemperature();
        //calculates energy cost
        float cost = (speed + speed * size) + visionRange + (1f / 12f) * Mathf.Pow(Mathf.Abs(worldTemperature - idealTemperature), 2);
        //deducts the cost from the animal energy value
        energy -= cost * Time.deltaTime * 0.3f;

    }
    private void Die()
    {
        //removes itself from the list storing all animals
        GameObject.Find("SimulationManager").GetComponent<WorldLogic>().RemoveAnimal(gameObject);
        //destroys itself
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision) //when the animal collides with something
    {
        if (collision.gameObject.tag == "Food") //if the animal has collided with a food object
        {
            EatFood(collision.gameObject); //eat the food object
        }
    }
    void EatFood(GameObject foodObject)
    {
        //destroys the food object
        Destroy(foodObject);
        //adds 30 to the energy value
        energy += 30f;

        //if energy has exceeded the maximum energy amount, reduce it to the value of the energy cap
        if (energy > energyCap)
        {
            energy = energyCap;
        }
    }

    //returns the animal speed
    public int GetSpeed()
    {
        return speed;
    }
    //returns animal size
    public float GetSize()
    {
        return size;
    }
    //returns animal vision range
    public float GetVisionRange()
    {
        return visionRange;
    }
    //returns animal ideal temperature
    public int GetTemperature()
    {
        return idealTemperature;
    }

    //if selected mode is on, change it to off, and vice versa
    public void ToggleSelectedMode()
    {
        if (isSelected) //if selected mode is true
        {
            isSelected = false; //sets selected mode to false
            //sets animal colours to default values
            gameObject.GetComponent<SpriteRenderer>().color = defaultAnimalColour;
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = defaultVisionRangeColour;
        }
        else //if selected mode is false
        {
            isSelected = true; //sets selected mode to true
            //sets animal colours to the selected version
            gameObject.GetComponent<SpriteRenderer>().color = selectedAnimalColour;
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = selectedVisionRangeColour;
        }
    }

    //returns Animal storing current animal values
    public Animal GetAnimalStats()
    {
        return localStats;
    }

    public void SetEnergyDeduction(bool value) //sets energy deduction mode to value
    {
        isDeductingEnergy = value;
    }
}