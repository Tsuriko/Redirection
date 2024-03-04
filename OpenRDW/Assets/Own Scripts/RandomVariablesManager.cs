using System;
using System.Collections.Generic;
using UnityEngine;

public class RandomVariablesManager : MonoBehaviour
{
    [Serializable]
    public struct VariablesCombination
    {
        public float offsetValue;
        public int personRedirected; // Changed from float to int to match the data type
        public bool liveRedirection;
        public float redirectedWalkingIntensity;
    }

    [Header("Seed for Random Generation")]
    [SerializeField] private int seed = 12345; // Default seed, can be set in Inspector

    private readonly float[] offsetValues = { -2, -1, 1, 2 };
    private readonly int[] personRedirected = { 0, 1, 2 }; // 0 = both, 1 = host, 2 = other
    private readonly bool[] liveRedirectionValues = { true, false };
    private readonly float[] redirectedWalkingIntensity = { 0.5f, 0.7f, 1f };

    public List<VariablesCombination> AllCombinations { get; private set; }

    private void Awake()
    {
        GenerateAllPossibleCombinations();
    }

    public void SetSeedWithStudyId(int studyId)
    {
        seed += studyId; // Adds the study ID to the existing seed
    }

    public void GenerateAllPossibleCombinations()
    {
        // Use the provided seed for repeatable results
        UnityEngine.Random.InitState(seed);

        AllCombinations = new List<VariablesCombination>();

        foreach (var offsetValue in offsetValues)
        {
            foreach (var person in personRedirected)
            {
                foreach (var liveRedirection in liveRedirectionValues)
                {
                    foreach (var intensity in redirectedWalkingIntensity)
                    {
                        VariablesCombination combination = new VariablesCombination
                        {
                            offsetValue = offsetValue, // Corrected to match struct member name
                            personRedirected = person, // Corrected to match struct member name
                            liveRedirection = liveRedirection, // Corrected to match struct member name
                            redirectedWalkingIntensity = intensity // Corrected to match struct member name
                        };

                        AllCombinations.Add(combination);
                    }
                }
            }
        }
    }
}