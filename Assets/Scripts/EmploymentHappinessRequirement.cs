
using Assets.Scripts;
using UnityEngine;

[RequireComponent(typeof(Worker))]
public class EmploymentHappinessRequirement : MonoBehaviour, IHappinessRequirement
{
    [SerializeField] private int _importance;
    private Worker _worker;

    private void Awake()
    {
        _worker = GetComponent<Worker>();
    }

    public float CheckFulfillment()
    {
        var employed = _worker.Workplace is null;
        return employed ? 1 : 0;
    }

    public int GetImportance() => _importance;
}
