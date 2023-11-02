using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalUpdate : MonoBehaviour
{

    public static GlobalUpdate instance;

    public List<IRun> _runs = new List<IRun>(40);
    public List<IFixedRun> _fixedRuns = new List<IFixedRun>(10);

    public void Add<T>(T cache) where T : IMorningCache
    {
        if (cache is IRun run) _runs.Add(run);
        if (cache is IFixedRun fixedRun) _fixedRuns.Add(fixedRun);
    }
    public void Remove<T>(T cache) where T : IMorningCache
    {
        if (cache is IRun run) _runs.Remove(run);
        if (cache is IFixedRun fixedRun) _fixedRuns.Remove(fixedRun);
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    private void Update()
    {
        for(var i = 0; i < _runs.Count; i++)
        {
            if (_runs[i].IsActive) _runs[i].Run();
        }
    }
    private void FixedUpdate()
    {
        for(var i = 0; i < _fixedRuns.Count; i++)
        {
            if (_fixedRuns[i].IsActive) _fixedRuns[i].FixedRun();
        }
    }
    private void OnDestroy()
    {
        _runs?.Clear();
        _fixedRuns?.Clear();
    }
}
