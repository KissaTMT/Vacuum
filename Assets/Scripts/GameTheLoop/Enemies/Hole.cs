using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MorningCache, IRun
{
    [SerializeField] private Sprite[] _spites;
    [SerializeField] private float _speed;

    private Transform _transform;
    private SpriteRenderer _renderer;

    public void Run() => _transform.position = Vector3.MoveTowards(_transform.position, _transform.position + Vector3.down, _speed * Time.deltaTime);

    private void Start()
    {
        _transform = GetComponent<Transform>();
        _renderer = GetComponent<SpriteRenderer>();
        if (ChangerLocation.instance != null) _renderer.sprite = _spites[ChangerLocation.instance.CurrentLocation];
        var size = Random.Range(0.5f, 1f);
        _transform.localScale = new Vector2(_transform.localScale.x * size, _transform.localScale.y * size);
    }
}
