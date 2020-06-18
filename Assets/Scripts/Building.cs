using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public GeneralBuildingStats GeneralBuildingStats;
    public IStorage WareHouse;

    [HideInInspector]
    public Tile tile;

    
   
    #region Workers
    public List<Worker> _workers; //List of all workers associated with this building, either for work or living
    #endregion

    #region Methods   
    public void WorkerAssignedToBuilding(Worker w)
    {
        _workers.Add(w);
    }

    public void WorkerRemovedFromBuilding(Worker w)
    {
        _workers.Remove(w);
    }
    #endregion

}