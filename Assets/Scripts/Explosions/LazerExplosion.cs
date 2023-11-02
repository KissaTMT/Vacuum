using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerExplosion : Explosion
{
    [SerializeField] private Color[] _colors;

    protected override void OnEnable()
    {
        SetColor();
        base.OnEnable();
    }
    private void SetColor()
    {
        for(var i = 0; i < size; i++)
        {
            particleSystems[i].startColor = _colors[ChangerLocation.instance ? ChangerLocation.instance.CurrentLocation : 0];
        }
    }
}
