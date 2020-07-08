using IGDSS20.Buildings;
using IGDSS20.Enums;
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
    [SerializeField] private NavigationManager _navigationManager;
    [SerializeField] private TileSet _tileSet;
    [SerializeField] private BuildingManager _buildingManager;
    [Tooltip("Allow the camera to move past the map's boundaries in relation to the camera angle")]
    [SerializeField] private bool AllowCameraAngleToInfluenceBoundaries;

    [SerializeField] private int _moneyPool;
    private IWinCondition[] _winConditions;
    private ILossCondition[] _lossConditions;

    //TODO hardcoded for now
    private const int tickIntervalInSeconds = 5;
    private const int taxRateEmployed = 5;
    private const int taxRateUnemployed = 2;
    private const int taxRateRetiree = 1;


    private void Awake()
    {
        _winConditions = GetComponents<IWinCondition>();
        _lossConditions = GetComponents<ILossCondition>();
    }


    void Start()
    {
        _ = _heightmap ?? throw new ArgumentNullException("Heightmap for map generation has not been set! Set one in the GameManager script");


        var base_map = _mapManager.FromHeightmap(_heightmap, _tileSet, 5);
        _mapManager.DrawMap(base_map);

        var bounds = base_map.GetBoundaries();

        if (AllowCameraAngleToInfluenceBoundaries)
            bounds.OffsetBoundaries(cameraManager.CalculateOffsetFromCameraAngle());

        cameraManager.cameraBoundaries = bounds;

        InvokeRepeating("TickEconomy", 0, tickIntervalInSeconds);
    }

    private void Update()
    {
        foreach (var item in _winConditions)
        {
            if (item.Satisfied())
            {
                EndGame();
                DisplayWonMessage(item);
            }
        }

        foreach (var item in _lossConditions)
        {
            if (item.Satisfied())
            {
                EndGame();
                DisplayLossMessage(item);
            }
        }
    }

    private void DisplayLossMessage(ILossCondition item)
    {
        print($"Lost: {item.Reason()}");
    }

    private void DisplayWonMessage(IWinCondition item)
    {
        print($"Won:  {item.Reason()}");
    }

    [ContextMenu("ForceEconomyTick")]
    private void TickEconomy()
    {
        // constant income
        _moneyPool += 100;

        var taxesEmployed = _jobManager.GetAmountOfEmployedWorkers() * taxRateEmployed;
        var taxesUnemployed = _jobManager.GetAmountOfUnemployedWorkers() * taxRateUnemployed;
        var taxesRetiree = _jobManager.GetAmountOfRetirees() * taxRateRetiree;

        var taxes = taxesEmployed + taxesRetiree + taxesUnemployed;
        _moneyPool += taxes;

        var upkeepCost = _buildingManager.GetUpkeepCost();
        _moneyPool -= upkeepCost;
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

        var moneyAvailable = _moneyPool >= building.GeneralBuildingStats.BuildCostMoney;
        var resourceAvailable = storage.GetResourceIfAvailable(ResourceType.Plank, building.GeneralBuildingStats.BuildCostPlanks);
        var allowedTileType = building.GeneralBuildingStats.AllowedTileTypes.Contains(tile.Type);

        if (moneyAvailable && resourceAvailable && allowedTileType)
        {
            _moneyPool -= building.GeneralBuildingStats.BuildCostMoney;
            building.ConstructOnTile(tile, storage);
            _buildingManager.AddPlacedBuilding(building);
            building.potentialFieldsList = _navigationManager.CreatePotentialFieldMapFor(building);
        }
        else
        {
            print($"Placement of building failed. allowed tile : {allowedTileType} , resources available {resourceAvailable} , money available {moneyAvailable}");
            Destroy(building.gameObject);
        }
    }

    public int GetMoneyAmount() => _moneyPool;

    public void EndGame()
    {
        StopCoroutine("TickEconomy");
        Time.timeScale = 0;

        print("Game ended");
    }
}
