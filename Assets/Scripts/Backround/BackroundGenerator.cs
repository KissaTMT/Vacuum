using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BackroundGenerator : MonoBehaviour
{
    private const float MIN_X_LIMIT = -2.2f;
    private const float MAX_X_LIMIT = 2.2f;
    public static int countOfPlanets { get; set; }

    [SerializeField] private SpaceObject _galaxy;
    [SerializeField] private Planet _planet;
    [SerializeField] private List<SerializedList<Sprite>> _sprites;
    [SerializeField] private ChangerLocation _changer;
    [SerializeField] private BonusesManager _stats;
    [SerializeField] private bool _isInverse;

    private List<float> _sizesOfPlanet = new List<float>();
    private Quaternion _quaternionIdentity;
    private void OnEnable() => _changer.ChangeLocation += OnChangeLocation;
    private void OnDisable() => _changer.ChangeLocation -= OnChangeLocation;

    private void Start()
    {
        countOfPlanets = 0;
        _quaternionIdentity = Quaternion.identity;
        SpawnPlanet(Random.Range(-4, transform.position.y - 2));
        SpawnGalaxy();
        StartCoroutine(ChangeOfSpawn());
    }
    private void OnChangeLocation()
    {
        SceneCleaner.Clear(ObjectManager.instance.SpaceObjects);
        SpawnPlanet(Random.Range(-4, transform.position.y - 2));
        SpawnGalaxy();
    }
    private IEnumerator ChangeOfSpawn()
    {
        var delayBetweenWaves = new WaitForSeconds(Random.Range(1, 5));
        var delayBetweenPlanets = new WaitForSeconds(Random.Range(2, 4));
        while (true)
        {
            yield return delayBetweenWaves;
            if (countOfPlanets < 2 && _stats.HyperdriveTime <= 0)
            {
                _sizesOfPlanet.Clear();
                var countPlanets = Random.Range(1, 4);
                for (int i = 0; i < countPlanets; i++)
                {
                    if (i > 0) SpawnPlanet( 10,true);
                    else SpawnPlanet(10);
                    yield return delayBetweenPlanets;
                }
            }
        }
    }
    private void SpawnGalaxy()
    {
        var size = Random.Range(0, 4);
        var formerPositionsY = new float[size];
        for (int i = 0; i < size; i++)
        {
            var posY = Random.Range(-4, 6);
            while (true)
            {
                if (formerPositionsY.All(i => Mathf.Abs(i - posY) > 1.4f))
                {
                    formerPositionsY[i] = posY;
                    break;
                }
                else posY = Random.Range(-4, 6);
            }
            var galaxy = Spawn(_galaxy, Random.Range(MIN_X_LIMIT, MAX_X_LIMIT), posY);
            galaxy.SetSprite(_sprites[_changer.CurrentLocation][0]);
            galaxy.Initialize();
        }
    }
    private void SpawnPlanet(float posY, bool isSomePlanet = false)
    {
        var planet = (Planet)Spawn(_planet, Random.Range(MIN_X_LIMIT, MAX_X_LIMIT), posY);
        var size = Random.Range(0.1f, 1);
        if (isSomePlanet)
        {
            while (true)
            {
                if (_sizesOfPlanet.All(item => Mathf.Abs(item - size) > 0.2f)) break;
                else size = Random.Range(0.15f, 1f);
            }
        }
        planet.SetSize(size);
        planet.SetSprite(_sprites[_changer.CurrentLocation][Random.Range(1, _sprites[_changer.CurrentLocation].Count)]);
        planet.Initialize();
        _sizesOfPlanet.Add(size);
    }
    private SpaceObject Spawn(SpaceObject obj, float posX, float posY) => Instantiate(obj, new Vector2(posX, posY), _isInverse?Quaternion.Euler(0,0,180):_quaternionIdentity);
}
