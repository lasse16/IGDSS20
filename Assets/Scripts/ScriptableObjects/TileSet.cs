using UnityEngine;

[CreateAssetMenu(fileName = "data", menuName = "ScriptableObjects/TileSet", order = 1)]
public class TileSet : ScriptableObject
{
    public Tile WaterTile;
    public Tile SandTile;
    public Tile GrassTile;
    public Tile ForestTile;
    public Tile StoneTile;
    public Tile MountainTile;
}