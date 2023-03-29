using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSettings : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel, _musicOn, _musicOff; 
    public void IsPause(bool flag = false)
    {
        _pausePanel.SetActive(flag);
        Time.timeScale = flag ? 0 : 1;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void ManageMusic(bool conditional)
    {
        Music.instance.SwitchMusic(conditional);
    }
}
