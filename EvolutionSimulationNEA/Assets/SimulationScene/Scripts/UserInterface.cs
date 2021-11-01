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
        UpdateTextValue(numberOfAnimalsToSpawnSlider);
        worldScript.SetNumberOfAnimalsToSpawn((int)numberOfAnimalsToSpawnSlider.value);

        UpdateTextValue(environmentTempSlider);
        worldScript.SetWorldTemperature((int)environmentTempSlider.value);

        UpdateTextValue(numberOfGenerationsSlider);
        generationScript.SetNumberOfGenerations((int)numberOfGenerationsSlider.value);

        UpdateTextValue(generationDurationSlider);
        generationScript.SetGenerationDuration((int)generationDurationSlider.value);

        UpdateTextValue(foodPerGenerationSlider);
        generationScript.SetFoodPerGeneration((int)foodPerGenerationSlider.value);

        UpdateTextValue(reproductionChanceSlider);
        worldScript.SetReproductionPercentage(reproductionChanceSlider.value);

    }

    private void UpdateTextValue(Slider slider)
    {
        Text valueText = slider.transform.Find("Value").GetComponent<Text>();
        valueText.text = slider.value.ToString();
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
        EditValue(reproductionChanceSlider, 0.1f);
    }
    public void DecrementReproductionPercentage()
    {
        EditValue(reproductionChanceSlider, -0.1f);
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
