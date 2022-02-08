using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//this script manages the user interface
public class UserInterface : MonoBehaviour
{
    public Sprite playIcon; //play image
    public Sprite pauseIcon; //pause image
    public Image playButton; //image displayed by the play/pause button
    private bool isPlaying = false; //true if the simulation is playing

    private float timeScale = 1f; //scale of time
    private int timeScaleExponent = 0; //used to control the time scale increases and decreases

    private WorldLogic worldScript; //WorldLogic script
    private GenerationLogic generationScript; //GenerationLogic script

    public GameObject settingsPanel; //settings panel game object
    private bool displaySettings = false; //true if settings panel is being displayed

    public GameObject dataPanel; //data panel game object
    bool displayData = false; //true if data panel is being displayed

    public LineGraphManager graphManagerScript; //graph manager script

    //sliders in the settings panel used to change simulation values
    public Slider environmentTempSlider;
    public Slider numberOfAnimalsToSpawnSlider;
    public Slider numberOfGenerationsSlider;
    public Slider generationDurationSlider;
    public Slider foodPerGenerationSlider;
    public Slider reproductionChanceSlider;
    public Slider standardDeviationSlider;

    public Slider speedSlider;
    public Slider visionSlider;
    public Slider sizeSlider;
    public Slider tempSlider;

    bool selectionModeOn = false; //true if an animal is being selected
    GameObject currentSelection; //refers to animal currently being selected

    public GameObject rightDataPanel; //right data panel game object
    public GameObject[] parentButtons; //array of buttons in the right data panel, used for representing the parents of selected animal

    public GameObject dataPanelInfoButton; //info button attached to data panel

    //info panels for various UI elements
    public GameObject leftDataPanelInfoPanel;
    public GameObject rightDataPanelInfoPanel;
    public GameObject settingsInfoPanel;
    public GameObject dataPanelInfoPanel;
    
    //called at the start
    void Start()
    {
        Time.timeScale = 0f; //sets time scale to zero, pauses the simulation
        worldScript = gameObject.GetComponent<WorldLogic>(); //assigns WorldLogic script
        generationScript = gameObject.GetComponent<GenerationLogic>(); //assigns GenerationLogic script
    }

    //called every frame
    void Update()
    {
        if (displaySettings == true) //if settings panel is being displayed
        {
            //updates the text captions of each slider to reflect the slider value
            //updates the variable value in WorldLogic to reflect the slider value

            UpdateTextValue(numberOfAnimalsToSpawnSlider, 1f);
            worldScript.SetNumberOfAnimalsToSpawn((int)numberOfAnimalsToSpawnSlider.value);

            UpdateTextValue(environmentTempSlider, 1f);
            worldScript.SetWorldTemperature((int)environmentTempSlider.value);

            UpdateTextValue(numberOfGenerationsSlider, 1f);
            generationScript.SetNumberOfGenerations((int)numberOfGenerationsSlider.value);

            UpdateTextValue(generationDurationSlider, 1f);
            generationScript.SetGenerationDuration((int)generationDurationSlider.value);

            UpdateTextValue(foodPerGenerationSlider, 1f);
            generationScript.SetFoodPerGeneration((int)foodPerGenerationSlider.value);

            UpdateTextValue(reproductionChanceSlider, 0.01f);
            worldScript.SetReproductionPercentage(reproductionChanceSlider.value * 0.01f);

            UpdateTextValue(standardDeviationSlider, 1f);
            worldScript.SetStandardDeviation(standardDeviationSlider.value);

            UpdateTextValue(speedSlider, 1f);
            worldScript.SetStartingSpeed((int)speedSlider.value);

            UpdateTextValue(visionSlider, 0.1f);
            worldScript.SetStartingVision(visionSlider.value * 0.1f);

            UpdateTextValue(sizeSlider, 0.1f);
            worldScript.SetStartingSize(sizeSlider.value * 0.1f);

            UpdateTextValue(tempSlider, 1f);
            worldScript.SetStartingIdealTemp((int)tempSlider.value);
        }


        if (selectionModeOn) //if an animal is being selected
        {
            //sets selectedAnimalScript to the AnimalLogic script of the currently selected animal
            AnimalLogic selectedAnimalScript = currentSelection.GetComponent<AnimalLogic>();
            if (selectedAnimalScript.parents.Count > 0) //if the selected animal has any parents
            {
                for (int i = 0; i < selectedAnimalScript.parents.Count; i++) //loops through the parents
                {
                    //if the parent game object is equal to null, i.e. if the parent is "dead"
                    if (selectedAnimalScript.parents[i].GetObject() == null)
                    {
                        //enables the red cross image on the parent button
                        parentButtons[i+1].transform.Find("Cross").gameObject.SetActive(true);
                    }
                }
            }
        }

    }

