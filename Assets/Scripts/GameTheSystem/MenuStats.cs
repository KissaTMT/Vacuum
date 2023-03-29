using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMods
{
    Hard,
    Test
};

public class MenuStats : MonoBehaviour
{
    public static int countCrystal;
    public static int countSnowflake;
    public static int gameMode;
    public static int bestScore;

    [SerializeField] private Text _countCrystal,_countSnowflake,_bestScore;

    public void UpdateUI()
    {
        _bestScore.text = "Best Score:" + bestScore.ToString();
        _countCrystal.text = countCrystal.ToString();
        _countSnowflake.text = countSnowflake.ToString();
    }
    private void Start()
    {
        countCrystal = DataSaver.LoadInt("countCoin");
        countSnowflake = DataSaver.LoadInt("countSnowflake");
        gameMode = (int)GameMods.Hard;
        SceneDataSaver.Clear();
        Pooling.instance.Clear();
        bestScore = DataSaver.LoadInt("bestScore");
    }
}