using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingBun : MorningCache, IRun
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _speed, _rotationSpeed;
    private Transform _transform;
    public void Run()
    {
        _transform.position = Vector2.Lerp(_transform.position, _target.position, _speed * Time.deltaTime);
        try { Rotation(Bun.singleton.Transform.position); }
        catch { Rotation(new Vector3(0, -10, 0)); }
    }
    private void Start() => _transform = GetComponent<Transform>();
    private void Rotation(Vector3 position)
    {
        float dx = _transform.position.x - position.x;
        float dy = _transform.position.y - position.y;

        float angle = Mathf.Atan2(dx, dy) * Mathf.Rad2Deg;

        _transform.rotation = Quaternion.RotateTowards(_transform.rotation, Quaternion.Euler(new Vector3(0, 0, -angle)), Time.deltaTime * _rotationSpeed);
    }
}
