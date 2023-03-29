using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class Settings : MonoBehaviour
{
    public UnityAction ReturnBack;

    [SerializeField] protected GameObject b_advancedMusic, b_musicOn, b_musicOff;
    [SerializeField] protected InputField b_inputField;
    [SerializeField] protected SafeArea b_right;
    [SerializeField] protected Slider b_volumeSlider, b_pitchingSlider,b_to8Daudio;
    [SerializeField] protected RectTransform b_upUIRightRect, b_downUIRightRect;
    [SerializeField] protected Image b_fillPressed;

    protected Music b_music;

    private bool _buttonPressed;

    public void ResetMusic() => b_music.ResetClip();
    public void LoadMusic()
    {
        var path = b_inputField.text;
        b_inputField.text = string.Empty;
        if (!string.IsNullOrWhiteSpace(path)) b_music.LoadClip(path);
    }
    public void ManageMusic(bool conditional)
    {
        b_music.SwitchMusic(conditional);
        UpdateUI();
    }
    public void RightSafeAreaActivate(bool flag)
    {
        b_right.ToSafeArea(flag);
        UpdateUI();
    }
    public void PressedButtonMusic()
    {
        _buttonPressed = true;
        StartCoroutine(ShowAdvancedMusicSettings());
    }
    public void ResetButtonMusic()
    {
        _buttonPressed = false;
        b_fillPressed.fillAmount = 0;
    }
    public void To8dAudo(float power)
    {
        b_music.To8dAudio(power==1);
        UpdateUI();
    }
    public void SetPitching(float power)
    {
        power /= 10;
        b_music.SetPitch(power);
        UpdateUI();
    }
    public void SetVolume(float power)
    {
        power /= 10;
        b_music.SetVolume(power);
        UpdateUI();
    }
    public void CloseAdvancedMusicSettings() => b_advancedMusic.SetActive(false);
    protected virtual void Start()
    {
        b_music = Music.instance;
        UpdateUI();
    }
    private void OnGUI()
    {
        if (Input.GetKey(KeyCode.Escape)) ReturnBack?.Invoke();
    }
    private IEnumerator ShowAdvancedMusicSettings()
    {
        var timer = 0f;
        UpdateUI();
        b_fillPressed.fillAmount = timer;
        while (_buttonPressed)
        {
            yield return null;
            timer += Time.unscaledDeltaTime;
            b_fillPressed.fillAmount = timer;
            if (timer > 1)
            {
                b_advancedMusic.SetActive(true);
                break;
            }
        }
        b_fillPressed.fillAmount = 0;
    }
    protected virtual void UpdateUI()
    {
        b_volumeSlider.value = b_music.Volume * 10;
        b_pitchingSlider.value = b_music.Pitch * 10;
        b_to8Daudio.value = b_music.Is8dAudio ? 1 : 0;


        b_musicOn.SetActive(b_music.EnableMusic);
        b_musicOff.SetActive(!b_music.EnableMusic);

        b_upUIRightRect.gameObject.SetActive(DataSaver.LoadBool(b_right.gameObject.name) ? true : false);
        b_downUIRightRect.gameObject.SetActive(DataSaver.LoadBool(b_right.gameObject.name) ? false : true);
    }
}
