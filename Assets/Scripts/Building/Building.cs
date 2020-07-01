using IGDSS20.Assets.Scripts.Navigation;
using IGDSS20.Helpers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace IGDSS20.Buildings
{
    public abstract class Building : MonoBehaviour
    {
        public GeneralBuildingStats GeneralBuildingStats;
        public PotentialMap potentialFieldsList;
        public IStorage Storage;

        [ReadOnly]
        public Tile Tile;
        public abstract bool ConstructOnTile(Tile tile, IStorage storage);

    }
}