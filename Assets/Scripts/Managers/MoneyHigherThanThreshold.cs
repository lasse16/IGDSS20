using UnityEngine;

[RequireComponent(typeof(GameManager))]
public class MoneyHigherThanThreshold : MonoBehaviour, IWinCondition
{

    [SerializeField] private int _threshold;
    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = GetComponent<GameManager>();
    }

    public string Reason()
    {
        return $"Money grew larger than {_threshold}";
    }

    public bool Satisfied()
    {
        var money = _gameManager.GetMoneyAmount();
        return money > _threshold;
    }
}
