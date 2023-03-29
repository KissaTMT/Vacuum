using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : SimpleGun
{
    [SerializeField] private LazerBase[] _lazersByLocation;
    public void SetLazer(int id) => b_lazerPrefab = _lazersByLocation[id];
}
