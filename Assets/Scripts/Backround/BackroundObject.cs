using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackroundObject : MonoBehaviour
{
    [SerializeField] private float _timeToDestrouBuns;
    private Transform _transform;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        var size = Random.Range(0.5f, 1);
        _transform.localScale = new Vector2(size * _transform.localScale.x, size * _transform.localScale.y);
        StartCoroutine(Boom());
    }
    private IEnumerator Boom()
    {
        yield return new WaitForSeconds(_timeToDestrouBuns);
        Destroy(gameObject); Destroy(gameObject);
    }
}
