using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script manages animal movement
public class AnimalMovement : MonoBehaviour
{
    const float xBoundary = 3.7f; //positive and negative boundary - animal cannot go outside this on the x-axis
    const float yBoundary_positive = 4.55f;
    const float yBoundary_negative = -2.88f; //boundaries for travel on the y-axis

    private Vector2 direction; //current direction of the animal, in direction vector form
    private int speed; //animal speed
    private float size; //animal size
    private float timer = 0f; //timer used to generate random movements
    private float timeUntilNextDirection; //random amount of time between changes of direction

    private bool isTargeting; //true if targeting food
    private GameObject target; //food target object

    //called at the start
    void Start()
    {
        direction = SetRandomDirection(0, 0); //sets a random start direction
        timeUntilNextDirection = Random.Range(0f, 5f); //generates a random time until change in direction

    }

    //called every frame
    void Update()
    {
        Move(); // runs move subroutine

        if (isTargeting == false) //if not targeting a food object
        {
            timer += Time.deltaTime; //increases the timer in real time

            if (timer >= timeUntilNextDirection) //if timer passes the time until change in direction
            {
                timeUntilNextDirection = Random.Range(0f, 5f);
                timer = 0f;
                direction = SetRandomDirection(0, 0);
                //assigns new time until next direction, resets timer, generates a new random direction
            }
        }
        else
        {
            //if food is eaten by another animal, turn off targeting mode
            if (target == null)
            {
                isTargeting = false;
            }
        }

        //if too far right
        if (transform.position.x >= xBoundary)
        {
            timeUntilNextDirection = Random.Range(0f, 5f);
            timer = 0f;
            direction = SetRandomDirection(-1, 0); //sets random direction in the negative x direction
        }
        //if too far left
        if (transform.position.x <= -xBoundary)
        {
            timeUntilNextDirection = Random.Range(0f, 5f);
            timer = 0f;
            direction = SetRandomDirection(1, 0); //sets random direction in the positive x direction
        }
        //if too high
        if (transform.position.y >= yBoundary_positive)
        {
            timeUntilNextDirection = Random.Range(0f, 5f);
            timer = 0f;
            direction = SetRandomDirection(0, -1); //sets random direction in the negative y direction
        }
        //if too low
        if (transform.position.y <= yBoundary_negative)
        {
            timeUntilNextDirection = Random.Range(0f, 5f);
            timer = 0f;
            direction = SetRandomDirection(0, 1); //sets random direction in the positive y direction
        }
    }
    
    void Move() //called every frame
    {
        Vector2 difference = direction * Time.deltaTime * (-0.25f*speed*size + 1.25f*speed) * 0.04f; //calculates distance to be moved in vector form
        Vector3 difference_v3 = new Vector3(difference.x, difference.y, 0); //converts to vector3 form
        transform.position = transform.position + difference_v3; //adds the distance to the current position and moves the object
    }

    //parameters used to specify if the new direction should be in a specific quadrant (a value of 0 means no preference)
    Vector2 SetRandomDirection(int xQuadrant, int yQuadrant)
    {
        Vector2 direction = new Vector2();

        if (xQuadrant == -1) //if negative x quadrant, generate x value between -1 and 0
        {
            direction.x = Random.Range(-1f, 0f);
        }
        else if (xQuadrant == 0) //if no preference, generative x value between -1 and 1
        {
            direction.x = Random.Range(-1f, 1f);
        }
        else if (xQuadrant == 1) //if positive x quadrant, generate x value between 0 and 1
        {
            direction.x = Random.Range(0f, 1f);
        }

        direction.y = Mathf.Sqrt(1 - (direction.x * direction.x));
        // x^2 + y^2 = 1, as the position of the direction vector should lie on the unit circle
        // therefore the modulus of the y value can be calculated with 1 - x^2

        if (yQuadrant == -1) //if the y quadrant is negative, multiply by -1 to change it to a negative value
        {
            direction.y = direction.y * -1;
        }
        else if (yQuadrant == 0) //if no preference, multiply by (-1)^(random number between 1 and 2): this randomises whether it is positive or negative
        {
            direction.y = direction.y * Mathf.Pow(-1, Random.Range(1, 3));
        }

        return direction;
    }

    public void SetSpeed(int speedValue) //sets speed value of animal
    {
        speed = speedValue;
    }
    public void SetSize(float sizeValue) //sets size value of animal
    {
        size = sizeValue;
    }

    public void TargetFood(GameObject foodObject) // process of targeting a food object
    {
        target = foodObject; //sets target object

        Vector3 directionVector = target.transform.position - transform.position; //calculates direction the animal needs to go to reach the food
        float currentLength = Mathf.Sqrt(Mathf.Pow(directionVector.x, 2) + Mathf.Pow(directionVector.y, 2)); //calculates length of current direction vector
        directionVector = directionVector * (1 / currentLength); // gives direction vector length of 1 by dividing it by the current length

        isTargeting = true; //sets targeting mode on
        direction = directionVector; //sets animal direction to the targeting direction vector

    }


    
}
