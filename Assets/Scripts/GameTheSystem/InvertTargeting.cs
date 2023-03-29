using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertTargeting : MorningCache,IRun
{
    [SerializeField] private Rodar _rodar;
    [SerializeField] private GameObject _gun;
    private Transform _transform;
    private Transform _target;

    public void Run() => _transform.position = new Vector2(Mathf.Clamp(_transform.position.x, -2f, 2f), _transform.position.y);
    private void OnEnable()
    {
        _rodar.Ternsite += Ternsite;
    }

    private void OnDisable()
    {
        _rodar.Ternsite -= Ternsite;
    }
    private void Awake() => _transform = GetComponent<Transform>();
    private void Start()
    {
        _transform = GetComponent<Transform>();
        _target = PlayerInteractive.instance?.Transform;
        StartCoroutine(Movement());
    }
    private IEnumerator Movement()
    {
        while (_target != null)
        {
            var actualTarget = new Vector3(_target.position.x, _transform.position.y);

            _transform.position = Vector2.Lerp(_transform.position, actualTarget,Time.deltaTime);

            yield return null;
        }
    }

    private void Ternsite(Vector2 targetPosition)
    {
        _target = null;

        _transform.position = new Vector2(targetPosition.x, _transform.position.y);

        StartCoroutine(GunOff());
    }
    private IEnumerator GunOff()
    {
        yield return new WaitForSeconds(0.5f);
        _gun.SetActive(false);
    }
}
