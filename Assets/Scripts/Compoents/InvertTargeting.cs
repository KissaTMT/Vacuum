using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertTargeting : MorningCache,IRun
{
    [SerializeField] private Rodar _rodar;
    [SerializeField] private GameObject _gun;
    private Transform _target;

    public void Run() => transform.position = new Vector2(Mathf.Clamp(transform.position.x, -2f, 2f), transform.position.y);
    private void OnEnable()
    {
        _rodar.Ternsite += Ternsite;
    }

    private void OnDisable()
    {
        _rodar.Ternsite -= Ternsite;
    }
    private void Start()
    {
        _target = Player.instance?.Transform;
        StartCoroutine(Movement());
    }
    private IEnumerator Movement()
    {
        while (_target != null)
        {
            var actualTarget = new Vector3(_target.position.x, transform.position.y);

            transform.position = Vector2.Lerp(transform.position, actualTarget,Time.deltaTime);

            yield return null;
        }
    }

    private void Ternsite(Vector2 targetPosition)
    {
        _target = null;

        transform.position = new Vector2(targetPosition.x, transform.position.y);

        StartCoroutine(GunOff());
    }
    private IEnumerator GunOff()
    {
        yield return new WaitForSeconds(0.5f);
        _gun.SetActive(false);
    }
}