    //updates text caption to reflect slider value
    private void UpdateTextValue(Slider slider, float multiplier)
    {
        Text valueText = slider.transform.Find("Value").GetComponent<Text>(); //sets valueText to the caption text of the slider
        valueText.text = (slider.value*multiplier).ToString(); //sets valueText contents equal to the slider value * multiplier
    }

    //edits the slider value by a set amount
    private void EditValue(Slider slider, float amount)
    {
        slider.value += amount;
    }



    //subroutines used by the plus and minus buttons on either side of each slider
    //the incremement subroutines add 1 to the slider value and the decrememnt subroutines subtract 1 from the slider value

    public void IncrementNumberOfAnimalsToSpawn()
    {
        EditValue(numberOfAnimalsToSpawnSlider, 1f);
    }
    public void DecrementNumberOfAnimalsToSpawn()
    {
        EditValue(numberOfAnimalsToSpawnSlider, -1f);
    }

    public void IncrementWorldTemp()
    {
        EditValue(environmentTempSlider, 1f);
    }
    public void DecrementWorldTemp()
    {
        EditValue(environmentTempSlider, -1f);
    }

    public void IncrementNumberOfGenerations()
    {
        EditValue(numberOfGenerationsSlider, 1f);
    }
    public void DecrementNumberOfGenerations()
    {
        EditValue(numberOfGenerationsSlider, -1f);
    }

    public void IncrementGenerationDuration()
    {
        EditValue(generationDurationSlider, 1f);
    }
    public void DecrementGenerationDuration()
    {
        EditValue(generationDurationSlider, -1f);
    }

    public void IncrementFoodPerGeneration()
    {
        EditValue(foodPerGenerationSlider, 1f);
    }
    public void DecrementFoodGeneration()
    {
        EditValue(foodPerGenerationSlider, -1f);
    }

    public void IncrementReproductionPercentage()
    {
        EditValue(reproductionChanceSlider, 1f);
    }
    public void DecrementReproductionPercentage()
    {
        EditValue(reproductionChanceSlider, -1f);
    }

    public void IncrementStandardDeviation()
    {
        EditValue(standardDeviationSlider, 1f);
    }
    public void DecrementStandardDeviation()
    {
        EditValue(standardDeviationSlider, -1f);
    }


    public void IncrementStartingSpeed()
    {
        EditValue(speedSlider, 1f);
    }
    public void DecrementStartingSpeed()
    {
        EditValue(speedSlider, -1f);
    }

    public void IncrementStartingVision()
    {
        EditValue(visionSlider, 1f);
    }
    public void DecrementStartingVision()
    {
        EditValue(visionSlider, -1f);
    }

    public void IncrementStartingSize()
    {
        EditValue(sizeSlider, 1f);
    }
    public void DecrementStartingSize()
    {
        EditValue(sizeSlider, -1f);
    }

    public void IncrementStartingIdeaTemp()
    {
        EditValue(tempSlider, 1f);
    }
    public void DecrementStartingIdealTemp()
    {
        EditValue(tempSlider, -1f);
    }



    //when play/pause button is pressed
    public void PlayPauseButton()
    {
        if (isPlaying) //if the simulation is playing, pause the simulation
        {
            Pause();
        }
        else //if the simulation is paused, play the simulation
        {
            Play();
        }
    }

    //plays simulation
    private void Play()
    {
        isPlaying = true;
        playButton.sprite = pauseIcon; //changes play/pause button icon to the pause icon
        Time.timeScale = timeScale; //restores time scale to its value before the simulation was paused
    }
    //pauses simulation
    //Pause() is public because upon reset, the WorldLogic needs to be able to put the game in a pause state regardless of current state
    public void Pause()
    {
        isPlaying = false;
        playButton.sprite = playIcon; //changes play/pause button icon to the play icon
        Time.timeScale = 0f; //sets time scale to 0: freezes time
    }

    //increases time scale
    public void TimeScaleUp()
    {
        //if simulation is playing AND timeScaleExponent is less than 3 (this prevents the time scale being endlessly increased
        if (timeScaleExponent < 3 && isPlaying)
        {
            timeScaleExponent += 1; //incremements timeScaleExponent by 1
            //sets timeScale to 2^timeScaleExponent (each time this subroutine is called it doubles the time scale)
            timeScale = Mathf.Pow(2, timeScaleExponent);
            Time.timeScale = timeScale; //sets simulation time scale to the variable timeScale
        }
    }
    //decreases time scale
    public void TimeScaleDown()
    {
        //if simulation is playing and timeScaleExponent is greater than -2
        if (timeScaleExponent > -2 && isPlaying)
        {
            timeScaleExponent -= 1; //decrements timeScaleExponent by 1
            //sets timeScale to 2^timeScaleExponent (each time this subroutine is called it halves the time scale)
            timeScale = Mathf.Pow(2, timeScaleExponent);
            Time.timeScale = timeScale; //sets simulation time scale to the variable timeScale
        }
    }

