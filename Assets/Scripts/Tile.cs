using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Enums
{
    public class Tile : MonoBehaviour
    {
        public TileType _type;
        public List<Tile> NeighbouringTiles;
    }
}