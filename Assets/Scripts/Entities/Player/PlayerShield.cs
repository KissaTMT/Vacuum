using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;
    private SpriteRenderer _renderer;
    private Sprite _currentSprite;

    public void SetSprite(int id, bool flag)
    {
        _currentSprite = _sprites[id];
        if (flag) _renderer.sprite = _currentSprite;
    }
    private void Awake() => _renderer = GetComponent<SpriteRenderer>();
    private void OnEnable() => _renderer.sprite = _currentSprite;
}
