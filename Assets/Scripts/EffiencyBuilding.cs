using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class EffiencyBuilding : Building
    {
        protected List<IEfficiencyRequirement> _efficiencyRequirements = new List<IEfficiencyRequirement>();
        protected float _effectiveGenerationInterval;

        [SerializeField]
        [ReadOnly]
        protected float _efficiency;



        protected float CalculateEfficiency()
        {
            var total = 0f;
            var totalImportance = 0;

            foreach (var requirement in _efficiencyRequirements)
            {
                var fulfillment = requirement.CheckFulfillment();
                var importance = requirement.GetImportance();

                //Convention for must-be requirements
                if (importance == -1 && fulfillment == 0)
                    return 0;

                totalImportance += importance;


                total += fulfillment * importance;
            }

            if (totalImportance == 0)
                return 1;

            return total / totalImportance;
        }

        protected float CalculateEffectiveInterval(float BaseIntervalInSeconds)
        {
            return (1 / CalculateEfficiency()) * BaseIntervalInSeconds;
        }

    }
}
