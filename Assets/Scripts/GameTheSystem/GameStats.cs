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
    [SerializeField] private PlayerInteractive _player;
    [SerializeField] private Text _scoreText, _crystalText, _snowflakeText;

    private float _score;
    private bool _isLoadingScene = false;

    private void Awake()
    {
        instance = this;
        _score = SceneDataSaver.LoadInt(nameof(_score), 0);
    }
    private void OnEnable()
    {
        _player.OnGetBonus += AddPoint;
        _player.OnDeath += SaveStats;
    }
    private void OnDisable()
    {
        _player.OnGetBonus -= AddPoint;
        _player.OnDeath -= SaveStats;
        SceneDataSaver.SaveInt(nameof(_score), (int)_score);
    }

    private void Start()
    {
        ShowCount(_crystalText, MenuStats.countCrystal);
        ShowCount(_snowflakeText, MenuStats.countSnowflake);
        ShowCount(_scoreText, (int)_score);
        StartCoroutine(ManageScore());
    }
    private IEnumerator ManageScore()
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
            var loadMenuScene = SceneManager.LoadSceneAsync(0);
            loadMenuScene.allowSceneActivation = false;
            StartCoroutine(Reload(loadMenuScene));
        }
    }
    private IEnumerator Reload(AsyncOperation async)
    {
        _blackOn.SetActive(true);
        _boomText.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        _isLoadingScene = false;
        async.allowSceneActivation = true;
    }
}
