using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnerInversion : MonoBehaviour
{
    private const string SCORE_TO_BOSS = "scoreToBoss";
    private const float DELAY_QUEUE = 2;
    private const float DELAY_BETWEEN_ASTEROID = 0.75f;
    private bool EnableSpawn => !_isHyperdriveFlight && BlackHoleWay.instance.IsDisactive;

    [SerializeField] private List<SerializedList<GameObject>> _objects;
    [SerializeField] private GameObject _invertedPlayer, _boom, _bigBoom, _ternsiteHole;
    [SerializeField] private Bonus _bonus;
    [SerializeField] private Snowflake _snowflake;

    private ChangerLocation _changer;
    private PlayerGun _mainGun;
    private Quaternion _quaternionIdentity;
    private Transform _transform;
    private GameObject _currentEnemy;
    private bool _enableAsteroidSpawn;
    private bool _enableHyperdrive;
    private bool _isHyperdriveFlight;
    private bool _isRessurection;

    private int _scoreToBoss;
    private int _counterShoots;

    private void Start()
    {
        _scoreToBoss = SceneDataSaver.LoadInt(SCORE_TO_BOSS, 100);
        _changer = ChangerLocation.instance;
        _mainGun = Player.instance.ComponentManager.MainGun;
        _mainGun.Shooted += TryResurrection;
        _transform = GetComponent<Transform>();
        _quaternionIdentity = Quaternion.identity;
        StartCoroutine(SpawnAsteroids());
        StartCoroutine(SpawnQueueBest());
    }

    private void TryResurrection(Vector2 position)
    {
        var probability = Random.value;
        _counterShoots++;
        if (_currentEnemy == null && EnableSpawn)
        {
            if (_counterShoots % 7 == 0 && _counterShoots % 4 != 0 &&  probability > 0.5f && probability < 0.9f && Player.instance.Transform.position.y < -1)
            {
                StartCoroutine(ResurrectionEnemyRoutine(position));
                _isRessurection = true;
                return;
            }
            else if (_counterShoots % 7 == 0 && _counterShoots % 4 != 0 && GameStats.instance.Score >= _scoreToBoss && Mathf.Abs(Player.instance.Transform.position.x) < 1 && Player.instance.Transform.position.y < -0.5f)
            {
                StartCoroutine(ResurrectionBossRoutine());
                _isRessurection = true;
                return;
            }
            else if (_counterShoots % 4 == 0  && _counterShoots % 7 != 0 && _enableAsteroidSpawn)
            {
                StartCoroutine(ResurrectionAsteroidRoutine(position));
                return;
            }
        }
    }

    private void OnDisable()
    {
        _mainGun.Shooted -= TryResurrection;
        SceneDataSaver.SaveInt(SCORE_TO_BOSS, (int)GameStats.instance.Score + _scoreToBoss);
    }
    private IEnumerator SpawnAsteroids()
    {
        var delay = new WaitForSeconds(DELAY_BETWEEN_ASTEROID);
        while (true)
        {
            yield return delay;
            if (_currentEnemy == null && _enableAsteroidSpawn && EnableSpawn) ObjectPooling.instance.GetItem(_objects[_changer.CurrentLocation][0], new Vector2(Random.Range(-2.2f, 2.2f), _transform.position.y),_quaternionIdentity);
        }
    }
    private IEnumerator ResurrectionEnemyRoutine(Vector2 vector)
    {
        _enableAsteroidSpawn = false;

        var position = new Vector2(vector.x, 3);
        var distanceModifier = Mathf.Abs(3-Player.instance.transform.position.y);

        yield return new WaitForSeconds(distanceModifier/8 - 0.5f);

        StartCoroutine(SpawnWithExplosionRoutine(_objects[_changer.CurrentLocation][Random.Range(1, _objects[_changer.CurrentLocation].Count - 1)], _boom, position));
    }
    private IEnumerator ResurrectionBossRoutine()
    {
        _enableAsteroidSpawn = false;

        var position = new Vector2(0, 3.5f);
        var distanceModifier = Mathf.Abs(3.5f - Player.instance.transform.position.y);

        yield return new WaitForSeconds(distanceModifier / 8 - 0.5f);

        StartCoroutine(SpawnWithExplosionRoutine(_objects[_changer.CurrentLocation][_objects[_changer.CurrentLocation].Count - 1], _bigBoom, position));
        _scoreToBoss += Random.Range(150, 250);
    }
    private IEnumerator ResurrectionAsteroidRoutine(Vector2 vector)
    {
        var posX = vector.x;
        var distanceModifier = Random.Range(4f, 6f);
        var posY = Player.instance.Transform.position.y + distanceModifier;
        var position = new Vector2(posX, posY);
        var delay = distanceModifier / 8 - 0.5f;
        yield return new WaitForSeconds(delay);
        InitiateBoom(_boom, position);
        yield return new WaitForSeconds(0.5f);
        ObjectPooling.instance.GetItem(_objects[_changer.CurrentLocation][0], position, _quaternionIdentity);
    }
    private IEnumerator SpawnQueueBest()
    {
        var delay = new WaitForSeconds(DELAY_QUEUE);
        SpawnInvertedPlayer();
        yield return new WaitForSeconds(10);
        while (true)
        {
            yield return delay;
            if (EnableSpawn)
            {
                var probability = Random.value;
                _enableHyperdrive = true;
                if (_currentEnemy == null && !_isRessurection)
                {
                    if (!_enableAsteroidSpawn)
                    {
                        if (probability < 0.3f && _enableHyperdrive) yield return StartCoroutine(HyperdriveFlight());
                        else
                        {
                            _enableAsteroidSpawn = true;
                            _isRessurection = false;
                            yield return new WaitForSeconds(Random.Range(6, 12));
                        }
                    }
                    if (probability > 0.9f)
                    {
                        _enableAsteroidSpawn = false;
                        _currentEnemy = Spawn(_ternsiteHole, Random.Range(-1.6f, 1.6f), 8);
                    }
                }
            }
        }
    }
    private void SpawnInvertedPlayer()
    {
        _enableAsteroidSpawn = false;
        _currentEnemy = Spawn(_invertedPlayer, new Vector3(Random.value > 0.5f ? 1 : -1, -6));
        _currentEnemy.GetComponent<InvertedPlayer>().IsInverse = true;
    }
    private IEnumerator HyperdriveFlight()
    {
        _enableHyperdrive = false;
        _isHyperdriveFlight = true;
        Player.instance.GetBerk.Invoke((int)Bonuses.Hyperdrive);
        Player.instance.ComponentManager.InitiateHyperdriveFlight();
        StartCoroutine(GenerateSnowflake());
        yield return new WaitForSeconds(ShopOfBonuses.timeOfBonus[(int)Bonuses.Hyperdrive]-1);
        var position = new Vector3(Player.instance.Transform.position.x, Player.instance.Transform.position.y);
        var bonus = Spawn(_bonus, position);
        yield return new WaitUntil(() => Mathf.Abs(bonus.Transform.position.y - 3) < 2f);
        position = new Vector3(bonus.Transform.position.x, 3);
        yield return StartCoroutine(SpawnWithExplosionRoutine(_objects[_changer.CurrentLocation][1], _boom, position));
        yield return new WaitUntil(() => _currentEnemy == null);
        _isHyperdriveFlight = false;
    }
    private IEnumerator GenerateSnowflake()
    {
        var delay = new WaitForSeconds(0.5f);
        while (BonusesManager.instance.HyperdriveTime > 4)
        {
            yield return delay;
            Instantiate(_snowflake, new Vector2(Random.Range(-1.6f, 1.6f), _transform.position.y), _quaternionIdentity);
        }
    }
    private IEnumerator SpawnWithExplosionRoutine(GameObject enemyPrefab, GameObject explosionPrefab, Vector2 position)
    {
        if(enemyPrefab.TryGetComponent(out Wave wave) == false) InitiateBoom(explosionPrefab, position);
        yield return new WaitForSeconds(0.5f);
        _currentEnemy = Spawn(enemyPrefab,position);
    }
    private IEnumerator SpawnWithExplosionRoutine(GameObject enemyPrefab, GameObject explosionPrefab, Vector2 position, float delay)
    {
        if (enemyPrefab.TryGetComponent(out Wave wave) == false) InitiateBoom(explosionPrefab, position);
        yield return new WaitForSeconds(delay);
        _currentEnemy = Spawn(enemyPrefab, position);
    }
    private void InitiateBoom(GameObject explosionPrefab, Vector2 position) => ObjectPooling.instance.GetItem(explosionPrefab, position, _quaternionIdentity);
    private void InitiateBoom(GameObject item, float posX, float posY) => ObjectPooling.instance.GetItem(item, new Vector2(posX, posY), _quaternionIdentity);
    private T Spawn<T>(T item, Vector2 position) where T : Object => Instantiate(item, position, _quaternionIdentity);
    private T Spawn<T>(T item, float posX, float posY) where T : Object => Instantiate(item, new Vector2(posX, posY), _quaternionIdentity);
}