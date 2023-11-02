using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MorningCacheInstaller))]
public class MorningCache : MonoBehaviour
{
    protected new Transform transform;
    public bool IsActive { get; set; }
    protected virtual void CacheInit()
    {
        transform = GetComponent<Transform>();
    }
    private void Awake()
    {
        CacheInit();
    }
}
