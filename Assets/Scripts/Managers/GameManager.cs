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
    [SerializeField] private TileSet _tileSet;
    [SerializeField] private BuildingManager _buildingManager;
    [Tooltip("Allow the camera to move past the map's boundaries in relation to the camera angle")]
    [SerializeField] private bool AllowCameraAngleToInfluenceBoundaries;

    [SerializeField] private int moneyPool;

    //TODO hardcoded for now
    private const int tickIntervalInSeconds = 60;
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


    private void TickEconomy()
    {
        // constant income
        moneyPool += 100;
        var upkeepCost = _buildingManager.GetUpkeepCost();
        moneyPool = -upkeepCost;
    }

    public void SpawnBuilding(Vector3 mousePosition)
    {
        var wareHouse = GetComponent<WareHouse>();
        var tile = mouseManager.GetClickedTile(mousePosition);

        //TODO Select bulding type
        var requiredBuildingType = BuildingType.Fishery;
        var building = _buildingManager.GetBuildingOfType(requiredBuildingType);


        var buildingScript = building.GetComponent<Building>();
        var moneyAvailable = moneyPool > buildingScript.GeneralBuildingStats.BuildCostMoney;

        if (moneyAvailable && wareHouse.GetResourceIfAvailable(ResourceType.Plank, buildingScript.GeneralBuildingStats.BuildCostPlanks))
            _buildingManager.PlaceBuildingOnTile(buildingScript, tile);
    }
}
