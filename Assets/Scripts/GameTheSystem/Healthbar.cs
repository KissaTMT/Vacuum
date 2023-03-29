using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    //[SerializeField] private PlayerInteractive _player;
    //private Image _image;
    //private Color _color;
    //private int formerHeath;
    //
    //private void Awake() => transform.parent.gameObject.SetActive(MenuStats.gameMode != (int)GameMods.Hard);
    //private void OnEnable() => _player.OnChangeHp += UpdateIndicatorData;
    //private void OnDisable() => _player.OnChangeHp -= UpdateIndicatorData;
    //private void Start()
    //{
    //    _image = GetComponent<Image>();
    //    _color = _image.color;
    //    formerHeath = _player.Health;
    //    UpdateIndicatorData(formerHeath);
    //}
    //private void UpdateIndicatorData(int health)
    //{
    //    if (health != formerHeath) _image.color = health < formerHeath ? Color.red : Color.green;
    //    _image.fillAmount = health / 5f;
    //    formerHeath = health;
    //    StartCoroutine(ChangeColor());
    //}
    //private IEnumerator ChangeColor()
    //{
    //    yield return new WaitForSeconds(0.1f);
    //    _image.color = _color;
    //}
}
