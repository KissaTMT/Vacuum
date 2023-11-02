using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerGun : SimpleGun
{
    public UnityAction<Vector2> Shooted;

    [SerializeField] private LazerBase[] _lazersByLocation;
    public void SetLazer(int id) => b_lazerPrefab = _lazersByLocation[id];
    protected override void Shoot(Vector2 direction)
    {
        base.Shoot(direction);
        Shooted?.Invoke(b_transform.position);
    }
}
