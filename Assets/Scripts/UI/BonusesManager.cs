using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum Bonuses
{
    Shield = 0,
    DoubleGun = 1,
    Magnet = 2,
    DoubleCrystal = 3,
    DoubleScore = 4,
    Hyperdrive = 5
};
public class BonusesManager : MonoBehaviour
{
    private const int SIZE = 6;

    public static BonusesManager instance;

    public UnityAction Hyperdrive;
    public float[] TimeOfBonus { get; private set; } = new float[SIZE];
    public float HyperdriveTime => TimeOfBonus[SIZE - 1];
    public bool IsBonus => TimeOfBonus.Any(i => i > 0);

    [SerializeField] private BonusAnimator[] _bonus;
    [SerializeField] private Image[] _bonusIndicators;

    private bool[] _enableAnimates = new bool[SIZE];
    private Player _player;


    public void Initialize()
    {
        instance = this;
        _player = Player.instance;
        _player.GetBerk += InitializeBonus;
    }
    private void OnDisable() => _player.GetBerk -= InitializeBonus;
    private void InitializeBonus(int id)
    {
        TimeOfBonus[id] = ShopOfBonuses.timeOfBonus[id];
        _enableAnimates[id] = true;
        _bonus[id].Animate(false);
        _bonus[id].gameObject.SetActive(true);
        _bonusIndicators[id].fillAmount = 1;
        if (id == (int)Bonuses.Hyperdrive) Hyperdrive?.Invoke();
    }
    private void Start()
    {
        for (var i = 0; i < SIZE; i++)
        {
            _bonus[i].gameObject.SetActive(false);
        }
        StartCoroutine(ManageBonusCoroutine());
    }
    private IEnumerator ManageBonusCoroutine()
    {
        var delay = new WaitForSeconds(1);
        while (true)
        {
            yield return delay;
            if (TimeOfBonus[(int)Bonuses.Hyperdrive] > 0) BonusCycle((int)Bonuses.Hyperdrive);
            else
            {
                _bonus[(int)Bonuses.Hyperdrive].gameObject.SetActive(false);

                for (int i = 0; i < TimeOfBonus.Length - 1; i++)
                {
                    if (TimeOfBonus[i] > 0) BonusCycle(i);
                    else _bonus[i].gameObject.SetActive(false);
                }
            }
        }
    }
    private void BonusCycle(int id)
    {
        TimeOfBonus[id]--;
        _bonusIndicators[id].fillAmount = TimeOfBonus[id] / ShopOfBonuses.timeOfBonus[id];
        if (_enableAnimates[id] && TimeOfBonus[id] < 3)
        {
            _bonus[id].Animate(true);
            _enableAnimates[id] = false;
        }
    }
}
