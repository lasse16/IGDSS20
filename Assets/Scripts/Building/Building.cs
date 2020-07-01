using IGDSS20.Helpers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace IGDSS20.Buildings
{
    public abstract class Building : MonoBehaviour
    {
        public GeneralBuildingStats GeneralBuildingStats;
        public IStorage Storage;

        [ReadOnly]
        public Tile Tile;
        public abstract bool ConstructOnTile(Tile tile, IStorage storage);

        // for navigation
        public List<(Tile, int)> potentialFieldsList;
    }
}