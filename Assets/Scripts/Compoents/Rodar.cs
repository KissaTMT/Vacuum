using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Rodar : MonoBehaviour
{
    public static Transform target { get; private set; }
    public static Vector3 targetPosition { get; private set; }
    public UnityAction<Vector2> Ternsite;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out InvertedLazer lazer))
        {
            targetPosition = collision.gameObject.transform.position;
            target = collision.gameObject.transform;
        }
        if (collision.TryGetComponent(out EventHorizon eventHorizon))
        {
            if (eventHorizon.IsIvert)
            {
                Ternsite?.Invoke(collision.gameObject.transform.position);
            }
        }
    }
}
