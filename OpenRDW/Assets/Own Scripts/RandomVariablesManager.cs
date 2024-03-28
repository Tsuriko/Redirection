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
    public List<VariablesCombination> T6Combinations { get; private set; }
    public List<VariablesCombination> A1Combinations { get; private set; }
    public List<VariablesCombination> A2Combinations { get; private set; }
    public List<VariablesCombination> A3Combinations { get; private set; }
    public List<VariablesCombination> A4Combinations { get; private set; }


    public enum TaskCategory
    {
        FirstTask,
        T0_Practice,
        T1_NoRedirection,
        T2_RDW,
        T3_HR,
        T4_CombinedNearThreshold,
        T5_CombinedBeyondThreshold,
        T6_Random,

        A1,
        A2,
        A3,
        A4
    }

    private void Awake()
    {
        GenerateStudyListOrder();
    }

    public void SetSeedWithStudyId(int studyId)
    {
        seed = studyId + 1234;
    }

    public void GenerateHardcodedCombinations()
    {
        // Use the provided seed for repeatable results
        UnityEngine.Random.InitState(seed);

        T0Combinations = new List<VariablesCombination>();
        //T1Combinations = new List<VariablesCombination>();
        T2Combinations = new List<VariablesCombination>();
        T3Combinations = new List<VariablesCombination>();
        T45Combinations = new List<VariablesCombination>();
        //T5Combinations = new List<VariablesCombination>();
        T6Combinations = new List<VariablesCombination>();
        A1Combinations = new List<VariablesCombination>();
        A2Combinations = new List<VariablesCombination>();
        A3Combinations = new List<VariablesCombination>();
        A4Combinations = new List<VariablesCombination>();

        // T0 no rdw
        /*
        T0Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.T0_Practice,
            offsetValue = 0.5f,
            liveRedirection = false,
            redirectedWalkingIntensity = 0,
            redirectionSliderValue = 0
        });
        */
        // only RDW
        T0Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.T0_Practice,
            offsetValue = 0.5f,
            liveRedirection = false,
            redirectedWalkingIntensity = 1.4f,
            redirectionSliderValue = 1
        });

        // only HR
        T0Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.T0_Practice,
            offsetValue = 0.1f,
            liveRedirection = false,
            redirectedWalkingIntensity = 0,
            redirectionSliderValue = 0.3f
        });
        //both 
        T0Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.T0_Practice,
            offsetValue = 0.5f,
            liveRedirection = false,
            redirectedWalkingIntensity = 1,
            redirectionSliderValue = 0.3f
        });
        /*
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
        */
        // T2 RDW
        T2Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.T2_RDW,
            offsetValue = 0.0f,
            liveRedirection = false,
            redirectedWalkingIntensity = 1.4f,
            redirectionSliderValue = 0
        });
        T2Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.T2_RDW,
            offsetValue = 0.3f,
            liveRedirection = false,
            redirectedWalkingIntensity = 1.2f,
            redirectionSliderValue = 0
        });
        T2Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.T2_RDW,
            offsetValue = -0.3f,
            liveRedirection = false,
            redirectedWalkingIntensity = 1.1f,
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
            redirectedWalkingIntensity = 1,
            redirectionSliderValue = 0.3f
        });
        T45Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.T4_CombinedNearThreshold,
            offsetValue = -0.3f,
            liveRedirection = false,
            redirectedWalkingIntensity = 1,
            redirectionSliderValue = 0.3f
        });
        T45Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.T4_CombinedNearThreshold,
            offsetValue = 0.6f,
            liveRedirection = false,
            redirectedWalkingIntensity = 1,
            redirectionSliderValue = 0.3f
        });
        T45Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.T4_CombinedNearThreshold,
            offsetValue = -0.6f,
            liveRedirection = false,
            redirectedWalkingIntensity = 1,
            redirectionSliderValue = 0.3f
        });
        // T5 Combined Beyond Threshold
        T45Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.T5_CombinedBeyondThreshold,
            offsetValue = 0.0f,
            liveRedirection = false,
            redirectedWalkingIntensity = 1.1f,
            redirectionSliderValue = 0.3f
        });
        T45Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.T5_CombinedBeyondThreshold,
            offsetValue = 0.3f,
            liveRedirection = false,
            redirectedWalkingIntensity = 1.1f,
            redirectionSliderValue = 0.3f
        });
        T45Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.T5_CombinedBeyondThreshold,
            offsetValue = -0.3f,
            liveRedirection = false,
            redirectedWalkingIntensity = 1.1f,
            redirectionSliderValue = 0.3f
        });
        T45Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.T5_CombinedBeyondThreshold,
            offsetValue = 0.6f,
            liveRedirection = false,
            redirectedWalkingIntensity = 1.1f,
            redirectionSliderValue = 0.3f
        });
        T45Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.T5_CombinedBeyondThreshold,
            offsetValue = -0.6f,
            liveRedirection = false,
            redirectedWalkingIntensity = 1.1f,
            redirectionSliderValue = 0.3f
        });
        A4Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.A4,
            offsetValue = 0.3f,
            liveRedirection = false,
            redirectedWalkingIntensity = 1.4f,
            redirectionSliderValue = 0.3f
        });
        A3Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.A3,
            offsetValue = 0.3f,
            liveRedirection = false,
            redirectedWalkingIntensity = 1.0f,
            redirectionSliderValue = 0.3f
        });
        A2Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.A2,
            offsetValue = 0.3f,
            liveRedirection = false,
            redirectedWalkingIntensity = 0.8f,
            redirectionSliderValue = 0.3f
        });
        A1Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.A1,
            offsetValue = 0.3f,
            liveRedirection = false,
            redirectedWalkingIntensity = 0.6f,
            redirectionSliderValue = 0.3f
        });
        A4Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.A4,
            offsetValue = 0.5f,
            liveRedirection = false,
            redirectedWalkingIntensity = 1.4f,
            redirectionSliderValue = 0.3f
        });
        A3Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.A3,
            offsetValue = 0.5f,
            liveRedirection = false,
            redirectedWalkingIntensity = 1.0f,
            redirectionSliderValue = 0.3f
        });
        A2Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.A2,
            offsetValue = 0.5f,
            liveRedirection = false,
            redirectedWalkingIntensity = 0.8f,
            redirectionSliderValue = 0.3f
        });
        A1Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.A1,
            offsetValue = 0.5f,
            liveRedirection = false,
            redirectedWalkingIntensity = 0.6f,
            redirectionSliderValue = 0.3f
        });
        A4Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.A4,
            offsetValue = 0.7f,
            liveRedirection = false,
            redirectedWalkingIntensity = 1.4f,
            redirectionSliderValue = 0.3f
        });
        A3Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.A3,
            offsetValue = 0.7f,
            liveRedirection = false,
            redirectedWalkingIntensity = 1.0f,
            redirectionSliderValue = 0.3f
        });
        A2Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.A2,
            offsetValue = 0.7f,
            liveRedirection = false,
            redirectedWalkingIntensity = 0.8f,
            redirectionSliderValue = 0.3f
        });
        A1Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.A1,
            offsetValue = 0.7f,
            liveRedirection = false,
            redirectedWalkingIntensity = 0.6f,
            redirectionSliderValue = 0.3f
        });
        A4Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.A4,
            offsetValue = 0.1f,
            liveRedirection = false,
            redirectedWalkingIntensity = 1.4f,
            redirectionSliderValue = 0.3f
        });
        A3Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.A3,
            offsetValue = 0.1f,
            liveRedirection = false,
            redirectedWalkingIntensity = 1.0f,
            redirectionSliderValue = 0.3f
        });
        A2Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.A2,
            offsetValue = 0.1f,
            liveRedirection = false,
            redirectedWalkingIntensity = 0.8f,
            redirectionSliderValue = 0.3f
        });
        A1Combinations.Add(new VariablesCombination
        {
            taskCategory = TaskCategory.A1,
            offsetValue = 0.1f,
            liveRedirection = false,
            redirectedWalkingIntensity = 0.6f,
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
        //Shuffle(T1Combinations);
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
        //studyOrderCombination.Add(T0Combinations);

        // Shuffle the order of T1, T2, and T3 lists
        List<List<VariablesCombination>> t123Lists = new List<List<VariablesCombination>> { T2Combinations, T3Combinations };
        Shuffle(t123Lists);

        // Add shuffled T1, T2, and T3 lists to the order
        studyOrderCombination.AddRange(t123Lists);

        // Add T45 combinations to the order
        studyOrderCombination.Add(T45Combinations);
    }
    public void GenerateT6Study()
    {
        GenerateHardcodedCombinations();
        // Step 1: Duplicate the T6Combinations list.
        List<VariablesCombination> secondT6List = new List<VariablesCombination>(T6Combinations);

        // Step 2: Shuffle both lists.
        Shuffle(T6Combinations);
        Shuffle(secondT6List);

        studyOrderCombination = new List<List<VariablesCombination>> { T6Combinations, secondT6List };
    }
    public void ExpandAndShuffleALists()
    {
        // Expanding each list
        A1Combinations = ExpandList(A1Combinations);
        A2Combinations = ExpandList(A2Combinations);
        A3Combinations = ExpandList(A3Combinations);
        A4Combinations = ExpandList(A4Combinations);

        // Shuffling each expanded list
        Shuffle(A1Combinations);
        Shuffle(A2Combinations);
        Shuffle(A3Combinations);
        Shuffle(A4Combinations);
    }
    private List<VariablesCombination> ExpandList(List<VariablesCombination> originalList)
    {
        List<VariablesCombination> expandedList = new List<VariablesCombination>();
        foreach (var item in originalList)
        {
            for (int i = 0; i < 2; i++) // Repeat each entry 3 times
            {
                expandedList.Add(item);
            }
        }
        return expandedList;
    }
    public void GenerateStudyOrderCombination(int studyId)
{
    // Ensure A lists are expanded, shuffled, and ready to use
    ExpandAndShuffleALists();

    // Calculate the shift amount based on the study ID
    // This will determine how the lists A1-A4 are rearranged
    int shiftAmount = (studyId - 1) % 4; // Subtract 1 to make it zero-based, modulo 4 because there are 4 lists

    // Create a new list to hold the ordered combinations
    List<List<VariablesCombination>> orderedCombinations = new List<List<VariablesCombination>>();

    // Depending on the shiftAmount, reorder the lists
    switch (shiftAmount)
    {
        case 0:
            orderedCombinations.Add(A1Combinations);
            orderedCombinations.Add(A2Combinations);
            orderedCombinations.Add(A3Combinations);
            orderedCombinations.Add(A4Combinations);
            break;
        case 1:
            orderedCombinations.Add(A2Combinations);
            orderedCombinations.Add(A3Combinations);
            orderedCombinations.Add(A4Combinations);
            orderedCombinations.Add(A1Combinations);
            break;
        case 2:
            orderedCombinations.Add(A3Combinations);
            orderedCombinations.Add(A4Combinations);
            orderedCombinations.Add(A1Combinations);
            orderedCombinations.Add(A2Combinations);
            break;
        case 3:
            orderedCombinations.Add(A4Combinations);
            orderedCombinations.Add(A1Combinations);
            orderedCombinations.Add(A2Combinations);
            orderedCombinations.Add(A3Combinations);
            break;
    }

    // Now, orderedCombinations holds the lists in the correct order for this study ID
    // Assign it to studyOrderCombination to use elsewhere in your code
    studyOrderCombination = orderedCombinations;
}

}