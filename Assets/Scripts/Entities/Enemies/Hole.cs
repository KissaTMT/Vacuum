using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Hole : MonoBehaviour
{
    [SerializeField] private Sprite[] _spites;
    [SerializeField] private float _speed;

    private Transform _transform;
    private SpriteRenderer _renderer;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }
    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        if (ChangerLocation.instance != null) _renderer.sprite = _spites[ChangerLocation.instance.CurrentLocation];
        var size = Random.Range(0.5f, 1f);
        transform.localScale = new Vector2(transform.localScale.x * size, transform.localScale.y * size);
        StartCoroutine(Flight());
    }
    private IEnumerator Flight()
    {
        while (_transform.position.y > -11)
        {
            _transform.position += Vector3.down * _speed * Time.deltaTime;
            yield return null;
        }
    }
}
