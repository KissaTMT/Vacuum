using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLazer : LazerBase
{
    [SerializeField] protected bool b_isFromGun;
    [SerializeField] private Vector3 _direction;
    private void OnEnable()
    {
        if (!b_isFromGun) b_direction = _direction;
    }
}
