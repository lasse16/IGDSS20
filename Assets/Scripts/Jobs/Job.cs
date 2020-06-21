using Assets.Scripts;
using IGDSS20.Buildings;
using System;

namespace IGDSS20.Jobs
{
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

        internal void Employ(Worker worker)
        {
            Worker = worker;
        }

        internal void Fire()
        {
            Worker = null;
        }
    }
}