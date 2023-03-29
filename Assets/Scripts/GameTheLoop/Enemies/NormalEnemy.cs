using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : Enemy
{
    [SerializeField] private GameObject _boom, _crystal, _bonus;
    [SerializeField] private float _barier;

    private bool _isAlive;

    private void Start()
    {
        _isAlive = true;
        b_targetPosition = new Vector3(b_transform.position.x, _barier, 0);
        StartCoroutine(Flight(b_targetPosition));
    }
    private IEnumerator Flight(Vector3 target)
    {
        while (Vector2.Distance(b_transform.position, target) > float.Epsilon)
        {
            b_transform.position = Vector2.Lerp(b_transform.position, target, Time.deltaTime * b_speed * 3);
            yield return null;
        }
    }
    private IEnumerator Fall(Vector3 target)
    {
        while (Vector2.Distance(b_transform.position, target) > float.Epsilon)
        {
            b_transform.position = Vector2.MoveTowards(b_transform.position, target, Time.deltaTime * b_speed * 3);
            yield return null;
        }
    }
    private void Death()
    {
        if (_isAlive)
        {
            _isAlive = false;
            Pooling.instance.GetItem(_boom, b_transform.position, b_quaternionIdentity);
            if (b_maxHealth > 15) Instantiate(_bonus, b_transform.position, b_quaternionIdentity);
            else
            {
                if (Random.value > 0.5f) Instantiate(_bonus, b_transform.position, b_quaternionIdentity);
                else Instantiate(_crystal, b_transform.position, b_quaternionIdentity);
            }
            Destroy(gameObject);
        }
    }
    private void GetDamage()
    {
        b_health--;
        if (b_health == 1) ChangeStateShield?.Invoke(false);
        if (b_health == 0) Death();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerLazer playerLazer)) GetDamage();
        if (collision.TryGetComponent(out Rocket rocket) && rocket.EnableTargeting) GetDamage();
        if (collision.TryGetComponent(out PlayerInteractive player)) Death();

        if (collision.TryGetComponent(out EventHorizon eventHorizon)) Destroy(gameObject);
        if (collision.TryGetComponent(out Magnet magnet) && magnet.Mask == LayerMask.NameToLayer("Everything")) StartCoroutine(Fall(collision.transform.position));
    }
}
