using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BurstLazer : MonoBehaviour
{
    [SerializeField] private LazerBase _base;
    [SerializeField] private GameObject[] _lazers;
    [SerializeField] private GameObject _boom;

    private Transform _transform;
    private Quaternion _quaternionIdenity;
    private float[] _posY = new float[5] { -4 , 0, -1,-2,-3 };
    private int _countShots;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        _quaternionIdenity = Quaternion.identity;
        _countShots = ((_base.Gun.CountShoots) % 5) % 5;
        StartCoroutine(Flight());
    }
    private IEnumerator Flight()
    {
        yield return new WaitUntil(()=>Mathf.Abs(_transform.position.y - _posY[_countShots]) < 0.1f);
        Burst();
    }
    private void Burst()
    {
        ObjectPooling.instance.GetItem(_boom, _transform.position, _quaternionIdenity);
        for (int i = 0; i < _lazers.Length; i++)
        {
            Instantiate(_lazers[i], _transform.position, _quaternionIdenity);
        }
        Destroy(gameObject);
    }
}
