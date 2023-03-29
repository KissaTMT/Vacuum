using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : Drop
{
    [SerializeField] private GameObject[] _sprites;

    protected override void Initialize()
    {
        if (BonusesManager.instance.HyperdriveTime > 0) _sprites[_sprites.Length - 1].SetActive(true);
        else _sprites[ChangerLocation.instance.CurrentLocation].SetActive(true);
    }
}
