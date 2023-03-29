using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinusoidEnemyLazer : SimpleEnemyLazer
{
    [SerializeField] private float _frequency = 20f;
    [SerializeField] private float _magnitude = 0.5f;
    private float _timer;
    private void Update()
    {
        _timer += Time.deltaTime;
        var direction = new Vector3(Mathf.Cos(_timer * _frequency) * _magnitude, b_direction.y);
        b_transform.position = Vector3.MoveTowards(b_transform.position,b_transform.position + direction, b_speed * Time.deltaTime);
    }
}
