using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertedLazer : EnemyLazer
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Asteroid asteroid) || collision.TryGetComponent(out Player player)) Boom();
        else if (collision.TryGetComponent(out InvertedPlayer invertedPlayer)) Death(false);
        else if (collision.TryGetComponent(out InvertedEnemy enemy)) Death();
    }
}
