using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

internal class HexagonalGrid : Map
{

    private static readonly XYPair[] XYDifferenceNeighboursOddRow = new XYPair[] { new XYPair(1, 0), new XYPair(1, -1), new XYPair(0, -1), new XYPair(-1, -1), new XYPair(-1, 0), new XYPair(0, 1) };
    private static readonly XYPair[] XYDifferenceNeighboursEvenRow = new XYPair[] { new XYPair(1, 1), new XYPair(1, 0), new XYPair(0, -1), new XYPair(-1, 0), new XYPair(-1, 1), new XYPair(0, 1) };

    private static readonly Dictionary<XYPair, HexEdge> _edgeNeighbourOddRow = new Dictionary<XYPair, HexEdge> {
        { new XYPair(1, 0), HexEdge.UpperRight},
        { new XYPair(1, -1), HexEdge.LowerRight},
        { new XYPair(0, -1), HexEdge.LowerCenter},
        { new XYPair(-1, -1), HexEdge.LowerLeft},
        { new XYPair(-1, 0), HexEdge.UpperLeft},
        { new XYPair(0, 1), HexEdge.UpperCenter}
    };

    private static readonly Dictionary<XYPair, HexEdge> _edgeNeighbourEvenRow = new Dictionary<XYPair, HexEdge> {
        { new XYPair(1, 1), HexEdge.UpperRight},
        { new XYPair(1, 0), HexEdge.LowerRight},
        { new XYPair(0, -1), HexEdge.LowerCenter},
        { new XYPair(-1, 0), HexEdge.LowerLeft},
        { new XYPair(-1, 1), HexEdge.UpperLeft},
        { new XYPair(0, 1), HexEdge.UpperCenter}
    };

    public Vector3 Origin { get; }

    private int gridWidth;
    private int gridHeight;
    private HexComponent[,] grid;

    private float cellHeight;
    private float cellWidth;

    private float maxWorldPosOnXAxis;
    private float maxWorldPosOnYAxis;
    private float maxWorldPosOnZAxis;


    public HexagonalGrid(int width, int height, Vector2 cellDimensions, Vector3 origin = new Vector3())
    {
        gridWidth = width;
        gridHeight = height;
        Origin = origin;
        grid = new HexComponent[width, height];

        //Use these, under the assumption that all tiles are equally sized
        cellWidth = cellDimensions[0];
        cellHeight = cellDimensions[1];

    }

    internal void AddCell(GameObject tile, int x, int y)
    {
        var worldPos = CalculateWorldPosition(x, y);

        var gridComponent = new HexComponent { RenderObject = tile, WorldPosition = worldPos };
        grid[x, y] = gridComponent;
    }

    internal void SetHeightOfCell(float height, int x, int y)
    {
        grid[x, y].SetHeight(height);
    }

    public override void Show()
    {
        var parent = new GameObject("MapGrid");
        parent.transform.position = Origin;

        foreach (var gridComponent in grid)
        {
            var worldPos = gridComponent.WorldPosition;

            var obj = gridComponent.RenderObject;
            obj.transform.position = worldPos;

            obj.name = $"({ worldPos.x}:{worldPos.z})";

            obj.transform.SetParent(parent.transform, false);
        }
    }

    public Dictionary<GameObject,HexEdge> FindNeighboursOfTile(int x, int y)
    {
        var neighbours = new Dictionary<GameObject,HexEdge>();

        var possibleNeighbours = GetPossibleNeigbours(x, y);
        foreach (var neighbour in possibleNeighbours)
        {
            var neighbourCooords = neighbour.Key;
            var neighbourEdge = neighbour.Value;

            if (0 <= neighbourCooords.X && neighbourCooords.X < gridWidth && 0 <= neighbourCooords.Y && neighbourCooords.Y < gridHeight)
            {
                neighbours.Add(GetTile(neighbourCooords.X, neighbourCooords.Y),neighbourEdge);
            }
        }

        return neighbours;
    }

    private Dictionary<XYPair,HexEdge> GetPossibleNeigbours(int x, int y)
    {
        var even = (x & 1) == 0;
        var differences = even ?  _edgeNeighbourEvenRow : _edgeNeighbourOddRow;
        var res = new Dictionary<XYPair, HexEdge>();

        foreach (var pair in differences)
        {
            var diff = pair.Key;
            var edge = pair.Value;
            res.Add(new XYPair(x + diff.X, y + diff.Y),edge);
        }

        return res;
    }

    public GameObject GetTile(int x, int y)
    {
        return grid[x, y].RenderObject;
    }

    private Vector3 CalculateWorldPosition(int x, int y)
    {
        bool insettedRow = x % 2 == 0;
        var inset = Convert.ToInt32(insettedRow) * (cellHeight * 0.5f);

        var worldPosX = x * cellWidth * 0.75f;
        var worldPosY = 0;
        var worldPosZ = y * cellHeight + inset;

        maxWorldPosOnXAxis = worldPosX > maxWorldPosOnXAxis ? worldPosX : maxWorldPosOnXAxis;
        maxWorldPosOnYAxis = worldPosY > maxWorldPosOnYAxis ? worldPosY : maxWorldPosOnYAxis;
        maxWorldPosOnZAxis = worldPosZ > maxWorldPosOnZAxis ? worldPosZ : maxWorldPosOnZAxis;

        return new Vector3(worldPosX, worldPosY, worldPosZ);
    }

    public override Boundaries GetBoundaries()
    {
        var lowerBounds = Origin;
        lowerBounds.y = maxWorldPosOnYAxis;
        //TODO Fix rotation error - this fails if the final map is rotated in any way
        //TODO Set sensible max zoom out level
        var upperBounds = new Vector3(maxWorldPosOnXAxis, Mathf.Infinity, maxWorldPosOnZAxis);
        return new Boundaries(Origin, upperBounds);
    }

    private class HexComponent
    {
        public GameObject RenderObject { get; set; }
        public Vector3 WorldPosition { get; set; }

        internal void SetHeight(float height)
        {
            WorldPosition = new Vector3(WorldPosition.x, height, WorldPosition.z);
        }
    }

    public readonly struct XYPair
    {
        public XYPair(int x, int y)
        {
            X = x;
            Y = y;
        }

        public readonly int X;
        public readonly int Y;

        public override bool Equals(object obj)
        {
            if (obj is XYPair)
            {
                var other = (XYPair) obj;
                return other.X == X && other.Y == Y;
            }
            return false;
        }

        public override int GetHashCode()
        {
            //Stolen from MIT Class
            //https://ocw.mit.edu/courses/electrical-engineering-and-computer-science/6-046j-introduction-to-algorithms-sma-5503-fall-2005/video-lectures/lecture-7-hashing-hash-functions/
            return X * 31 + Y;
        }
    }
}

public enum HexEdge
{
    UpperLeft = 0,
    UpperCenter = 1,
    UpperRight = 2,
    LowerRight = 3,
    LowerCenter = 4,
    LowerLeft = 5,
}
