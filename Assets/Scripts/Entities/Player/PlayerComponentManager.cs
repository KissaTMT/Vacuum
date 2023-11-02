using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponentManager : MonoBehaviour
{
    public PlayerGun MainGun => _guns[0];

    [SerializeField] private Animator[] _lights;
    [SerializeField] private PlayerGun[] _guns;
    [SerializeField] private GameObject[] _bonuses;

    [SerializeField] private PlayerShield _shield;
    [SerializeField] private PlayerEngine _engine;
    public void InitiateHoleFlight()
    {
        ManageEnableGun(false);
        _lights[_lights.Length - 1].gameObject.SetActive(false);
        _lights[_lights.Length - 1].gameObject.SetActive(true);
        _shield.SetSprite(0, _shield.gameObject.activeInHierarchy);
        _engine.SetColor(0);
        _lights[ChangerLocation.instance.CurrentLocation].SetBool("IsOn", false);
    }
    public void InitiateHyperdriveFlight() => StartCoroutine(HyperdriveFlight());
    public void InitiateManageBonus(int index) => StartCoroutine(ManageBonus(index));
    private void Start() => SetComponentByLocation(ChangerLocation.instance.CurrentLocation);
    private void SetComponentByLocation(int id, bool isHyperdrive = false)
    {
        _lights[isHyperdrive ? _lights.Length - 1 : id].gameObject.SetActive(true);
        _shield.SetSprite(id, _shield.gameObject.activeInHierarchy);
        _engine.SetColor(id);
        for (var i = 0; i < _guns.Length; i++)
        {
            _guns[i].SetLazer(id);
        }
    }
    private IEnumerator HyperdriveFlight()
    {
        ManageEnableGun(false);
        _lights[_lights.Length - 1].gameObject.SetActive(false);
        _lights[ChangerLocation.instance.CurrentLocation].SetBool("IsOn", false);
        SetComponentByLocation(0, true);
        yield return new WaitUntil(() => BonusesManager.instance.HyperdriveTime < 1f);
        _lights[_lights.Length - 1].SetBool("IsOn", false);
        SetComponentByLocation(ChangerLocation.instance.CurrentLocation);
        ManageEnableGun(true);
        _lights[ChangerLocation.instance.FormerLocation].gameObject.SetActive(false);
    }
    private IEnumerator ManageBonus(int index)
    {
        _bonuses[index].SetActive(true);
        yield return new WaitUntil(() => BonusesManager.instance.TimeOfBonus[index] <= 0);
        _bonuses[index].SetActive(false);
    }
    private void ManageEnableGun(bool enable)
    {
        for (var i = 0; i < _guns.Length; i++)
        {
            _guns[i].gameObject.SetActive(enable);
        }
    }
}
