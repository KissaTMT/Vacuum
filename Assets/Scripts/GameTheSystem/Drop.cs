using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Drop : MonoBehaviour
{
    public Transform Transform => _transform;
    public bool IsInverse;
    [SerializeField] private float _speed;
    private Transform _transform;
    private Vector3 _baseDirection;
    private Vector3 _currentDirection;

    public void SetSpeed(float speed) => _speed = speed;
    protected virtual void Initialize() { }
    private void Awake() => _transform = GetComponent<Transform>();
    private void OnDisable()
    {
        StopCoroutine(Flight());
        StopCoroutine(nameof(Fall));
    }
    private void Start()
    {
        _baseDirection = new Vector3(_transform.position.x, -8);
        _currentDirection = _baseDirection;
        Initialize();
        StartCoroutine(Flight());
    }

    private IEnumerator Flight()
    {
        while (Vector2.Distance(_transform.position, _currentDirection) > float.Epsilon)
        {
            _transform.position = Vector2.MoveTowards(_transform.position, _currentDirection, Time.deltaTime * _speed);
            yield return null;
        }
    }
    private IEnumerator Fall(Transform target)
    {
        while (Vector2.Distance(_transform.position, target.position) > float.Epsilon)
        {
            _currentDirection = target.position;
            if (!target && _currentDirection != _baseDirection) _currentDirection = _baseDirection;
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsInverse && collision.TryGetComponent(out Enemy enemy)) Destroy(gameObject);
        if (collision.TryGetComponent(out Magnet magnet))
        {
            StartCoroutine(Fall(PlayerInteractive.instance ? PlayerInteractive.instance.Transform : _transform));
        }
    }
}
