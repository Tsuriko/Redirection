using System;
using System.Collections.Generic;
using UnityEngine;

public class RandomVariablesManager : MonoBehaviour
{
    [Serializable]
    public struct VariablesCombination
    {
        public float OffsetMaster;
        public float OffsetOther;
        public Algorithm SelectedAlgorithm;
        public float HandToWalkingRedirectionRatio;
    }

    public enum Algorithm
    {
        midpointRedrection,
        otherHandRedirection
    }

    [Header("Seed for Random Generation")]
    [SerializeField] private int seed = 12345; // Default seed, can be set in Inspector

    private readonly float[] offsetMasterValues = {-1,-0,1};
    private readonly float[] offsetOtherValues = { -1,-0,1};
    private readonly Algorithm[] algorithmValues = { Algorithm.midpointRedrection, Algorithm.otherHandRedirection};
    private readonly float[] redirectedWalkingIntensity = { 0.8f, 0.9f, 1f }; // Represented as 20/80, 25/75, 30/70 ratios

    public List<VariablesCombination> AllCombinations { get; private set; }

    private void Awake()
    {
        GenerateAllPossibleCombinations();
    }

    public void GenerateAllPossibleCombinations()
    {
        // Use the provided seed for repeatable results
        UnityEngine.Random.InitState(seed);

        AllCombinations = new List<VariablesCombination>();

        foreach (var offsetMaster in offsetMasterValues)
        {
            foreach (var offsetOther in offsetOtherValues)
            {
                foreach (var algorithm in algorithmValues)
                {
                    foreach (var ratio in redirectedWalkingIntensity)
                    {
                        VariablesCombination combination = new VariablesCombination
                        {
                            OffsetMaster = offsetMaster,
                            OffsetOther = offsetOther,
                            SelectedAlgorithm = algorithm,
                            HandToWalkingRedirectionRatio = ratio
                        };

                        AllCombinations.Add(combination);
                    }
                }
            }
        }
    }
}