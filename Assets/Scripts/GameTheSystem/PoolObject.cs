using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    public Transform Transform => b_transform;

    protected Transform b_transform;

    protected virtual void Awake() => b_transform = GetComponent<Transform>();
    protected virtual void OnDisable() => Pooling.instance.ActiveObjects.Remove(this);
}
