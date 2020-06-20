using IGDSS20.Buildings;
using IGDSS20.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingManager : MonoBehaviour, BuildingSelection.IGameplayActions
{
    public BuildingPrefabs Buildings;
    private BuildingSelection _buildingSelection;
    private List<Building> PlacedBuildings = new List<Building>();
    private BuildingType _currentActivePlacement = BuildingType.Lumberjack;


    private void Awake()
    {
        _buildingSelection = _buildingSelection ?? new BuildingSelection();
        _buildingSelection.Gameplay.SetCallbacks(this);
    }
    private void OnEnable()
    {

        _buildingSelection.Enable();
    }
    private void OnDisable()
    {
        _buildingSelection.Disable();
    }


    public int GetUpkeepCost()
    {
        var total = 0;
        foreach (var building in PlacedBuildings)
        {
            total += building.GeneralBuildingStats.UpkeepCost;
        }
        return total;
    }

    public void AddPlacedBuilding(Building building)
    {
        PlacedBuildings.Add(building);
    }

    public Building GetBuildingOfType(BuildingType type)
    {
        GameObject prefab;

        //This can be much cleaner in C#8
        switch (type)
        {
            case BuildingType.Fishery:
                prefab = Buildings.Fishery;
                break;
            case BuildingType.Lumberjack:
                prefab = Buildings.LumberJack;
                break;
            case BuildingType.Sawmill:
                prefab = Buildings.Sawmill;
                break;
            case BuildingType.SheepFarm:
                prefab = Buildings.SheepFarm;
                break;
            case BuildingType.FrameworkKnitters:
                prefab = Buildings.FrameworkKnitters;
                break;
            case BuildingType.PotatoFarm:
                prefab = Buildings.PotatoFarm;
                break;
            case BuildingType.SchnappsDistillery:
                prefab = Buildings.SchnappsDistillery;
                break;
            case BuildingType.FarmersResidence:
                prefab = Buildings.FarmersResidence;
                break;
            default:
                throw new Exception($"Unknown building type - {Enum.GetName(typeof(BuildingType), type)}");
        }

        var obj = Instantiate(prefab);
        return obj.GetComponent<Building>();
    }

    #region keyboard input handling
    //TODO find better solution
    public BuildingType GetCurrentPlacementBuilding() => _currentActivePlacement;

    public void SetRequestedBuildingToLumberjack() => _currentActivePlacement = BuildingType.Lumberjack;
    public void SetRequestedBuildingToFishery() => _currentActivePlacement = BuildingType.Fishery;
    public void SetRequestedBuildingToSawmill() => _currentActivePlacement = BuildingType.Sawmill;
    public void SetRequestedBuildingToFrameworkKnitter() => _currentActivePlacement = BuildingType.FrameworkKnitters;
    public void SetRequestedBuildingToSheepfarm() => _currentActivePlacement = BuildingType.SheepFarm;
    public void SetRequestedBuildingToPotatoFarm() => _currentActivePlacement = BuildingType.PotatoFarm;
    public void SetRequestedBuildingToSchnappsDistillery() => _currentActivePlacement = BuildingType.SchnappsDistillery;
    private void SetRequestedBuildingToFarmersResidence() => _currentActivePlacement = BuildingType.FarmersResidence;

    public void OnSelectBuilding1(InputAction.CallbackContext context) => SetRequestedBuildingToFishery();
    public void OnSelectBuilding2(InputAction.CallbackContext context) => SetRequestedBuildingToLumberjack();
    public void OnSelectBuilding3(InputAction.CallbackContext context) => SetRequestedBuildingToSawmill();
    public void OnSelectBuilding4(InputAction.CallbackContext context) => SetRequestedBuildingToSheepfarm();
    public void OnSelectBuilding5(InputAction.CallbackContext context) => SetRequestedBuildingToFrameworkKnitter();
    public void OnSelectBuilding6(InputAction.CallbackContext context) => SetRequestedBuildingToPotatoFarm();
    public void OnSelectBuilding7(InputAction.CallbackContext context) => SetRequestedBuildingToSchnappsDistillery();

    public void OnSelectBuilding8(InputAction.CallbackContext context) => SetRequestedBuildingToFarmersResidence();

    public void OnSelectBuilding9(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    #endregion
}