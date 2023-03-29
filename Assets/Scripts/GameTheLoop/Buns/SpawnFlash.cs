using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFlash : MonoBehaviour
{
    [SerializeField] private GameObject _flash;
    [SerializeField] private float _delay;
    private Transform _transform;
    private Quaternion _quaternionIdentity;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        _quaternionIdentity = Quaternion.identity;
    }
    private void OnEnable()
    {
        StartCoroutine(Spawning());
    }
    private IEnumerator Spawning()
    {
        var delay = new WaitForSeconds(_delay);
        while (true)
        {
            yield return delay;
            var item = Instantiate(_flash, _transform.position, _quaternionIdentity);
            item.transform.SetParent(_transform);
            yield return null;
        }
    }
}
