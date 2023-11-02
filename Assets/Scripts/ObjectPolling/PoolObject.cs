using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    public Transform Transform => b_transform;

    protected Transform b_transform;

    private void Awake() => b_transform = GetComponent<Transform>();
    private void OnDisable() => ObjectPooling.instance.ActiveObjects.Remove(this);
}
