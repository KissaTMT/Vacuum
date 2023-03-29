using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class InvertedPlayer : MonoBehaviour
{
    public bool IsInverse;
    [SerializeField] private GameObject[] _sprites;
    [SerializeField] private PlayerGun _gun;
    [SerializeField] private int _heath;
    [SerializeField] private float _barier, _speed;

    private Transform _transform;
    private Vector2 _targetPosition;

    private void Awake() => _transform = GetComponent<Transform>();

    private void Start()
    {
        _sprites[ChangerLocation.instance.CurrentLocation].SetActive(true);
        _targetPosition = new Vector2(_transform.position.x, _barier);
        _gun.SetLazer(ChangerLocation.instance.CurrentLocation);
        StartCoroutine(Flight());
        if (IsInverse) StartCoroutine(RunAway());
    }
    private IEnumerator Flight()
    {
        while (true)
        {
            if (Mathf.Abs(_transform.position.y - _barier) < 0.05f)
            {
                var actualTarget = new Vector2(Rodar.targetPosition.x, _transform.position.y);
                _gun.EnableShoot = true;
                _transform.position = Vector2.Lerp(_transform.position, actualTarget, 20 * Time.deltaTime);
            }
            else _transform.position = Vector2.Lerp(_transform.position, _targetPosition, _speed * Time.deltaTime);
            yield return null;
        }
    }

    private void Death() => PlayerInteractive.instance.OnDeath?.Invoke();
    private IEnumerator RunAway()
    {
        yield return new WaitForSeconds(10);
        StopCoroutine(Flight());
        _gun.gameObject.SetActive(false);
        _targetPosition = new Vector3(_transform.position.x, -8);
        yield return new WaitForSeconds(2);
        for (var i = 0f; i < 1; i += Time.deltaTime/3)
        {
            yield return null;
            _transform.position = Vector2.Lerp(_transform.position, _targetPosition, i * i);
        }
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerLazer lazer) || collision.TryGetComponent(out PlayerInteractive player)) Death();

        if (collision.TryGetComponent(out EventHorizon eventHorizon)) Destroy(gameObject);
    }
}
