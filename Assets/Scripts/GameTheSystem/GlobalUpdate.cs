using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalUpdate : MonoBehaviour
{
    public static List<IRun> runs = new List<IRun>(40);
    public static List<IFixedRun> fixedRuns = new List<IFixedRun>(10);

    private void Update()
    {
        for(var i = 0; i < runs.Count; i++)
        {
            if(runs[i].IsActive) runs[i].Run();
        }
    }
    private void FixedUpdate()
    {
        for(var i = 0; i < fixedRuns.Count; i++)
        {
            if (fixedRuns[i].IsActive) fixedRuns[i].FixedRun();
        }
    }
    private void OnDestroy()
    {
        runs?.Clear();
        fixedRuns?.Clear();
    }
}
