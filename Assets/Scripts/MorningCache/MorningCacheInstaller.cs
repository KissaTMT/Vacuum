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
        GlobalUpdate.instance.Add(_cache);
        _cache.IsActive = gameObject.activeInHierarchy;
        _initialized = true;
    }
    private void Start() => Initialize();
    private void OnEnable()
    {
        if(_initialized) _cache.IsActive = true;
    }
    private void OnDisable()
    {
        if (_initialized) _cache.IsActive = false;
    }
    private void OnDestroy() => GlobalUpdate.instance.Remove(_cache);
}
