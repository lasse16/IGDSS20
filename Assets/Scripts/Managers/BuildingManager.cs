using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    private RunTimeSet<Building> PlacedBuildings;


    public int GetUpkeepCost()
    {
        var total = 0;
        foreach (var building in PlacedBuildings)
        {
            total += building.GetUpkeepCost();
        }
        return total;
    }
}