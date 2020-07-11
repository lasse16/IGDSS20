using UnityEngine;

[RequireComponent(typeof(GameManager))]
public class PopulationHigherThanThreshold : MonoBehaviour, IWinCondition
{
    [SerializeField] private int _threshold;
    [SerializeField] private PopulationSet _population;

    public string Reason()
    {
        return $"Population grew larger than {_threshold}";
    }

    public bool Satisfied()
    {
        return _population.GetPopulationSize() > _threshold;
    }
}
