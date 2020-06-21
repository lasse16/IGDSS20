using UnityEngine;

namespace IGDSS20.Buildings
{
    [CreateAssetMenu(fileName = "data", menuName = "ScriptableObjects/Buildings")]
    public class BuildingPrefabs : ScriptableObject
    {
        //TODO Find a serialized dictionary 
        //This requires a manual change each time abuilding is added to the enum
        public GameObject Fishery;
        public GameObject LumberJack;
        public GameObject Sawmill;
        public GameObject FrameworkKnitters;
        public GameObject SheepFarm;
        public GameObject PotatoFarm;
        public GameObject SchnappsDistillery;
        public GameObject FarmersResidence;
    }
}