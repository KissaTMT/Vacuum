using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shader : MonoBehaviour
{
    public static Shader instance;

    private SpriteRenderer _renderer;

    public void Shade() => StartCoroutine(ShadeRoutine());
    public void Unshade() => StartCoroutine(UnshadeRoutine());

    private void Awake()
    {
        instance = this;
        _renderer = GetComponent<SpriteRenderer>();
    }
    private IEnumerator ShadeRoutine()
    {
        var red = _renderer.color.r;
        var green = _renderer.color.g;
        var blue = _renderer.color.b;
        for (var i = 0f; i < 1f; i += Time.deltaTime)
        {
            _renderer.color = new Color(red, green, blue, i);
            yield return null;
        }
    }

    private IEnumerator UnshadeRoutine()
    {
        var red = _renderer.color.r;
        var green = _renderer.color.g;
        var blue = _renderer.color.b;
        for (var i = 1f; i > 0f; i -= Time.deltaTime)
        {
            _renderer.color = new Color(red, green, blue, i);
            yield return null;
        }
    }
}
