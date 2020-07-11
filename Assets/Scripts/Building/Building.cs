using IGDSS20.Assets.Scripts.Navigation;
using IGDSS20.Helpers;
using System;
using System.Collections.Generic;
using TMPro;
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


        [ContextMenu("DisplayPotentialMap")]
        private void DisplayPotentialMap()
        {
            var parent = new GameObject("PotentialMap");
            foreach (var tilePotential in potentialFieldsList)
            {
                var tile = tilePotential.Key;
                var potential = tilePotential.Value;

                var text = new GameObject().AddComponent<TextMeshPro>();

                //Adjust for offsetted tile origin
                text.transform.position = tile.gameObject.transform.position + new Vector3(8, 1, 0);
                text.transform.rotation = Quaternion.Euler(90, 0, 0);
                text.transform.SetParent(parent.transform);
                text.name = tile.ToString();
                text.SetText($"{potential}");
            }

        }

    }
}