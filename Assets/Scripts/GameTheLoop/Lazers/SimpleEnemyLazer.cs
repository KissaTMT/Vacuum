using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemyLazer : EnemyLazer
{
    private void Start()
    {
        if (b_isFromGun) Boom();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerInteractive player) || collision.TryGetComponent(out Asteroid asteroid) || collision.TryGetComponent(out Rocket rocket)) Death();
        ReactOnMagnet(collision);
    }
}
