using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopRotationFixedUpdate : MorningCache,IFixedRun
{
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private bool _isInverse;
    private Transform _transform;
    public void FixedRun() => _transform.Rotate(0, 0, (_isInverse ? _rotateSpeed : -_rotateSpeed) * Time.deltaTime);
    private void Awake() => _transform = GetComponent<Transform>();
}
