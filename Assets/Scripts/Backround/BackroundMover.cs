using UnityEngine;

public class BackroundMover : MorningCache, IRun
{
    [SerializeField] private float _speed, _repeat;

    private Vector3 _startPosition;

    public void Run() => transform.position = _startPosition + Vector3.down * Mathf.Repeat(Time.time * _speed, _repeat);
    private void Start() => _startPosition = transform.position;
}