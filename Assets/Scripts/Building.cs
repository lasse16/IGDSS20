using Assets.Scripts;
using UnityEngine;

public class Building : MonoBehaviour
{
    public GeneralBuildingStats GeneralBuildingStats;
    public IStorage WareHouse;

    [HideInInspector]
    public Tile tile;

}

