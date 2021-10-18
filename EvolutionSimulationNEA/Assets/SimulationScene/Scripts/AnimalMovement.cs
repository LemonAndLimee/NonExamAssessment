using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalMovement : MonoBehaviour
{
    const float xBoundary = 3.7f;
    const float yBoundary_positive = 4.55f;
    const float yBoundary_negative = -2.88f;

    private Vector2 direction;
    private int speed;
    private float size;
    private float timer = 0f;
    private float timeUntilNextDirection;

    private bool isTargeting;
    private GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        direction = SetRandomDirection(0, 0);
        timeUntilNextDirection = Random.Range(0f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if (isTargeting == false)
        {
            timer += Time.deltaTime;

            if (timer >= timeUntilNextDirection)
            {
                timeUntilNextDirection = Random.Range(0f, 5f);
                timer = 0f;
                direction = SetRandomDirection(0, 0);
            }
        }
        else
        {
            //if food is eaten by another animal
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
            direction = SetRandomDirection(-1, 0);
        }
        //if too far left
        if (transform.position.x <= -xBoundary)
        {
            timeUntilNextDirection = Random.Range(0f, 5f);
            timer = 0f;
            direction = SetRandomDirection(1, 0);
        }
        //if too high
        if (transform.position.y >= yBoundary_positive)
        {
            timeUntilNextDirection = Random.Range(0f, 5f);
            timer = 0f;
            direction = SetRandomDirection(0, -1);
        }
        //if too low
        if (transform.position.y <= yBoundary_negative)
        {
            timeUntilNextDirection = Random.Range(0f, 5f);
            timer = 0f;
            direction = SetRandomDirection(0, 1);
        }
    }
    
    void Move()
    {
        Vector2 difference = direction * Time.deltaTime * (-0.25f*speed*size + 1.25f*speed) * 0.04f;
        Vector3 difference_v3 = new Vector3(difference.x, difference.y, 0);
        transform.position = transform.position + difference_v3;
    }

    //paramters used to specify if the new direction should be in a specific quadrant (a value of 0 means no preference)
    Vector2 SetRandomDirection(int xQuadrant, int yQuadrant)
    {
        Vector2 direction= new Vector2();

        if (xQuadrant == -1)
        {
            direction.x = Random.Range(-1f, 0f);
        }
        else if (xQuadrant == 0)
        {
            direction.x = Random.Range(-1f, 1f);
        }
        else if (xQuadrant == 1)
        {
            direction.x = Random.Range(0f, 1f);
        }

        direction.y = Mathf.Sqrt(1 - (direction.x * direction.x));
        if (yQuadrant == -1)
        {
            direction.y = direction.y * -1;
        }
        else if (yQuadrant == 0)
        {
            direction.y = direction.y * Mathf.Pow(-1, Random.Range(1, 3));
        }

        return direction;
    }

    public void SetSpeed(int speedValue)
    {
        speed = speedValue;
    }
    public void SetSize(float sizeValue)
    {
        size = sizeValue;
    }

    public void TargetFood(GameObject foodObject)
    {
        target = foodObject;

        Vector3 directionVector = target.transform.position - transform.position;
        float currentLength = Mathf.Sqrt(Mathf.Pow(directionVector.x, 2) + Mathf.Pow(directionVector.y, 2));
        directionVector = directionVector * (1 / currentLength);

        isTargeting = true;
        direction = directionVector;
    }


    
}
