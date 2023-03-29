using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bun : MonoBehaviour
{
    public static Bun singleton { get; private set; }
    public Transform Transform { get; private set; }
    private void Awake() => singleton = this;
    private void Start() => Transform = GetComponent<Transform>();
}
