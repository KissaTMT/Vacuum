using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlackHoleWay : MonoBehaviour
{
    public static BlackHoleWay instance;
    public bool IsDisactive => _timer <= 0;

    [SerializeField] private GameObject[] _buns;
    [SerializeField] private GameObject _blackHoleAttraction, _white,_flash;

    private Player _player;
    private Quaternion _quaternionIdentity;
    private float _timer;
    public void Initialize()
    {
        _player = Player.instance;
        instance = this;
        _player.FallingIntoDark += InitiateHoleFlight;
    }
    private void OnDisable() => _player.FallingIntoDark -= InitiateHoleFlight;
    private void Start() => _quaternionIdentity = Quaternion.identity;
    private void InitiateHoleFlight(int id)
    {
        SceneCleaner.Clear(ObjectManager.instance.ActiveGameObjects);
        StartCoroutine(Ternsite(id));
    }
    private IEnumerator Ternsite(int id)
    {
        yield return StartCoroutine(Flight());
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(id);
    }
    private IEnumerator Flight()
    {
        _timer = 20;
        _blackHoleAttraction.SetActive(true);
        var wait = new WaitForSeconds(1);
        StartCoroutine(SpawnFlashes());
        StartCoroutine(SpawnBuns());
        while (_timer > 1)
        {
            _timer--;
            yield return wait;
        }
        StopCoroutine(SpawnFlashes());
        StopCoroutine(SpawnBuns());
        _white.SetActive(true);
    }
    private IEnumerator SpawnFlashes()
    {
        var wait = new WaitForSeconds(0.4f);
        while (true)
        {
            yield return wait;
            Instantiate(_flash, new Vector2(Random.Range(-2.2f, 2.2f), 6), _quaternionIdentity);
        }
    }

    private IEnumerator SpawnBuns()
    {
        var wait = new WaitForSeconds(Random.Range(1, 2));
        while (true)
        {
            yield return wait;
            Instantiate(_buns[Random.Range(0, _buns.Length)], new Vector2(Random.Range(-2.2f, 2.2f), 4), _quaternionIdentity);
        }

    }
}
