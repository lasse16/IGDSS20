using UnityEngine;

[RequireComponent(typeof(ProductionBuilding))]
public class NeighbouringTileRequirement : MonoBehaviour, IEfficiencyRequirement
{
    [SerializeField] private int _importance;
    private ProductionBuilding _productionBuilding;

    private void Awake()
    {
        _productionBuilding = GetComponent<ProductionBuilding>();
    }
    public float CheckFulfillment()
    {
        var neighbouringTiles = _productionBuilding.Tile.NeighbouringTiles;
        var buildingStats = _productionBuilding.ProductionBuildingStats;

        if (buildingStats.EfficientNeighboringTile == TileType.None)
            return 1;

        int counter = 0;
        foreach (var item in neighbouringTiles)
        {
            if (item.Type == buildingStats.EfficientNeighboringTile)
                counter++;
        }

        var efficiency = (counter - buildingStats.MinEfficientNeigbor) / (buildingStats.MaxEfficientNeighbor - buildingStats.MinEfficientNeigbor);
        return Mathf.Clamp(efficiency, 0, 1);
    }

    public int GetImportance() => _importance;
}

