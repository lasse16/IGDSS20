using Assets.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    public GeneralBuildingStats GeneralBuildingStats;
    public IStorage Storage;

    [ReadOnly]  
    public Tile Tile;
    public abstract bool ConstructOnTile(Tile tile, IStorage storage);


}

