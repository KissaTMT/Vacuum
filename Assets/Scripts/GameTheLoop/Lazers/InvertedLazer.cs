using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertedLazer : EnemyLazer
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Asteroid asteroid) || collision.TryGetComponent(out PlayerInteractive player)) Boom();
        else if (collision.TryGetComponent(out Enemy enemy)) Death();
        else if (collision.TryGetComponent(out InvertedPlayer invertedPlayer)) Death(false);
    }
}
