using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Rodar : MonoBehaviour
{
    public UnityAction<Vector2> Ternsite;
    public UnityAction<Vector2> CatchLazer;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out InvertedLazer lazer))
        {
            CatchLazer?.Invoke(collision.gameObject.transform.position);
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
