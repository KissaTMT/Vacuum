using System.Collections;
using UnityEngine;

public class Explosion : PoolObject
{
    [SerializeField] private ParticleSystem[] _particleSystems;
    [SerializeField] private int _seconds;

    private WaitForSeconds _wait;
    private int _size;

    protected override void Awake()
    {
        base.Awake();
        _wait = new WaitForSeconds(_seconds);
        _size = _particleSystems.Length;
    }
    private void OnEnable() => StartCoroutine(GenerateExplosion());
    private IEnumerator GenerateExplosion()
    {
        for (var i = 0; i < _size; i++)
        {
            yield return null;
            _particleSystems[i].Play();
            yield return null;
            yield return null;
        }

        yield return _wait;
        gameObject.SetActive(false);
    }
}
