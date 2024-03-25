using System;
using System.Collections.Generic;
using UnityEngine;

public class RandomVariablesManager : MonoBehaviour
{
    [Serializable]
    public struct VariablesCombination
    {
        public TaskCategory taskCategory;
        public float offsetValue;
        public bool liveRedirection;
        public float redirectedWalkingIntensity;
        public float redirectionSliderValue;
    }

    [Header("Seed for Random Generation")]
    [SerializeField] private int seed = 12345; // Default seed, can be set in Inspector
    public List<List<VariablesCombination>> studyOrderCombination = new List<List<VariablesCombination>>();

    public List<VariablesCombination> T0Combinations { get; private set; }
    public List<VariablesCombination> T1Combinations { get; private set; }
    public List<VariablesCombination> T2Combinations { get; private set; }
    public List<VariablesCombination> T3Combinations { get; private set; }
    public List<VariablesCombination> T45Combinations { get; private set; }

    public enum TaskCategory
{
    FirstTask,
    T0_Practice,
    T1_NoRedirection,
    T2_RDW,
    T3_HR,
    T4_CombinedNearThreshold,
    T5_CombinedBeyondThreshold
}

    private void Awake()
    {
        GenerateStudyListOrder();
    }

    public void SetSeedWithStudyId(int studyId)
    {
        seed += studyId; // Adds the study ID to the existing seed
    }

