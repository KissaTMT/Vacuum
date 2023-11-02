using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Legacy
{
    public class SpawnerCurrent : MonoBehaviour
    {
        private const string SCORE_TO_BOSS = "scoreToBoss";
        private const int DELAY_QUEUE = 2;
        private const float DELAY_BETWEEN_ASTEROID = 0.5f;

        private bool EnableSpawn => BonusesManager.instance.HyperdriveTime <= 0 && BlackHoleWay.instance.IsDisactive;

        [SerializeField] private List<SerializedList<GameObject>> _objects;
        [SerializeField] private GameObject _invertedPlayer, _hole, _redHole, _crystal;


        private ChangerLocation _changer;
        private Transform _transform;
        private Quaternion _quaternionIdentity;
        private GameObject _currentEnemy;
        private bool _enableAsteroidSpawn;

        private int _scoreToBoss;

        private void Start()
        {
            _scoreToBoss = SceneDataSaver.LoadInt(SCORE_TO_BOSS, 100);
            _transform = GetComponent<Transform>();
            _changer = ChangerLocation.instance;
            _quaternionIdentity = Quaternion.identity;
            StartCoroutine(SpawnAsteroid());
            StartCoroutine(SpawnQueueBest());
        }
        private void OnDisable() => SceneDataSaver.SaveInt(SCORE_TO_BOSS, (int)GameStats.instance.Score + _scoreToBoss);

        private IEnumerator SpawnQueueBest()
        {
            var delay = new WaitForSeconds(DELAY_QUEUE);
            while (true)
            {
                yield return delay;

                if (EnableSpawn)
                {
                    var kind = Random.value;

                    if (_currentEnemy == null)
                    {
                        if (GameStats.instance.Score >= _scoreToBoss)
                        {
                            _enableAsteroidSpawn = false;
                            _currentEnemy = Spawn(_objects[_changer.CurrentLocation][_objects[_changer.CurrentLocation].Count - 1], 0, _transform.position.y);
                            _scoreToBoss += Random.Range(150, 250);
                        }
                        else
                        {
                            if (_enableAsteroidSpawn == false) _enableAsteroidSpawn = true;
                            if (kind > 0.5f && kind < 0.99f) _currentEnemy = Spawn(_objects[_changer.CurrentLocation][Random.Range(1, _objects[_changer.CurrentLocation].Count - 1)], Random.Range(-1.6f, 1.6f), _transform.position.y);
                            if (kind > 0.998f)
                            {
                                _enableAsteroidSpawn = false;
                                yield return delay;
                                var posX = Random.Range(-1.6f, 1.6f);
                                _currentEnemy = Spawn(_invertedPlayer, posX, -6);
                                yield return new WaitForSeconds(Random.Range(8, 14));
                                Spawn(_redHole, posX, _transform.position.y);
                            }
                        }
                        if (kind > 0.99f && kind < 0.998f) Spawn(_hole, Random.Range(-2.2f, 2.2f), _transform.position.y);
                    }
                }
                else yield return StartCoroutine(GenerateCrystal());
            }
        }
        private IEnumerator GenerateCrystal()
        {
            var delay = new WaitForSeconds(0.5f);
            while (BonusesManager.instance.HyperdriveTime > 4)
            {
                Spawn(_crystal, Random.Range(-2f, 2f), _transform.position.y);
                yield return delay;
            }
        }
        private IEnumerator SpawnAsteroid()
        {
            var delay = new WaitForSeconds(DELAY_BETWEEN_ASTEROID);
            while (true)
            {
                yield return delay;
                if (EnableSpawn && _enableAsteroidSpawn) ObjectPooling.instance.GetItem(_objects[_changer.CurrentLocation][0], new Vector2(Random.Range(-2.2f, 2.2f), _transform.position.y), _quaternionIdentity);
            }
        }
        private GameObject Spawn(GameObject obj, float posX, float posY) => Instantiate(obj, new Vector2(posX, posY), _quaternionIdentity);
    }
}