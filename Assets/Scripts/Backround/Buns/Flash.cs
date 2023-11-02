using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;
    private SpriteRenderer _renderer;

    private void Awake() => _renderer = GetComponent<SpriteRenderer>();
    private void Start() => _renderer.sprite = _sprites[Random.Range(0, _sprites.Length)];
}
