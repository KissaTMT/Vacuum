using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStats : MonoBehaviour
{
    public static GameStats instance;
    public float Score => _score;

    [SerializeField] private BonusesManager _stats;
    [SerializeField] private GameObject _boomText, _blackOn, _pauseListener;
    [SerializeField] private Text _scoreText, _crystalText, _snowflakeText;

    private Player _player;
    private float _score;
    private bool _isLoadingScene = false;

    public void Initialize()
    {
        instance = this;
        _player = Player.instance;
        _player.GetBonus += AddPoint;
        _player.DeathPlayer += Finish;
    }
    private void OnDisable()
    {
        _player.GetBonus -= AddPoint;
        _player.DeathPlayer -= Finish;
        SceneDataSaver.SaveInt(nameof(_score), (int)_score);
    }

    private void Start()
    {
        _score = SceneDataSaver.LoadInt(nameof(_score), 0);
        ShowCount(_crystalText, MenuStats.countCrystal);
        ShowCount(_snowflakeText, MenuStats.countSnowflake);
        ShowCount(_scoreText, (int)_score);
        StartCoroutine(ManageScoreRoutine());
    }
    private IEnumerator ManageScoreRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_stats.TimeOfBonus[(int)Bonuses.DoubleScore] > 0 ? 0.2f : 0.4f);
            _score++;
            ShowCount(_scoreText, (int)_score);
        }
    }
    private void AddPoint(Drop drop)
    {
        if (drop is Crystal)
        {
            MenuStats.countCrystal += (_stats.TimeOfBonus[(int)Bonuses.DoubleCrystal]) > 0 ? 2 : 1;
            ShowCount(_crystalText, MenuStats.countCrystal);
        }
        if (drop is Snowflake)
        {
            MenuStats.countSnowflake++;
            ShowCount(_snowflakeText, MenuStats.countSnowflake);
        }
    }
    private void ShowCount(Text text, int point) => text.text = point.ToString();
    private void Finish()
    {
        SaveStats();
        StartCoroutine(ReloadRoutine());
    }
    private void SaveStats()
    {
        if (_isLoadingScene == false)
        {
            _isLoadingScene = true;
            _pauseListener.SetActive(false);
            Time.timeScale = 1;
            PlayerPrefs.SetInt("countCoin", MenuStats.countCrystal);
            PlayerPrefs.SetInt("countSnowflake", MenuStats.countSnowflake);
            MenuStats.bestScore = Mathf.Max(MenuStats.bestScore, (int)_score);
            PlayerPrefs.SetInt("bestScore", MenuStats.bestScore);
        }
    }
    private IEnumerator ReloadRoutine()
    {
        var loadMenuScene = SceneManager.LoadSceneAsync(0);
        loadMenuScene.allowSceneActivation = false;
        _blackOn.SetActive(true);
        _boomText.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        _isLoadingScene = false;
        loadMenuScene.allowSceneActivation = true;
    }
}
