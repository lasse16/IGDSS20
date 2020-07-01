using IGDSS20.Buildings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.WSA;

namespace IGDSS20.Assets.Scripts.Navigation
{
    public class PotentialMap
    {
        private Dictionary<Tile, int> _weightMap;
        private Tile _startingPoint;

        public PotentialMap(Tile startingPoint)
        {
            _weightMap = new Dictionary<Tile, int>();
            _startingPoint = startingPoint;

            _weightMap.Add(startingPoint, 0);
        }

        public bool Add(Tile tile, int weight)
        {
            bool unknownTile = !_weightMap.ContainsKey(tile);

            if (unknownTile)
                _weightMap.Add(tile, weight);

            return unknownTile;
        }

        public void UpdateWeight(Tile tile, int newWeight)
        {
            _weightMap[tile] = newWeight;
        }

        public int GetWeight(Tile tile)
        {
            return _weightMap[tile];
        }

        public override string ToString()
        {
            var str = "";

            foreach (var item in _weightMap)
            {
                str += $"({item.Key}, {item.Value}), \n";
            }

            return str;
        }
    }
}
