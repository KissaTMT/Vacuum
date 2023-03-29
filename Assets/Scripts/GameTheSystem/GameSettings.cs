using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSettings : Settings
{
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private SafeArea _left;
    [SerializeField] private PauseListener _pauseListener;
    [SerializeField] private RectTransform _upUILeftRect, _downUILeftRect;

    public void IsPause(bool flag = false)
    {
        _pausePanel.SetActive(flag);
        Time.timeScale = flag ? 0 : 1;
        if (flag) CheckBonusUI();
        UpdateUI();
    }
    public void CheckBonusUI()
    {
        if (BonusesManager.instance.IsBonus)
        {
            _upUILeftRect.anchoredPosition = new Vector2(0, -110);
            _downUILeftRect.anchoredPosition = new Vector2(0, -110);
        }
        else
        {
            _upUILeftRect.anchoredPosition = new Vector3(0, -60, 0);
            _downUILeftRect.anchoredPosition = new Vector3(0, -60, 0);
        }
    }
    public void LeftSafeAreaActivate(bool flag)
    {
        _left.ToSafeArea(flag);
        UpdateUI();
    }
    public void BackToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    protected override void UpdateUI()
    {
        base.UpdateUI();
        b_upUIRightRect.gameObject.SetActive(DataSaver.LoadBool(b_right.gameObject.name) && _pausePanel.activeSelf);
        b_downUIRightRect.gameObject.SetActive(DataSaver.LoadBool(b_right.gameObject.name) == false && _pausePanel.activeSelf);
        _upUILeftRect.gameObject.SetActive(DataSaver.LoadBool(_left.gameObject.name) && _pausePanel.activeSelf);
        _downUILeftRect.gameObject.SetActive(DataSaver.LoadBool(_left.gameObject.name) == false && _pausePanel.activeSelf);
    }

    private void OnEnable() => _pauseListener.PointHandler += IsPause;
    private void OnDisable()=> _pauseListener.PointHandler -= IsPause;
}
