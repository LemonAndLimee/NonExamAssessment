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
        worldScript.SetReproductionPercentage(reproductionChanceSlider.value*0.01f);

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
}
