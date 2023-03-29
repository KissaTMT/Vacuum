using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Magnet : MonoBehaviour
{
    public float Radius => _radius;
    public LayerMask Mask => _mask;

    [SerializeField] private LayerMask _mask;

    private float _radius;
    private void Awake() => _radius = GetComponent<CircleCollider2D>().radius;
}