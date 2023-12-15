using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorningCacheInstaller : MonoBehaviour
{
    private IMorningCache _cache;
    private bool _initialized;

    public void Initialize()
    {
        _cache = GetComponent<IMorningCache>();
        Add();
        SetActive(gameObject.activeInHierarchy);
        _initialized = true;
    }
    private void Start() => Initialize();
    private void OnEnable()
    {
        if (_initialized) SetActive(true);
    }
    private void OnDisable()
    {
        if (_initialized) SetActive(false);
    }
    private void SetActive(bool active)
    {
        _cache.IsActive = active;
        //for (var i = 0; i < _cache.Length; i++)
        //{
        //    _cache[i].IsActive = active;
        //}
    }
    private void Add()
    {
        GlobalUpdate.instance.Add(_cache);
        //for (var i = 0; i < _cache.Length; i++)
        //{
        //    GlobalUpdate.instance.Add(_cache[i]);
        //}
    }
    private void Remove()
    {
        GlobalUpdate.instance.Remove(_cache);
        //for (var i = 0; i < _cache.Length; i++)
        //{
        //    GlobalUpdate.instance.Remove(_cache[i]);
        //}
    }
    private void OnDestroy() => Remove();
}
