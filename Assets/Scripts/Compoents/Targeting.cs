using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MorningCache,IFixedRun
{
    private Transform _target;

    public void FixedRun() => Rotation();

    private void Start()
    {
        _target = Player.instance.Transform;
    }
    private void Rotation()
    {
        var dx = transform.position.x - _target.position.x;
        var dy = transform.position.y - _target.position.y;
        var angle = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle - 90)), Time.deltaTime * 200);
    }
}
