using Assets.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private MouseManager mouseManager;
    [SerializeField] private Texture2D _heightmap;
    [SerializeField] private MapManager _mapManager;
    [SerializeField] private JobManager _jobManager;
    [SerializeField] private TileSet _tileSet;
    [SerializeField] private BuildingManager _buildingManager;
    [Tooltip("Allow the camera to move past the map's boundaries in relation to the camera angle")]
    [SerializeField] private bool AllowCameraAngleToInfluenceBoundaries;

    [SerializeField] private int moneyPool;

    //TODO hardcoded for now
    private const int tickIntervalInSeconds = 60;
    private const int taxRateEmployed = 5;
    private float timeSinceLastTick;

    void Start()
    {
        _ = _heightmap ?? throw new ArgumentNullException("Heightmap for map generation has not been set! Set one in the GameManager script");


        var base_map = _mapManager.FromHeightmap(_heightmap, _tileSet, 5);
        _mapManager.DrawMap(base_map);

        var bounds = base_map.GetBoundaries();

        if (AllowCameraAngleToInfluenceBoundaries)
            bounds.OffsetBoundaries(cameraManager.CalculateOffsetFromCameraAngle());

        cameraManager.cameraBoundaries = bounds;

    }


    void Update()
    {
        timeSinceLastTick += Time.deltaTime;

        if (timeSinceLastTick > tickIntervalInSeconds)
        {
            TickEconomy();
            timeSinceLastTick = -tickIntervalInSeconds;
        }
    }

    [ContextMenu("ForceEconomyTick")]
    private void TickEconomy()
    {
        // constant income
        moneyPool += 100;

        var taxesEmployed = _jobManager.GetAmountOfEmployedWorkers() * taxRateEmployed;
        var taxesUnemployed = _jobManager.GetAmountOfUnemployedWorkers() * 2;
        var taxesRetiree = _jobManager.GetAmountOfRetirees() * 1;

        var taxes = taxesEmployed + taxesRetiree + taxesUnemployed;
        moneyPool += taxes;

        var upkeepCost = _buildingManager.GetUpkeepCost();
        moneyPool -= upkeepCost;
    }

    public void SpawnBuilding(Vector3 mousePosition)
    {
        //TODO fix preconfigured ware house
        var storage = GetComponent<IStorage>();
        var tile = mouseManager.GetClickedTile(mousePosition);

        if (tile is null)
            return;

        var requiredBuildingType = _buildingManager.GetCurrentPlacementBuilding();



        var building = _buildingManager.GetBuildingOfType(requiredBuildingType);

        var moneyAvailable = moneyPool >= building.ProductionBuildingStats.BuildCostMoney;
        var resourceAvailable = storage.GetResourceIfAvailable(ResourceType.Plank, building.ProductionBuildingStats.BuildCostPlanks);
        var allowedTileType = building.GetSupportedTiles().Contains(tile.Type);

        if (moneyAvailable && resourceAvailable && allowedTileType)
        {
            moneyPool -= building.ProductionBuildingStats.BuildCostMoney;
            _buildingManager.PlaceBuildingOnTile(building, tile);
            building.WareHouse = storage;
        }
        else
        {
            print($"Placement of building failed. allowed tile : {allowedTileType} , resources available {resourceAvailable} , money available {moneyAvailable}");
            Destroy(building.gameObject);
        }
    }
}
