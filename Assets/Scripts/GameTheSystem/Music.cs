﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Music : MonoBehaviour
{
    private const string ENABLE_MUSIC_KEY = "enableMusic";
    private const string IS_8D_AUdIO_KEY = "is8dAudio";
    private const string VOLUME_KEY = "volume";
    private const string PITCH_KEY = "pitch";

    private const float UPPER_LIMIT_PAN = 0.75f;
    private const float LOWER_LIMIT_PAN = -0.75f;
    private const float SPEED_PAN = 0.25f;

    public static Music instance;
    public bool EnableMusic { get; private set; }
    public bool Is8dAudio { get; private set; }
    public float Volume { get; private set; }
    public float Pitch { get; private set; }

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _mainClip;

    private List<string> _directories;
    private List<AudioClip> _clips;
    private AudioClip _currentClip;
    private string _currentPath;
    private float _formerVolume;
    public void SetVolume(float power)
    {
        Volume = power;
        SyncUp(true);
        _audioSource.volume = power;
        SwitchMusic(power > 0);
        DataSaver.SaveFloat(VOLUME_KEY, power);
    }
    public void SetPitch(float power)
    {
        Pitch = power;
        _audioSource.pitch = Pitch;
        DataSaver.SaveFloat(PITCH_KEY, Pitch);
    }
    public void SwitchMusic(bool flag)
    {
        EnableMusic = flag;
        SyncUp(false);
        DataSaver.SaveBool(ENABLE_MUSIC_KEY, flag);
        if (flag)
        {
            To8dAudio(Is8dAudio);
            _audioSource.UnPause();
        }
        else _audioSource.Pause();
    }
    public void To8dAudio(bool flag)
    {
        StopCoroutine(Play8DdAudio());
        Is8dAudio = flag;
        DataSaver.SaveBool(IS_8D_AUdIO_KEY, flag);
        if (flag) StartCoroutine(Play8DdAudio());
        else _audioSource.panStereo = 0;
    }
    public void LoadClip(string path)
    {
        if(int.TryParse(path, out int result))
        {
            if (result < _directories.Count)
            {
                _currentPath = _directories[result];
                _audioSource.clip = _clips[result];
                _audioSource.Play();
                ResetPitch();
                DataSaver.SaveString(nameof(_currentPath), _currentPath);
                return;
            }
            else return;
        }
        else
        {
            StartCoroutine(LoadClipRoutine(path));
            return;
        }
    }
    private IEnumerator LoadClipRoutine(string path)
    {
        _currentPath = path;
        yield return StartCoroutine(GetAudioClip(path));
        _audioSource.Play();
        ResetPitch();
        DataSaver.SaveString(nameof(_currentPath), _currentPath);
    }
    public void ResetClip()
    {
        _currentPath = null;
        DataSaver.SaveString(nameof(_currentPath), _currentPath);
        _audioSource.clip = _mainClip;
        _audioSource.Play();
        ResetPitch();
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            _directories = new List<string>();
            _clips = new List<AudioClip>();
            LoadData();
            SetVolume(Volume);
            DontDestroyOnLoad(instance);
        }
        else Destroy(gameObject);
    }
    private void Start() => StartCoroutine(InitializeMusic());
    private IEnumerator InitializeMusic()
    {
        if (_currentPath != null)
        {
            yield return StartCoroutine(GetAllLoadedClips());
            _audioSource.clip = _clips[_clips.IndexOf(_clips.FirstOrDefault(i => i.name == _currentPath))];
        }
        _audioSource.Play();
        ResetPitch();
    }
    private void LoadData()
    {
        EnableMusic = DataSaver.LoadBool(ENABLE_MUSIC_KEY, true);
        Is8dAudio = DataSaver.LoadBool(IS_8D_AUdIO_KEY, true);
        Volume = DataSaver.LoadFloat(VOLUME_KEY, 0.5f);
        Pitch = DataSaver.LoadFloat(PITCH_KEY, 1);
        _currentPath = DataSaver.LoadString(nameof(_currentPath), null);
        _formerVolume = DataSaver.LoadFloat(nameof(_formerVolume));

        var size = DataSaver.LoadInt("directoryCount");
        for (var i = 0; i < size; i++)
        {
            _directories.Add(DataSaver.LoadString("directory " + i.ToString()));
        }
    }
    private IEnumerator GetAllLoadedClips()
    {
        for(var i = 0; i < _directories.Count; i++)
        {
            yield return StartCoroutine(LoadAudioClip(_directories[i]));
            _clips.Add(_currentClip);
        }
    }
    private void SyncUp(bool isVolume)
    {
        if (Volume != 0)
        {
            _formerVolume = Volume;
            DataSaver.SaveFloat(nameof(_formerVolume), _formerVolume);
        }
        if (isVolume)
        {
            if (EnableMusic && Volume == 0) EnableMusic = false;
            if (Volume > 0 && !EnableMusic) EnableMusic = true;
        }
        else
        {
            if (!EnableMusic) Volume = 0;
            if (EnableMusic && Volume == 0)
            {
                Volume = _formerVolume;
                EnableMusic = true;
            }
        }
    }
    private IEnumerator GetAudioClip(string path)
    {
        yield return LoadAudioClip(path);
        if (_currentClip != null)
        {
            _clips.Add(_currentClip);
            _audioSource.clip = _currentClip;
        }
        else _audioSource.clip = _mainClip;
    }
    private IEnumerator LoadAudioClip(string path)
    {
        using (var uwr = UnityWebRequestMultimedia.GetAudioClip("file://" + path, AudioType.MPEG))
        {
            var dlHandler = (DownloadHandlerAudioClip)uwr.downloadHandler;

            dlHandler.streamAudio = true;

            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError) yield break;

            if (dlHandler.isDone)
            {
                _currentClip = dlHandler.audioClip;

                if (_audioSource.clip != null)
                {
                    _currentClip = DownloadHandlerAudioClip.GetContent(uwr);
                    _currentClip.name = path;

                    if (_directories.FirstOrDefault(i => i.Equals(path)) == null)
                    {
                        DataSaver.SaveString("directory " + _directories.Count.ToString(), path);
                        _directories.Add(path);
                        DataSaver.SaveInt("directoryCount", _directories.Count);
                    }
                }
            }
        }
    }
    private void ResetPitch()
    {
        Pitch = DataSaver.LoadFloat(PITCH_KEY, 1);
        SetPitch(Pitch + 0.5f);
        SetPitch(Pitch - 0.5f);
    }
    private IEnumerator Play8DdAudio()
    {
        var limit = Mathf.Abs(LOWER_LIMIT_PAN) + Mathf.Abs(UPPER_LIMIT_PAN);
        _audioSource.panStereo = 0;
        while (EnableMusic && Is8dAudio)
        {
            yield return null;
            _audioSource.panStereo = LOWER_LIMIT_PAN + Mathf.PingPong(Time.unscaledTime * SPEED_PAN, limit);
        }
        _audioSource.panStereo = 0;
    }
}
