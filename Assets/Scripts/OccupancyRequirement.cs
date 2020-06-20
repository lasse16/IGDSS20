using UnityEngine;

[RequireComponent(typeof(ProductionBuilding))]
public class OccupancyRequirement : MonoBehaviour, IEfficiencyRequirement
{
    [SerializeField] private int _importance;
  
    private ProductionBuilding _building;
    private int _requiredWorker;

    private void Awake()
    {
        _building = GetComponent<ProductionBuilding>();
        _requiredWorker = _building.ProductionBuildingStats.JobsAvailable;
    }

    public float CheckFulfillment()
    {
        return _building.GetOccupants().Count / _requiredWorker;  
    }

    public int GetImportance() => _importance;
}

