using IGDSS20.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace IGDSS20.Assets.Scripts.Navigation
{
    [CreateAssetMenu(fileName ="data",menuName ="ScriptableObjects/TileWeights")]
    public class TileWeights : ScriptableObject
    {
        public int WaterTile;
        public int SandTile;
        public int GrassTile;
        public int ForestTile;
        public int StoneTile;
        public int MountainTile;
        public int Default;

        public int GetWeightForTileType(TileType type)
        {
            switch (type)
            {
                case TileType.Water:
                    return WaterTile;
                case TileType.Sand:
                    return SandTile;
                case TileType.Grass:
                    return GrassTile;
                case TileType.Forest:
                    return ForestTile;
                case TileType.Stone:
                    return StoneTile;
                case TileType.Mountain:
                    return MountainTile;
                default:
                    return Default;
            }
        }
    }
}
