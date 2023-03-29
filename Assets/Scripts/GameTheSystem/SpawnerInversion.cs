using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerInversion : MonoBehaviour
{
    private const string SCORE_TO_BOSS = "scoreToBoss";
    private const int DELAY_QUEUE = 2;
    private const float DELAY_BETWEEN_ASTEROID = 0.75f;
    private bool EnableSpawn => BonusesManager.instance.HyperdriveTime <= 0 && BlackHoleWay.instance.Timer <= 0;

    [SerializeField] private List<SerializedList<GameObject>> _objects;
    [SerializeField] private GameObject _invertedPlayer, _boom, _bigBoom, _ternsiteHole;
    [SerializeField] private Bonus _bonus;
    [SerializeField] private Snowflake _snowflake;

    private ChangerLocation _changer;
    private Quaternion _quaternionIdentity;
    private Transform _transform;
    private GameObject _currentEnemy;
    private bool _enableAsteroidSpawn;
    private bool _enableHyperdrive;

    private int _scoreToBoss;

    private void Start()
    {
        _scoreToBoss = SceneDataSaver.LoadInt(SCORE_TO_BOSS, 100);
        _changer = ChangerLocation.instance;
        _transform = GetComponent<Transform>();
        _quaternionIdentity = Quaternion.identity;
        StartCoroutine(SpawnAsteroids());
        StartCoroutine(ResurrectionAsteroids());
        StartCoroutine(SpawnQueueBest());
    }
    private void OnDisable() => SceneDataSaver.SaveInt(SCORE_TO_BOSS, (int)GameStats.instance.Score + _scoreToBoss);
    private IEnumerator SpawnAsteroids()
    {
        var delay = new WaitForSeconds(DELAY_BETWEEN_ASTEROID);
        while (true)
        {
            yield return delay;
            if (_currentEnemy == null && _enableAsteroidSpawn && EnableSpawn) Spawn(_objects[_changer.CurrentLocation][0], Random.Range(-2.2f, 2.2f), _transform.position.y);
        }
    }
    private IEnumerator ResurrectionAsteroids()
    {
        var delay = new WaitForSeconds(DELAY_BETWEEN_ASTEROID);
        while (true)
        {
            yield return delay;
            if (_currentEnemy == null && _enableAsteroidSpawn && EnableSpawn)
            {
                var posX = PlayerInteractive.instance.Transform.position.x;
                var posY = PlayerInteractive.instance.Transform.position.y + Random.Range(2f, 4f);
                var position = new Vector2(posX, posY);
                InitiateBoom(_boom, position);
                yield return new WaitForSeconds(1);
                Spawn(_objects[_changer.CurrentLocation][0], position);
            }
        }
    }
    private IEnumerator SpawnQueueBest()
    {
        _enableAsteroidSpawn = false;
        var delay = new WaitForSeconds(DELAY_QUEUE);
        _currentEnemy = Spawn(_invertedPlayer, new Vector3(Random.value > 0.5f ? 1 : -1, -6));
        _currentEnemy.GetComponent<InvertedPlayer>().IsInverse = true;
        yield return new WaitForSeconds(10);
        while (true)
        {
            yield return delay;
            if (EnableSpawn)
            {
                var kind = Random.value;
                _enableHyperdrive = true;
                if (_currentEnemy == null)
                {
                    if (GameStats.instance.Score >= _scoreToBoss && Mathf.Abs(PlayerInteractive.instance.Transform.position.x) < 1 && PlayerInteractive.instance.Transform.position.y < 3.5f)
                    {
                        _enableAsteroidSpawn = false;
                        var position = new Vector3(0, 3.5f);
                        yield return StartCoroutine(InitiateSpawn(_objects[_changer.CurrentLocation][_objects[_changer.CurrentLocation].Count - 1], _bigBoom, position));
                        _scoreToBoss += Random.Range(150, 250);
                    }
                    else
                    {
                        if (!_enableAsteroidSpawn)
                        {
                            if (kind < 0.4f && _enableHyperdrive) yield return StartCoroutine(HyperdriveFlight());
                            else
                            {
                                _enableAsteroidSpawn = true;
                                yield return new WaitForSeconds(Random.Range(6, 12));
                            }
                        }
                        if (kind > 0.5f && kind < 0.9f && PlayerInteractive.instance.Transform.position.y < 3)
                        {
                            _enableAsteroidSpawn = false;
                            var position = new Vector3(PlayerInteractive.instance.Transform.position.x, 3);
                            yield return StartCoroutine(InitiateSpawn(_objects[_changer.CurrentLocation][Random.Range(1, _objects[_changer.CurrentLocation].Count - 1)], _boom, position));
                        }
                        if (kind > 0.9f)
                        {
                            _enableAsteroidSpawn = false;
                            _currentEnemy = Spawn(_ternsiteHole, Random.Range(-1.6f, 1.6f), 8);
                        }
                    }
                }
            }
        }
    }
    private IEnumerator HyperdriveFlight()
    {
        _enableHyperdrive = false;
        PlayerInteractive.instance.OnGetBerk.Invoke((int)Bonuses.Hyperdrive);
        PlayerInteractive.instance.ComponentManager.InitiateHyperdriveFlight();
        StartCoroutine(GenerateSnowflake());
        yield return new WaitForSeconds(ShopOfBonuses.timeOfBonus[(int)Bonuses.Hyperdrive]-1);
        var position = new Vector3(PlayerInteractive.instance.Transform.position.x, PlayerInteractive.instance.Transform.position.y);
        var bonus = Spawn(_bonus, position);
        yield return new WaitUntil(() => Mathf.Abs(bonus.Transform.position.y - 3) < 2f);
        position = new Vector3(bonus.Transform.position.x, 3);
        yield return StartCoroutine(InitiateSpawn(_objects[_changer.CurrentLocation][1], _boom, position));
        yield return new WaitUntil(() => _currentEnemy == null);
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
    private IEnumerator InitiateSpawn(GameObject enemyPrefab, GameObject explosionPrefab, Vector2 position)
    {
        if(enemyPrefab.TryGetComponent(out Wave wave) == false) InitiateBoom(explosionPrefab, position);
        yield return new WaitForSeconds(1);
        _currentEnemy = Spawn(enemyPrefab,position);
    }
    private void InitiateBoom(GameObject explosionPrefab, Vector2 position) => Pooling.instance.GetItem(explosionPrefab, position, _quaternionIdentity);
    private void InitiateBoom(GameObject item, float posX, float posY) => Pooling.instance.GetItem(item, new Vector2(posX, posY), _quaternionIdentity);
    private GameObject Spawn(GameObject obj, float posX, float posY)=> Instantiate(obj, new Vector2(posX, posY), _quaternionIdentity);
    private GameObject Spawn(GameObject obj, Vector2 position) => Instantiate(obj, position, _quaternionIdentity);
    private T Spawn<T>(T item, Vector2 position) where T : MonoBehaviour => Instantiate(item, position, _quaternionIdentity);
}
