using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PoolObject))]
public class Asteroid : MorningCache,IRun
{
    [SerializeField] private GameObject _boom;

    [SerializeField] private Drop _crystal;

    [SerializeField] private SpriteRenderer _spriteRenderer;

    [SerializeField] private Color[] _color = new Color[3];

    [SerializeField] private bool _isInverse;

    private Vector3 _direction;
    private float _speed;
    private bool _isAlive;

    public void Run() => transform.position = Vector3.MoveTowards(transform.position, transform.position + _direction, _speed * Time.deltaTime);
    private void OnEnable()
    {
        _speed = Random.Range(5.0f, 8.0f);
        var size = 0.1f / (_speed + 3);
        _isAlive = true;
        transform.localScale = new Vector2(size, size);

        _direction = Vector3.down;
        _spriteRenderer.color = _color[Random.Range(0, _color.Length)];
    }
    private void Death()
    {
        if (_isAlive)
        {
            _isAlive = false;
            ObjectPooling.instance.GetItem(_boom, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isInverse) return;
        if(collision.TryGetComponent(out PlayerLazer playerLazer) || collision.TryGetComponent(out Player player))
        {
            if (Random.value > 0.5f)
            {
                var drop = Instantiate(_crystal, transform.position, Quaternion.identity);
                drop.SetSpeed(_speed);
            }
            Death();
        }
        if (collision.TryGetComponent(out EnemyLazer enemyLazer)) Death();
        if (!_isInverse && collision.TryGetComponent(out Enemy enemy)) Death();
        if (collision.TryGetComponent(out Magnet magnet) && magnet.Mask == LayerMask.NameToLayer("Everything")) _direction = collision.transform.position - transform.position;
    }
}
