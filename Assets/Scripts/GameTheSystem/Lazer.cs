using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Lazer : MonoBehaviour
{
    public bool IsEnemy,IsBoss,IsInvert, IsBurst;
    [SerializeField] private GameObject _boom;
    [SerializeField] private bool[] _axes = new bool[4];
    [SerializeField] private float _speed;
    [SerializeField] private bool _isHoming;
    private Transform _transform;
    private Rigidbody2D _rb;
    private Quaternion _quaternionIdentity;
    private Vector2 _targetPosition;
    private void Start()
    {
        _transform = GetComponent<Transform>();
        _rb = GetComponent<Rigidbody2D>();
        _quaternionIdentity = Quaternion.identity;
        if(_isHoming)_targetPosition = GameObject.Find("gunTarget").transform.position;
        else InitializedAxesLazer();
    }
    private void InitializedAxesLazer()
    {
        var axes = new Vector2[] { Vector2.down, Vector2.up, Vector2.right, Vector2.left };
        for (var i = 0; i < axes.Length; i++)
        {
            if (_axes[i]) _rb.velocity += _speed * axes[i];
        }
    }

    private void Update()
    {
        if (_isHoming) _transform.position = Vector3.MoveTowards(_transform.position, _targetPosition, _speed * Time.deltaTime);
    }
    private void Boom()
    {
        Instantiate(_boom,_transform.position,_quaternionIdentity);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Block block) || collision.TryGetComponent(out Lazer lazer) ||collision.TryGetComponent(out Rodar rodar) || 
            collision.TryGetComponent(out EventHorizon eventHorizon) || collision.TryGetComponent(out Hole hole) || 
            collision.TryGetComponent(out Drop bonus) || collision.TryGetComponent(out BackroundObject backround)) return;
        if (collision.TryGetComponent(out InvertedPlayer inverted) && IsInvert) Destroy(gameObject);
        if (IsEnemy)
        {
            if(collision.TryGetComponent(out Enemy enemy))
            {
                if (IsBurst) return;
                else if (IsInvert) Boom();
                else Instantiate(_boom, _transform.position, _quaternionIdentity);
            }
            else if (IsInvert) return;
            else Boom();
        }
        if (!IsEnemy)
        {
            if (collision.TryGetComponent(out PlayerInteractive player)) return;
            else Boom();
        }
    }
}
