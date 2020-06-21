using IGDSS20.Enums;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileType Type;
    public List<Tile> NeighbouringTiles;
}
