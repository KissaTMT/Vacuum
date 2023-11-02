using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InvertedEnemy : Enemy
{
    public bool IsWave => _isWave;
    [SerializeField] private GameObject[] _guns;
    [SerializeField] private float _timeToRun;
    [SerializeField] private bool _isWave;
    public void WaveRunAway() => StartCoroutine(RunAway());
    public void StopFire()
    {
        if (TryGetComponent(out ManagerOfBossGun manage)) manage.EnableShot = false;
        else
        {
            for (int i = 0; i < _guns.Length; i++)
            {
                _guns[i].SetActive(false);
            }
        }
    }
    private void Death()
    {
        if (!_isWave)
        {
            StopFire();
            StartCoroutine(RunAway());
        }
    }
    private IEnumerator RunAway()
    {
        yield return new WaitForSeconds(_timeToRun);
        b_targetPosition = new Vector3(b_transform.position.x, 15);
        for (var i = 0f; i < 1; i += Time.deltaTime / 3)
        {
            b_transform.position = Vector2.Lerp(b_transform.position, b_targetPosition, i * i * i * i);
            yield return null;
        }
        Destroy(gameObject);
    }
    private void GetDamage()
    {
        b_health--;
        if (b_health == b_maxHealth - 1) ChangeStateShield?.Invoke(true);
        if (b_health == 0) Death();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerLazer playerLazer)) GetDamage();
    }
}
