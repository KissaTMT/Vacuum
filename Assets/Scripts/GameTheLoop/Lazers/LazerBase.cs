using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LazerBase : MorningCache, IRun
{
    public SimpleGun Gun { get; protected set; }

    [SerializeField] protected GameObject b_explosion;
    [SerializeField] protected float b_speed;

    protected Transform b_transform;
    protected Rigidbody2D b_rb;
    protected Quaternion b_quaternionIdentity;
    protected Vector3 b_direction;

    public void Run() => b_transform.position = Vector3.MoveTowards(b_transform.position, b_transform.position + b_direction, b_speed * Time.deltaTime);
    public void SetDirectionFromGun(SimpleGun gun, Vector3 direction)
    {
        Gun = gun;
        b_direction = direction;
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
            if (magnet.Mask == LayerMask.NameToLayer("Everything")) b_direction = collision.transform.position - b_transform.position;
        }
    }
    protected  void Boom() => Pooling.instance.GetItem(b_explosion, b_transform.position, b_quaternionIdentity);
    private void Awake()
    {
        b_transform = GetComponent<Transform>();
        b_rb = GetComponent<Rigidbody2D>();
        b_quaternionIdentity = Quaternion.identity;
    }
}
