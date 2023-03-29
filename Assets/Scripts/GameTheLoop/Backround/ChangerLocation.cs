using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChangerLocation : MonoBehaviour
{
    public static ChangerLocation instance { get; private set; }
    public int CurrentLocation => _currentLocation;
    public int FormerLocation => _formerLocation;

    public UnityAction ChangeLocation;

    [SerializeField] private GameObject[] _location;
    [SerializeField] private GameObject _hyperdrive;
    [SerializeField] private Hyperdrive[] _hyperdrives;
    [SerializeField] private BonusesManager _bonusesManager;
    [SerializeField] private bool _isInverse;

    private int _currentLocation;
    private int _formerLocation;

    private void Awake()
    {
        instance = this;
        _currentLocation = !_isInverse ? Random.Range(0, _location.Length) : 1;
        _location[_currentLocation].SetActive(true);
    }
    private void OnEnable() => _bonusesManager.Hyperdrive += () => StartCoroutine(HyperdriveFlight());
    private void OnDisable() => _bonusesManager.Hyperdrive -= () => StartCoroutine(HyperdriveFlight());
    private IEnumerator HyperdriveFlight()
    {
        SceneCleaner.Clean(ObjectManager.instance.ActiveGameObjectList);
        Pooling.instance.Clear();
        _hyperdrive.SetActive(true);
        yield return new WaitForSeconds(ShopOfBonuses.timeOfBonus[(int)Bonuses.Hyperdrive] / 2);
        Change();
        yield return new WaitUntil(() => _bonusesManager.HyperdriveTime == 1);
        for (var i = 0; i < _hyperdrives.Length; i++)
        {
            _hyperdrives[i].Animate();
        }
        yield return new WaitForSeconds(2);
        _hyperdrive.SetActive(false);
    }
    private void Change()
    {
        var nextLocation = Random.Range(0, _location.Length);
        if (nextLocation == _currentLocation)
        {
            while (true)
            {
                nextLocation = Random.Range(0, _location.Length);
                if (nextLocation != _currentLocation) break;
            }
        }
        _formerLocation = _currentLocation;
        _currentLocation = nextLocation;
        _location[_formerLocation].SetActive(false);
        _location[_currentLocation].SetActive(true);
        ChangeLocation?.Invoke();
    }

}
