using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MorningCache, IRun
{
    [SerializeField] private Sprite[] _spites;
    [SerializeField] private float _speed;

    private SpriteRenderer _renderer;

    public void Run() => transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.down, _speed * Time.deltaTime);

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        if (ChangerLocation.instance != null) _renderer.sprite = _spites[ChangerLocation.instance.CurrentLocation];
        var size = Random.Range(0.5f, 1f);
        transform.localScale = new Vector2(transform.localScale.x * size, transform.localScale.y * size);
    }
}
