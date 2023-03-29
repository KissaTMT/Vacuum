using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MorningCache,IRun
{
    [SerializeField] private GameObject _boom;

    [SerializeField] private Drop _crystal;

    [SerializeField] private Color[] _color = new Color[3];

    [SerializeField] private bool _isInverse;

    private Transform _transform;
    private Vector3 _direction;
    private float _speed;
    private bool _isAlive;

    public void Run() => _transform.position = Vector3.MoveTowards(_transform.position, _transform.position + _direction, _speed * Time.deltaTime);
    private void Awake()
    {
        _transform = GetComponent<Transform>();
        GetComponent<SpriteRenderer>().color = _color[Random.Range(0,_color.Length)];
    }
    private void Start()
    {
        _speed = Random.Range(5.0f, 8.0f);
        var size = 0.1f / (_speed + 3);
        _isAlive = true;
        _transform.localScale = new Vector2(size, size);

        _direction = Vector3.down;
    }
    private void Death()
    {
        if (_isAlive)
        {
            _isAlive = false;
            Pooling.instance.GetItem(_boom, _transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isInverse) return;
        if(collision.TryGetComponent(out PlayerLazer playerLazer) || collision.TryGetComponent(out PlayerInteractive player))
        {
            if (Random.value > 0.5f)
            {
                var drop = Instantiate(_crystal, _transform.position, Quaternion.identity);
                drop.SetSpeed(_speed);
            }
            Death();
        }
        if (collision.TryGetComponent(out EnemyLazer enemyLazer)) Death();
        if (!_isInverse && collision.TryGetComponent(out Enemy enemy)) Death();
        if (collision.TryGetComponent(out Magnet magnet) && magnet.Mask == LayerMask.NameToLayer("Everything")) _direction = collision.transform.position - _transform.position;
    }
}
