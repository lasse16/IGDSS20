using UnityEngine;

[CreateAssetMenu(fileName = "data", menuName = "ScriptableObjects/HousingBuildingStats")]
public class HousingBuildingStats : GeneralBuildingStats
{
    [Tooltip("Number of maximal inhabitants")]
    public int MaxInhabitants;

    [Tooltip("Number of workers at the placement of the building")]
    public int StartingInhabitants;

    [Tooltip("Interval in which new workers are created")]
    public int WorkerGenerationIntervalInSeconds;
}