    //toggles settings panel
    public void ToggleSettings()
    {
        if (displayData == true) //if currently displaying the data panel
        {
            ToggleData(); //disables data panel
        }

        if (displaySettings == false) //if settings panel is disabled, enable settings panel
        {
            displaySettings = true;
            settingsPanel.SetActive(true);
        }
        else //if settings panel is enabled, disable settings panel
        {
            displaySettings = false;
            settingsPanel.SetActive(false);
        }
    }

    //toggles data panel
    public void ToggleData()
    {
        graphManagerScript.ToggleVisibility(); //toggles visibility of the graph

        if (displaySettings == true) //if settings panel is currently displaying
        {
            ToggleSettings(); //disables settings panel
        }

        if (displayData == false) //if data panel is disabled, enable data panel
        {
            displayData = true;
            dataPanel.SetActive(true);
            dataPanelInfoButton.SetActive(true);
        }
        else //if data panel is enabled, disable data panel
        {
            displayData = false;
            dataPanel.SetActive(false);
            dataPanelInfoButton.SetActive(false);
        }
    }

    //used to turn selection mode on and off
    public void SetSelectionMode(bool mode, GameObject selection)
    {
        selectionModeOn = mode; //sets SelectionModeOn to mode
        currentSelection = selection; //sets currentSelection to selection object passed into the subroutine

        if (selectionModeOn) //if selectionMode is true
        {
            ResetRightDataPanel(); //reset the right data panel
            rightDataPanel.SetActive(true); //enables right data panel game object
        }
        else //if selectionMode is false
        {
            rightDataPanel.SetActive(false); //disables right data panel game object
        }
    }
    public void ResetRightDataPanel() //resets the right data panel contents
    {
        for (int index = 0; index < parentButtons.Length; index++) //loops through every parent button
        {
            parentButtons[index].transform.Find("Cross").gameObject.SetActive(false); //deactivate cross image for that button
            parentButtons[index].SetActive(false); //deactivate button
        }
        SelectRightDataPanelButton(0); //sets the bottom button to be the one currently selected

        //assigns animalScript as the AnimalLogic script of the currently selected animal
        AnimalLogic animalScript = currentSelection.GetComponent<AnimalLogic>();
        parentButtons[0].SetActive(true); //enables bottom button
        if (animalScript.parents.Count > 0) //if the animal has parents
        {
            for (int i = 0; i < animalScript.parents.Count; i++) //loops through each parent
            {
                //activates the button corresponding to that parent
                //takes into account that the button at index 0 refers to the animal itself, so parent 0 corresponds to button 1, etc.
                parentButtons[i + 1].SetActive(true);
            }
        }

    }

    void SelectRightDataPanelButton(int index) //used to select one of the parent buttons
    {
        if (index == 0) //if bottom button pressed
        {
            //displays currently selected animal's stats in the left data panel
            worldScript.SetDisplayStats(currentSelection.GetComponent<AnimalLogic>().GetAnimalStats());
        }
        else //if any other button pressed
        {
            //sets left data panel to display the corresponding parent animal's stats
            worldScript.SetDisplayStats(currentSelection.GetComponent<AnimalLogic>().parents[index - 1]);
        }

        for (int buttonIndex = 0; buttonIndex < parentButtons.Length; buttonIndex++) //loops through each parent button
        {
            if (buttonIndex == index) //if the button is the button that was pressed
            {
                //enables the selection glow image of that button
                parentButtons[buttonIndex].transform.Find("SelectionGlow").gameObject.SetActive(true);
            }
            else //if the button is not the one pressed
            {
                //disables the selection glow image of that button
                parentButtons[buttonIndex].transform.Find("SelectionGlow").gameObject.SetActive(false);
            }
        }

        
    }

    //subroutines used for pressing each button
    public void SelectButton0()
    {
        SelectRightDataPanelButton(0);
    }
    public void SelectButton1()
    {
        SelectRightDataPanelButton(1);
    }
    public void SelectButton2()
    {
        SelectRightDataPanelButton(2);
    }
    public void SelectButton3()
    {
        SelectRightDataPanelButton(3);
    }

    void ToggleInfoPanel(GameObject panel) //toggles info panel
    {
        if (panel.activeSelf == true) //if panel is enabled
        {
            panel.SetActive(false); //disables panel
        } 
        else //if panel is disabled
        {
            panel.SetActive(true); //enables panel
        }
    }

    public void LeftDataPanelInfoButton() //used to toggle left data panel info panel
    {
        ToggleInfoPanel(leftDataPanelInfoPanel);
    }
    public void RightDataPanelInfoButton() //used to toggle right data panel info panel
    {
        ToggleInfoPanel(rightDataPanelInfoPanel);
    }
    public void SettingsInfoButton() //used to toggle settings panel info panel
    {
        ToggleInfoPanel(settingsInfoPanel);
    }
    public void DataPanelInfoButton() //used to toggle data panel info panel
    {
        ToggleInfoPanel(dataPanelInfoPanel);
    }
}