    public void GenerateHardcodedCombinations()
    {
        // Use the provided seed for repeatable results
        UnityEngine.Random.InitState(seed);

        T0Combinations = new List<VariablesCombination>();
        T1Combinations = new List<VariablesCombination>();
        T2Combinations = new List<VariablesCombination>();
        T3Combinations = new List<VariablesCombination>();
        T45Combinations = new List<VariablesCombination>();
        //T5Combinations = new List<VariablesCombination>();

        // T0 no rdw
        T0Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.T0_Practice,
            offsetValue = 1,
            liveRedirection = false,
            redirectedWalkingIntensity = 0,
            redirectionSliderValue = 0
        });
        // only RDW
        T0Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.T0_Practice,
            offsetValue = 1,
            liveRedirection = false,
            redirectedWalkingIntensity = 1,
            redirectionSliderValue = 1
        });

        // only HR
        T0Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.T0_Practice,
            offsetValue = 1,
            liveRedirection = false,
            redirectedWalkingIntensity = 0,
            redirectionSliderValue = 0.3f
        });
        //both 
        T0Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.T0_Practice,
            offsetValue = 1,
            liveRedirection = false,
            redirectedWalkingIntensity = 1,
            redirectionSliderValue = 0.3f
        });

        // T1 no redirection
        T1Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.T1_NoRedirection,
            offsetValue = 0,
            liveRedirection = false,
            redirectedWalkingIntensity = 0,
            redirectionSliderValue = 0
        });
        T1Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.T1_NoRedirection,
            offsetValue = 0.3f,
            liveRedirection = false,
            redirectedWalkingIntensity = 0,
            redirectionSliderValue = 0
        });
        T1Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.T1_NoRedirection,
            offsetValue = -0.3f,
            liveRedirection = false,
            redirectedWalkingIntensity = 0,
            redirectionSliderValue = 0
        });
               T1Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.T1_NoRedirection,
            offsetValue = 0.6f,
            liveRedirection = false,
            redirectedWalkingIntensity = 0,
            redirectionSliderValue = 0
        });
               T1Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.T1_NoRedirection,
            offsetValue = -0.6f,
            liveRedirection = false,
            redirectedWalkingIntensity = 0,
            redirectionSliderValue = 0
        });

        // T2 RDW
        T2Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.T2_RDW,
            offsetValue = 0.0f,
            liveRedirection = false,
            redirectedWalkingIntensity = 1,
            redirectionSliderValue = 0
        });
        T2Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.T2_RDW,
            offsetValue = 0.3f,
            liveRedirection = false,
            redirectedWalkingIntensity = 1,
            redirectionSliderValue = 0
        });
        T2Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.T2_RDW,
            offsetValue = -0.3f,
            liveRedirection = false,
            redirectedWalkingIntensity = 1,
            redirectionSliderValue = 0
        });
        T2Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.T2_RDW,
            offsetValue = 0.6f,
            liveRedirection = false,
            redirectedWalkingIntensity = 1,
            redirectionSliderValue = 0
        });
        T2Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.T2_RDW,
            offsetValue = -0.6f,
            liveRedirection = false,
            redirectedWalkingIntensity = 1,
            redirectionSliderValue = 0
        });
        // T3 HR

        T3Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.T3_HR,
            offsetValue = 0.0f,
            liveRedirection = false,
            redirectedWalkingIntensity = 0,
            redirectionSliderValue = 0.3f
        });
        T3Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.T3_HR,
            offsetValue = 0.15f,
            liveRedirection = false,
            redirectedWalkingIntensity = 0,
            redirectionSliderValue = 0.3f
        });
        T3Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.T3_HR,
            offsetValue = -0.15f,
            liveRedirection = false,
            redirectedWalkingIntensity = 0,
            redirectionSliderValue = 0.3f
        });
        T3Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.T3_HR,
            offsetValue = 0.05f,
            liveRedirection = false,
            redirectedWalkingIntensity = 0,
            redirectionSliderValue = 0.3f
        });
        T3Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.T3_HR,
            offsetValue = -0.05f,
            liveRedirection = false,
            redirectedWalkingIntensity = 0,
            redirectionSliderValue = 0.3f
        });
        // T4 Combined Near Threshold
        T45Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.T4_CombinedNearThreshold,
            offsetValue = 0.0f,
            liveRedirection = false,
            redirectedWalkingIntensity = 0.8f,
            redirectionSliderValue = 0.3f
        });
        T45Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.T4_CombinedNearThreshold,
            offsetValue = 0.3f,
            liveRedirection = false,
            redirectedWalkingIntensity = 0.8f,
            redirectionSliderValue = 0.3f
        });
        T45Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.T4_CombinedNearThreshold,
            offsetValue = -0.3f,
            liveRedirection = false,
            redirectedWalkingIntensity = 0.8f,
            redirectionSliderValue = 0.3f
        });
        T45Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.T4_CombinedNearThreshold,
            offsetValue = 0.6f,
            liveRedirection = false,
            redirectedWalkingIntensity = 0.8f,
            redirectionSliderValue = 0.3f
        });
        T45Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.T4_CombinedNearThreshold,
            offsetValue = -0.6f,
            liveRedirection = false,
            redirectedWalkingIntensity = 0.8f,
            redirectionSliderValue = 0.3f
        });
        // T5 Combined Beyond Threshold
        T45Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.T5_CombinedBeyondThreshold,
            offsetValue = 0.0f,
            liveRedirection = false,
            redirectedWalkingIntensity = 1,
            redirectionSliderValue = 0.3f
        });
        T45Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.T5_CombinedBeyondThreshold,
            offsetValue = 0.3f,
            liveRedirection = false,
            redirectedWalkingIntensity = 1,
            redirectionSliderValue = 0.3f
        });
        T45Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.T5_CombinedBeyondThreshold,
            offsetValue = -0.3f,
            liveRedirection = false,
            redirectedWalkingIntensity = 1,
            redirectionSliderValue = 0.3f
        });
        T45Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.T5_CombinedBeyondThreshold,
            offsetValue = 0.6f,
            liveRedirection = false,
            redirectedWalkingIntensity = 1,
            redirectionSliderValue = 0.3f
        });
        T45Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.T5_CombinedBeyondThreshold,
            offsetValue = -0.6f,
            liveRedirection = false,
            redirectedWalkingIntensity = 1,
            redirectionSliderValue = 0.3f
        });
        T45Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.T5_CombinedBeyondThreshold,
            offsetValue = 1f,
            liveRedirection = false,
            redirectedWalkingIntensity = 1,
            redirectionSliderValue = 0.3f
        });
        T45Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.T5_CombinedBeyondThreshold,
            offsetValue = -1f,
            liveRedirection = false,
            redirectedWalkingIntensity = 1,
            redirectionSliderValue = 0.3f
        });





    }
    private void Shuffle<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    public void GenerateAllCombinations()
    {
        GenerateHardcodedCombinations();
        Shuffle(T1Combinations);
        Shuffle(T2Combinations);
        Shuffle(T3Combinations);
        Shuffle(T45Combinations);
    }
    public void GenerateStudyListOrder()
{
    GenerateAllCombinations();

    // Create a list to store the order of the lists
    studyOrderCombination = new List<List<VariablesCombination>>();

    // Add T0 combinations to the order
    studyOrderCombination.Add(T0Combinations);

    // Shuffle the order of T1, T2, and T3 lists
    List<List<VariablesCombination>> t123Lists = new List<List<VariablesCombination>> { T1Combinations, T2Combinations, T3Combinations };
    Shuffle(t123Lists);

    // Add shuffled T1, T2, and T3 lists to the order
    studyOrderCombination.AddRange(t123Lists);

    // Add T45 combinations to the order
    studyOrderCombination.Add(T45Combinations);
}
}