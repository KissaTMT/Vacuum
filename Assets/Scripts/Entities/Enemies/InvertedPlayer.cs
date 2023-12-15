using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class InvertedPlayer : Enemy
{
    public bool IsInverse;
    [SerializeField] private GameObject[] _sprites;
    [SerializeField] private PlayerGun _gun;
    [SerializeField] private Rodar _rodar;
    [SerializeField] private float _barier;

    private Vector2 _targetPosition;
    private void OnEnable()
    {
        _rodar.CatchLazer += (position) => _targetPosition = position;
    }
    private void OnDisable()
    {
        _rodar.CatchLazer -= (position) => _targetPosition = position;
    }


    private void Start()
    {
        _sprites[ChangerLocation.instance.CurrentLocation].SetActive(true);
        _targetPosition = new Vector2(b_transform.position.x, _barier);
        _gun.SetLazer(ChangerLocation.instance.CurrentLocation);
        StartCoroutine(Flight());
        if (IsInverse) StartCoroutine(RunAway());
    }
    private IEnumerator Flight()
    {
        while (true)
        {
            if (Mathf.Abs(b_transform.position.y - _barier) < 0.05f)
            {
                var actualTarget = new Vector2(_targetPosition.x, b_transform.position.y);
                _gun.EnableShoot = true;
                b_transform.position = Vector2.Lerp(b_transform.position, actualTarget, 20 * Time.deltaTime);
            }
            else b_transform.position = Vector2.Lerp(b_transform.position, _targetPosition, b_speed * Time.deltaTime);
            yield return null;
        }
    }

    private void Death() => Player.instance.Death();
    private IEnumerator RunAway()
    {
        yield return new WaitForSeconds(10);
        StopCoroutine(Flight());
        _gun.gameObject.SetActive(false);
        _targetPosition = new Vector3(b_transform.position.x, -8);
        yield return new WaitForSeconds(2);
        for (var i = 0f; i < 1; i += Time.deltaTime/3)
        {
            yield return null;
            b_transform.position = Vector2.Lerp(b_transform.position, _targetPosition, i * i);
        }
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerLazer lazer) || collision.TryGetComponent(out Player player)) Death();

        if (collision.TryGetComponent(out EventHorizon eventHorizon)) Destroy(gameObject);
    }
}
