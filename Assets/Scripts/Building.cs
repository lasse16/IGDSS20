using Assets.Scripts;
using System;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    public GeneralBuildingStats GeneralBuildingStats;
    public IStorage Storage;

    [HideInInspector]
    public Tile Tile;

    public abstract bool ConstructOnTile(Tile tile, IStorage storage);

}

