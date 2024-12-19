using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExperienceManager : MonoBehaviour
{
    [Header("Experience")]
    [SerializeField] AnimationCurve experienceCurve;

    int currentLevel, totalExperience;
    int previousLevelsExperience, nextLevelsExperience;

    [Header("Interface")]
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI experienceText;
    [SerializeField]    Image experienceFill;

    [Header("Buff Selection")]
    [SerializeField] BuffSelectionUI buffSelectionUI; // Reference to BuffSelectionUI

    // Singleton instance
    public static ExperienceManager instance;


    void Start()
    {
        UpdateLevel();
    }

    void Update()
    {

    }
    private void Awake()
    {
        // Singleton pattern uygulamasý
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Sahne deðiþtirilse bile bu objenin yok edilmemesini saðlar
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void AddExperience(int amount)
    {
        totalExperience += amount;
        CheckForLevelUp();
        UpdateInterface();
    }

    void CheckForLevelUp()
    {
        if (totalExperience >= nextLevelsExperience)
        {
            currentLevel++;
            UpdateLevel();
            TriggerBuffSelection();

            // Start level up sequence.... vfx or sound?
        }
    }
    void UpdateLevel()
    {
        previousLevelsExperience = (int)experienceCurve.Evaluate(currentLevel);
        nextLevelsExperience = (int)experienceCurve.Evaluate(currentLevel + 1);
        UpdateInterface();
    }

    void UpdateInterface()
    {
        int start = totalExperience - previousLevelsExperience;
        int end = nextLevelsExperience - previousLevelsExperience;

        levelText.text = currentLevel.ToString();
        experienceText.text = start + " exp / " + end + " exp";
        experienceFill.fillAmount = (float)start / (float)end;
    }
    void TriggerBuffSelection()
    {
        List<Buff> randomBuffs = BuffManager.Instance.GetRandomBuffs(3);
        buffSelectionUI.ShowBuffSelection(randomBuffs);
    }
}
