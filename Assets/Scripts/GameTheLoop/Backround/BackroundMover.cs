using UnityEngine;

public class BackroundMover : MorningCache, IRun
{
    [SerializeField] private float _speed, _repeat;

    private Vector3 _startPosition;
    private Transform _transform;

    public void Run() => _transform.position = _startPosition + Vector3.down * Mathf.Repeat(Time.time * _speed, _repeat);
    private void Awake() => _transform = GetComponent<Transform>();
    private void Start() => _startPosition = _transform.position;
}