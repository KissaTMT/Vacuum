using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSettings : Settings
{
    private const int GAME_SCENE_ID = 1;

    [SerializeField] private GameObject _shopPanel;
    [SerializeField] private MenuStats _stats;

    private AsyncOperation _loadGame;
    public void Play() => _loadGame.allowSceneActivation = true;
    public void Inversion() => SceneManager.LoadScene(2);
    public void Exit() => Application.Quit();
    public void ShopPanel(bool flag) => _shopPanel.SetActive(flag);
    public void TestMode() => MenuStats.gameMode = 1;
    protected override void Start()
    {
        base.Start();
        _loadGame = SceneManager.LoadSceneAsync(GAME_SCENE_ID);
        _loadGame.allowSceneActivation = false;
        _stats.UpdateUI();
    }
    protected override void UpdateUI()
    {
        base.UpdateUI();
        b_upUIRightRect.gameObject.SetActive(DataSaver.LoadBool(b_right.gameObject.name) ? true : false);
        b_downUIRightRect.gameObject.SetActive(DataSaver.LoadBool(b_right.gameObject.name) ? false : true);
    }
}
