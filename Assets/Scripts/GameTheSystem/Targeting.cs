using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MorningCache,IFixedRun
{
    private Transform _target;
    private Transform _transform;

    public void FixedRun() => Rotation();

    private void Start()
    {
        _transform = GetComponent<Transform>();
        _target = PlayerInteractive.instance.Transform;
    }
    private void Rotation()
    {
        var dx = _transform.position.x - _target.position.x;
        var dy = _transform.position.y - _target.position.y;
        var angle = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
        _transform.rotation = Quaternion.RotateTowards(_transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle - 90)), Time.deltaTime * 200);
    }
}
