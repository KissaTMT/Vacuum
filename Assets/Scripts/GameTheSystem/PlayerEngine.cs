using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEngine : MonoBehaviour
{
    [SerializeField] private Color[] _colors;
    private ParticleSystem _particleSystem;

    public void SetColor(int id) => _particleSystem.startColor = _colors[id];
    private void Awake() => _particleSystem = GetComponent<ParticleSystem>();
}
