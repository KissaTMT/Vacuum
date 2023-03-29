using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : SpaceObject
{
    private const float SPEED = 1;
    private float _speed;
    public void SetSize(float size) => b_size = size;
    public override void Initialize()
    {
        b_transform.localScale = new Vector2(b_transform.localScale.x * b_size, b_transform.localScale.y * b_size);
        b_renderer.sortingOrder = (int)(b_size * 100);
        Shade(b_size);
        _speed = b_size * (SPEED + b_size / 5);
        StartCoroutine(Flight());
        StartCoroutine(Boom(b_size));
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        Generator.countOfPlanets++;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        Generator.countOfPlanets--;
    }
    private IEnumerator Flight()
    {
        while (b_transform.position.y > -11)
        {
            b_transform.position += Vector3.down * _speed * Time.deltaTime;
            yield return null;
        }
    }
    private IEnumerator Boom(float size)
    {
        yield return new WaitForSeconds(20 / size);
        Destroy(gameObject);
    }
}
