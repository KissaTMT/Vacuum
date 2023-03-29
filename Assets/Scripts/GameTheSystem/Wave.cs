using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class Wave : MonoBehaviour
{
    [SerializeField] private GameObject  _gameObject,_boom;
    [SerializeField] private bool _isInversion;

    private List<GameObject> _enemies = new List<GameObject>(4);
    private List<InvertedEnemy> _invertedEnemies = new List<InvertedEnemy>(4);
    private Transform _transform;
    private Quaternion _quaternionIdentity;
    private float[] _positionX = new float[4] { -1.8f, -0.6f, 0.6f, 1.8f };

    private void Start()
    {
        _transform = GetComponent<Transform>();
        _quaternionIdentity = Quaternion.identity;
        if (_isInversion) StartCoroutine(ManageInvertedWaveBad());
        else StartCoroutine(SpawnWaveBad());
    }
    private IEnumerator SpawnWaveBad()
    {
        foreach (var pos in _positionX)
        {
            _enemies.Add(Instantiate(_gameObject, new Vector2(pos, _transform.position.y), _quaternionIdentity));
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitUntil(() => _enemies.All(item => item == null));
        Destroy(gameObject);
    }
    private IEnumerator WaitSpawnWave()
    {
        var isSpawn = new bool[4];
        while (isSpawn.All(item => item == true) == false)
        {
            yield return null;
            for (var i = 0; i < 4; i++)
            {
                if (PlayerInteractive.instance != null)
                {
                    if (!isSpawn[i] && Mathf.Abs(PlayerInteractive.instance.Transform.position.x - _positionX[i]) < 0.4f)
                    {
                        isSpawn[i] = true;
                        yield return StartCoroutine(InvertedSpawn(_positionX[i]));
                    }
                }
            }
        }
    }

    private IEnumerator InvertedSpawn(float posX)
    {
        Instantiate(_boom, new Vector2(posX, _transform.position.y), _quaternionIdentity);
        yield return new WaitForSeconds(1);
        _invertedEnemies.Add(Instantiate(_gameObject, new Vector2(posX, _transform.position.y), _quaternionIdentity).GetComponent<InvertedEnemy>());
    }

    private IEnumerator ManageInvertedWaveBad()
    {
        yield return StartCoroutine(WaitSpawnWave());

        yield return new WaitUntil(() => _invertedEnemies.All(item => item.Health <= 0));

        var sortedList = _invertedEnemies.OrderByDescending(enemy => enemy.Transform.position.x);

        foreach (var enemy in sortedList)
        {
            enemy.StopFire();
        }

        foreach(var enemy in sortedList)
        {
            enemy.WaveRunAway();
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitUntil(() => _invertedEnemies.All(item => item == null));

        Destroy(gameObject);
    }
}