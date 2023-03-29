using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLazerWithTarget : EnemyLazer,IRun
{
    public new void Run() => b_transform.position = Vector2.MoveTowards(b_transform.position, b_direction, b_speed * Time.deltaTime);
    protected override void ReactOnMagnet(Collider2D collision)
    {
        if (collision.TryGetComponent(out Magnet magnet) && magnet.Mask == LayerMask.NameToLayer("Everything")) b_direction = collision.transform.position;
    }
    private void Start() { if (b_isFromGun) Boom(); }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerInteractive player) || collision.TryGetComponent(out Asteroid asteroid) || collision.TryGetComponent(out Rocket rocket)) Death();
        ReactOnMagnet(collision);
    }
}
