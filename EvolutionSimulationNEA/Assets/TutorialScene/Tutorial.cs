using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//this script manages the tutorial
public class Tutorial : MonoBehaviour
{
    public List<GameObject> tutorialSteps = new List<GameObject>(); //list of the tutorial steps
    int currentStepIndex = 0; //current tutorial step number

    public void GoToNextStep() //goes to next tutorial step
    {
        //disables current tutorial step gameobject
        tutorialSteps[currentStepIndex].SetActive(false);
        currentStepIndex++; //increments step number by 1
        tutorialSteps[currentStepIndex].SetActive(true); //enables next tutorial step gameobject
    }
    public void GoToPreviousStep() //returns to previous tutorial step
    {
        tutorialSteps[currentStepIndex].SetActive(false); //disables current tutorial step gameobject
        currentStepIndex--; //decrements step number by 1
        tutorialSteps[currentStepIndex].SetActive(true); //enables previous tutorial step gameobject
    }

    public void GoToSimulation() //goes to the simulation scene
    {
        SceneManager.LoadScene(1); //loads simulation scene
    }
}
