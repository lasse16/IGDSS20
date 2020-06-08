using UnityEngine;

[CreateAssetMenu(fileName = "data", menuName = "ScriptableObjects/HousingBuildingStats")]
public class HousingBuildingStats : GeneralBuildingStats
{
    [Tooltip("Number of maximal inhabitants")]
    public int MaxInhabitants;
}