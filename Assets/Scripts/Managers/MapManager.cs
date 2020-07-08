using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapManager : MonoBehaviour
{
    //Hardcoded for now, maybe some way of Renderer.Mesh.bounds in order to get the dimensions
    //It's the z dimension of the tile
    private float CELL_HEIGHT = 10;

    public Map FromHeightmap(Texture2D _heightmap, TileSet tileSet, float heightScaling = 1)
    {
        var width = _heightmap.width;
        var height = _heightmap.height;

        var dimensions = new Vector2();
        dimensions.x = CELL_HEIGHT / Mathf.Sqrt(3) * 2;
        dimensions.y = CELL_HEIGHT;

        var grid = new HexagonalGrid(width, height, dimensions);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var color = _heightmap.GetPixel(x, y);
                var grayValue = color.grayscale;
                GameObject tile = Instantiate(GetTileFromColor(color, tileSet).gameObject);

                grid.AddCell(tile, x, y);
                grid.SetHeightOfCell(grayValue * heightScaling, x, y);
            }
        }

        //TODO Set up neighbouring relations on grid addition
        //As the hex grid is static, neighbouring relations are only defined by row. Hence they can be known prior to fully filling in the grid.
        // The moment a new gameobject is added, all neighbours can be updated, and add the new cell to their list.
        //Make sure to add existing ones to the list of the new cell too.
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var tile = grid.GetTile(x, y);
                var neighbouringGameObjects = grid.FindNeighboursOfTile(x, y);
                Tile component = tile.GetComponent<Tile>();

                //Damn fancy LINQ stuff, VS refactor FTW
                List<Tile> neighbours = (from item in neighbouringGameObjects.Keys
                                         select item.GetComponent<Tile>()).ToList();

                foreach (var item in neighbouringGameObjects)
                {
                    var obj = item.Key;
                    if (component.Type == obj.GetComponent<Tile>().Type)
                        component.Edges.DisableEdge(item.Value);
                }

                component.NeighbouringTiles = neighbours;
            }
        }

        return grid;
    }

    public void DrawMap(Map map)
    {
        map.Show();
    }

    private Tile GetTileFromColor(Color color, TileSet _tileSet)
    {
        //As long as heightmap is only in grayscale, these two are equivalent
        //But the assignment description especially asks for the max value of all color channels
        //var colorValue = color.grayscale;
        var colorValue = Math.Max(color.r, Math.Max(color.g, color.b));

        switch (colorValue)
        {
            case 0:
                return _tileSet.WaterTile;
            case float _ when colorValue < 0.2:
                return _tileSet.SandTile;
            case float _ when colorValue < 0.4:
                return _tileSet.GrassTile;
            case float _ when colorValue < 0.6:
                return _tileSet.ForestTile;
            case float _ when colorValue < 0.8:
                return _tileSet.StoneTile;
            case float _ when colorValue <= 1.0:
                return _tileSet.MountainTile;

            default:
                return _tileSet.WaterTile;
        }
    }

}