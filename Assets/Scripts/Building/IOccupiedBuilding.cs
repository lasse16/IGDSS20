using Assets.Scripts;
using System.Collections.Generic;

namespace IGDSS20.Buildings
{
    public interface IOccupiedBuilding
    {
        List<Worker> GetOccupants();
    }
}