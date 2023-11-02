using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private GameStats _stats;
    [SerializeField] private BonusesManager _bonusManager;
    [SerializeField] private BlackHoleWay _blackHoleWay;

    private void Awake()
    {
        _player.Initialize();
        _stats.Initialize();
        _bonusManager.Initialize();
        _blackHoleWay.Initialize();
    }
}
