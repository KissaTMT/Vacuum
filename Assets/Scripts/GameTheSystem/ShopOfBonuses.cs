using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopOfBonuses : MonoBehaviour
{
    private const int SIZE = 6;

    public readonly static int[] timeOfBonus = new int[SIZE];

    [SerializeField] private MenuStats _stats;
    [SerializeField] private Image[] _indicators = new Image[SIZE];
    [SerializeField] private Text[] _cost = new Text[SIZE];

    private int[] _countOfUpgrades = new int[SIZE];

    private void Start()
    {
        for (int i = 0; i < SIZE; i++)
        {
            timeOfBonus[i] = DataSaver.LoadInt("timeOfBonus" + i.ToString(), 10);
            _indicators[i].fillAmount = DataSaver.LoadFloat($"indicator" + i.ToString());
            _cost[i].text = DataSaver.LoadString("cost" + i.ToString(), "200");
            _countOfUpgrades[i] = DataSaver.LoadInt("countOfUpgrades" + i.ToString());
        }
    }
    public void ChangeId(int id)
    {
        if (id != -1 && _countOfUpgrades[id] < 5)
        {
            var price = 200 * _countOfUpgrades[id] + 200;
            if (MenuStats.countCrystal >= price) Purchase(id, price);
        }
    }
    private void Purchase(int id, int price)
    {
        MenuStats.countCrystal -= price;
        DataSaver.SaveInt("countCoin", MenuStats.countCrystal);
        _countOfUpgrades[id]++;
        Upgrade(id);
        IndicatorUpgrade(id);
        DataSaver.SaveInt("countOfUpgrades" + id.ToString(), _countOfUpgrades[id]);
    }

    private void IndicatorUpgrade(int id)
    {
        var power = 2f * _countOfUpgrades[id] / 10f;
        _indicators[id].fillAmount = power;
        _cost[id].text = power == 1 ? "MAX" : (power * 1000 + 200).ToString();

        _stats.UpdateUI();
        DataSaver.SaveFloat("indicator" + id.ToString(), power);
        DataSaver.SaveString("cost" + id.ToString(), _cost[id].text);
    }

    private void Upgrade(int id)
    {
        timeOfBonus[id] = timeOfBonus[id] > 20 ? 20 : timeOfBonus[id];
        timeOfBonus[id] += 2;
        DataSaver.SaveInt("timeOfBonus" + id.ToString(), timeOfBonus[id]);
    }
}