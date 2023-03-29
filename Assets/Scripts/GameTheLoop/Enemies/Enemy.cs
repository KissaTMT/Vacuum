using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    public UnityAction<bool> ChangeStateShield;
    public Transform Transform => b_transform;
    public int Health => b_health;

    [SerializeField] protected int b_health;
    [SerializeField] protected float b_speed;

    protected Transform b_transform;
    protected Quaternion b_quaternionIdentity;
    protected Vector3 b_targetPosition;
    protected int b_maxHealth;

    private void Awake()
    {
        b_transform = GetComponent<Transform>();
        b_maxHealth = b_health;
        b_quaternionIdentity = Quaternion.identity;
    }
}
