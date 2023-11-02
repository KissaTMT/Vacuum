using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopRotationUpdate : MorningCache, IRun
{
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private bool _isInverse;

    public void Run() => transform.Rotate(0, 0, (_isInverse ? _rotateSpeed : -_rotateSpeed) * Time.deltaTime);

}
