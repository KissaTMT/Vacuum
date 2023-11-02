using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : SimpleEnemyLazer,IRun
{
    public bool EnableTargeting { get; private set; }
    [SerializeField] private Transform _target;
    [SerializeField] private float _rotationSpeed;

    public new void Run()
    {
        transform.position = Vector2.MoveTowards(transform.position, _target.position, b_speed * Time.deltaTime);
        if (EnableTargeting) Rotation(Player.instance.Transform.position);
    }
    protected override void ReactOnMagnet(Collider2D collision)
    {
        if (collision.TryGetComponent(out Magnet magnet) && magnet.Mask == LayerMask.NameToLayer("Everything")) _target = collision.transform;
    }
    private void Start() => StartCoroutine(DelayToTargteting());
    private IEnumerator DelayToTargteting() 
    {
        yield return new WaitForSeconds(0.3f);
        EnableTargeting = true;
    }
    private void Rotation(Vector3 position)
    {
        var dx = transform.position.x - position.x;
        var dy = transform.position.y - position.y;
        var angle = Mathf.Atan2(dx, dy) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0, 0, -angle)), Time.deltaTime * _rotationSpeed);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out LazerBase lazer) || collision.TryGetComponent(out Player player)) Death();
        if (collision.TryGetComponent(out Enemy enemy))
        {
            if (EnableTargeting) Death();
            else Boom();
        }
        ReactOnMagnet(collision);
    }
}
