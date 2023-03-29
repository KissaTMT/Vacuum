using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InvertedBurst : MonoBehaviour
{
    [SerializeField] private LazerBase _base;
    [SerializeField] private GameObject[] _lazers;
    [SerializeField] private GameObject _lazer,_boom;
    private Transform _transform;

    private void Start()
    {
        var countShots = ((_base.Gun.CountShoots) % 5) % 5;
        var posY = new float[5] { 0, -4, -3, -2, -1 };
        _transform = GetComponent<Transform>();
        _transform.position = new Vector2(_transform.position.x,posY[countShots]);
        StartCoroutine(ManageInverseLazers());
    }
    private IEnumerator ManageInverseLazers()
    {
        yield return new WaitUntil(() => _lazers.Any(i => Mathf.Abs(i.transform.position.x - _transform.position.x) < 0.1f && Mathf.Abs(i.transform.position.y - _transform.position.y) < 0.1));
        {
            Pooling.instance.GetItem(_boom, _transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
            _lazer.SetActive(true);
            for (var i = 0; i < _lazers.Length; i++)
            {
                Destroy(_lazers[i]);
            }
        }
        yield return new WaitUntil(() => _lazer == null);
        Destroy(gameObject);
    }

}
