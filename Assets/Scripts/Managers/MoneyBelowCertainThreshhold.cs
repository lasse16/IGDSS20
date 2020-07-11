using System.CodeDom;
using UnityEngine;

[RequireComponent(typeof(GameManager))]
public class MoneyBelowCertainThreshhold : MonoBehaviour, ILossCondition
{
    [SerializeField] private int _threshold;
    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = GetComponent<GameManager>();
    }

    public string Reason()
    {
        return $"Money fell below {_threshold}";
    }

    public bool Satisfied()
    {
        return _gameManager.GetMoneyAmount() < _threshold;
    }
}
