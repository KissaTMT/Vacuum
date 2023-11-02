using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingBun : MorningCache, IRun
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _speed, _rotationSpeed;
    public void Run()
    {
        transform.position = Vector2.Lerp(transform.position, _target.position, _speed * Time.deltaTime);
        try { Rotation(Bun.singleton.Transform.position); }
        catch { Rotation(new Vector3(0, -10, 0)); }
    }
    private void Rotation(Vector3 position)
    {
        float dx = transform.position.x - position.x;
        float dy = transform.position.y - position.y;

        float angle = Mathf.Atan2(dx, dy) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0, 0, -angle)), Time.deltaTime * _rotationSpeed);
    }
}
