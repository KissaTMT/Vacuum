using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LazerBase : MorningCache, IRun
{
    public SimpleGun Gun { get; protected set; }
    public Transform Transform => transform;

    [SerializeField] protected GameObject b_explosion;
    [SerializeField] protected float b_speed;

    protected Rigidbody2D b_rb;
    protected Quaternion b_quaternionIdentity;
    protected Vector3 b_direction;

    public void Run() => transform.position = Vector3.MoveTowards(transform.position, transform.position + b_direction, b_speed * Time.deltaTime);
    public void SetDirectionFromGun(SimpleGun gun, Vector3 direction)
    {
        Gun = gun;
        b_direction = direction;
    }
    protected override void CacheInit()
    {
        base.CacheInit();
        b_rb = GetComponent<Rigidbody2D>();
        b_quaternionIdentity = Quaternion.identity;
    }

    protected void Death(bool withExplosion = true)
    {
        if(withExplosion) Boom();
        Destroy(gameObject);
    }
    protected virtual void ReactOnMagnet(Collider2D collision)
    {
        if (collision.TryGetComponent(out Magnet magnet))
        {
            if (magnet.Mask == LayerMask.NameToLayer("Everything")) b_direction = collision.transform.position - transform.position;
        }
    }
    protected void Boom() => ObjectPooling.instance.GetItem(b_explosion, transform.position, b_quaternionIdentity);
}
