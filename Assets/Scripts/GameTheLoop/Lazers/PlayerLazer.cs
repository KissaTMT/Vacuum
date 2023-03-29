using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLazer : LazerBase
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Enemy enemy) || collision.TryGetComponent(out Asteroid asteroid) || collision.TryGetComponent(out Rocket rocket)) Death();
        ReactOnMagnet(collision);
    }
}
