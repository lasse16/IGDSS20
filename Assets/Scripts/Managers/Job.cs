using Assets.Scripts;
using System;

[Serializable]
public class Job
{
    public ProductionBuilding Workplace;
    public Worker Worker;

    public Job(Worker worker, ProductionBuilding workPlace)
    {
        Workplace = workPlace;
        Worker = worker;
    }

    public Job(ProductionBuilding workPlace)
    {
        Workplace = workPlace;
    }
}



