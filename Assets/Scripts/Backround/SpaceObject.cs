using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceObject : MonoBehaviour
{
    protected Transform b_transform;
    protected SpriteRenderer b_renderer;
    protected float b_size;
    public void SetSprite(Sprite sprite) => b_renderer.sprite = sprite;
    public virtual void Initialize()
    {
        b_size = Random.Range(0.2f, 1);
        Shade(b_size);
        b_transform.localScale = new Vector2(b_transform.localScale.x * b_size, b_transform.localScale.y * b_size);
    }
    protected virtual void OnEnable() => ObjectManager.instance.SpaceObjects.AddLast(gameObject);
    protected virtual void OnDisable() => ObjectManager.instance.SpaceObjects.Remove(gameObject);
    protected void Shade(float size)
    {
        var shader = 0.375f*size + 0.625f;
        b_renderer.color = new Color(shader, shader, shader);
    }
    private void Awake()
    {
        b_transform = GetComponent<Transform>();
        b_renderer = GetComponent<SpriteRenderer>();
    }
}
