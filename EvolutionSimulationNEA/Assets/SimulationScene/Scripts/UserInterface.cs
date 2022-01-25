using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    public Sprite playIcon;
    public Sprite pauseIcon;
    public Image playButton;
    private bool isPlaying = false;

    private float timeScale = 1f;
    private int timeScaleExponent = 0;

    private WorldLogic worldScript;
    private GenerationLogic generationScript;

    public GameObject settingsPanel;
    private bool displaySettings = false;

    public GameObject dataPanel;
    bool displayData = false;

    public LineGraphManager graphManagerScript;

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

    bool selectionModeOn = false;
    GameObject currentSelection;

    public GameObject rightDataPanel;
    public GameObject[] parentButtons;
    GameObject rightDataPanelSelection;
    int rightDataPanelSelectionIndex = 0;

    
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;
        worldScript = gameObject.GetComponent<WorldLogic>();
        generationScript = gameObject.GetComponent<GenerationLogic>();
    }

    // Update is called once per frame
    void Update()
    {
        if (displaySettings == true)
        {
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


        if (selectionModeOn)
        {
            AnimalLogic selectedAnimalScript = currentSelection.GetComponent<AnimalLogic>();
            if (selectedAnimalScript.parents.Count > 0)
            {
                for (int i = 0; i < selectedAnimalScript.parents.Count; i++)
                {
                    if (selectedAnimalScript.parents[i].GetObject() == null)
                    {
                        parentButtons[i+1].transform.Find("Cross").gameObject.SetActive(true);
                    }
                }
            }
        }

    }

    private void UpdateTextValue(Slider slider, float multiplier)
    {
        Text valueText = slider.transform.Find("Value").GetComponent<Text>();
        valueText.text = (slider.value*multiplier).ToString();
    }

    private void EditValue(Slider slider, float amount)
    {
        slider.value += amount;
    }

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

    public void PlayPauseButton()
    {
        if (isPlaying)
        {
            Pause();
        }
        else
        {
            Play();
        }
    }

    private void Play()
    {
        isPlaying = true;
        playButton.sprite = pauseIcon;
        Time.timeScale = timeScale;
    }
    //pause is public because upon reset, the WorldLogic needs to be able to put the game in a pause state regardless of current state
    public void Pause()
    {
        isPlaying = false;
        playButton.sprite = playIcon;
        Time.timeScale = 0f;
    }

    public void TimeScaleUp()
    {
        if (timeScaleExponent < 3 && isPlaying)
        {
            timeScaleExponent += 1;
            timeScale = Mathf.Pow(2, timeScaleExponent);
            Time.timeScale = timeScale;
        }
    }
    public void TimeScaleDown()
    {
        if (timeScaleExponent > -2 && isPlaying)
        {
            timeScaleExponent -= 1;
            timeScale = Mathf.Pow(2, timeScaleExponent);
            Time.timeScale = timeScale;
        }
    }

    public void ToggleSettings()
    {
        if (displayData == true)
        {
            ToggleData();
        }

        if (displaySettings == false)
        {
            displaySettings = true;
            settingsPanel.SetActive(true);
        }
        else
        {
            displaySettings = false;
            settingsPanel.SetActive(false);
        }
    }

    public void ToggleData()
    {
        graphManagerScript.ToggleVisibility();

        if (displaySettings == true)
        {
            ToggleSettings();
        }

        if (displayData == false)
        {
            displayData = true;
            dataPanel.SetActive(true);
        }
        else
        {
            displayData = false;
            dataPanel.SetActive(false);
        }
    }

    public void SetSelectionMode(bool mode, GameObject selection)
    {
        selectionModeOn = mode;
        currentSelection = selection;

        if (selectionModeOn)
        {
            ResetRightDataPanel();
            rightDataPanel.SetActive(true);
        }
        else
        {
            
            rightDataPanel.SetActive(false);
        }
    }
    public void ResetRightDataPanel()
    {
        for (int index = 0; index < parentButtons.Length; index++)
        {
            parentButtons[index].transform.Find("Cross").gameObject.SetActive(false);
            parentButtons[index].SetActive(false);
        }
        SelectRightDataPanelButton(0);

        AnimalLogic animalScript = currentSelection.GetComponent<AnimalLogic>();
        parentButtons[0].SetActive(true);
        if (animalScript.parents.Count > 0)
        {
            for (int i = 0; i < animalScript.parents.Count; i++)
            {
                parentButtons[i + 1].SetActive(true);
            }
        }

    }

    void SelectRightDataPanelButton(int index)
    {
        rightDataPanelSelectionIndex = index;
        if (index == 0)
        {
            rightDataPanelSelection = currentSelection;
            worldScript.SetDisplayStats(currentSelection.GetComponent<AnimalLogic>().GetAnimalStats());
        }
        else
        {
            rightDataPanelSelection = currentSelection.GetComponent<AnimalLogic>().parents[index-1].GetObject();
            worldScript.SetDisplayStats(currentSelection.GetComponent<AnimalLogic>().parents[index - 1]);
        }

        for (int buttonIndex = 0; buttonIndex < parentButtons.Length; buttonIndex++)
        {
            if (buttonIndex == index)
            {
                parentButtons[buttonIndex].transform.Find("SelectionGlow").gameObject.SetActive(true);
            }
            else
            {
                parentButtons[buttonIndex].transform.Find("SelectionGlow").gameObject.SetActive(false);
            }
        }

        
    }

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
}
