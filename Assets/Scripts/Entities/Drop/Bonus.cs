using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : Drop
{
    public int ID => _id;

    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private GameObject[] _lights;

    private SpriteRenderer _renderer;
    private int _id = -1;
    public void SetID(int id) => _id = id;
    protected override void Initialize()
    {
        _renderer = GetComponent<SpriteRenderer>();
        if (_id == -1) SetID(Random.Range(0, _sprites.Length));
        _renderer.sprite = _sprites[_id];
        _lights[ChangerLocation.instance.CurrentLocation].SetActive(true);
    }
}
