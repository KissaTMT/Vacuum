using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorningCacheInstaller : MonoBehaviour
{
    private IMorningCache _cache;

    private void Awake()
    {
        _cache = GetComponent<IMorningCache>();
        if (_cache is IRun run) GlobalUpdate.runs.Add(run);
        if (_cache is IFixedRun fixedRun) GlobalUpdate.fixedRuns.Add(fixedRun);
    }
    private void OnEnable() => _cache.IsActive = true;
    private void OnDisable() => _cache.IsActive = false;
    private void OnDestroy()
    {
        if (_cache is IRun run) GlobalUpdate.runs.Remove(run);
        if (_cache is IFixedRun fixedRun) GlobalUpdate.fixedRuns.Remove(fixedRun);
    }
}
