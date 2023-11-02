using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventHorizon : MonoBehaviour
{
    public bool IsTernsite;
    public bool IsIvert;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Magnet magnet) || collision.TryGetComponent(out Hole hole) || collision.TryGetComponent(out Rodar rodar)
            || collision.TryGetComponent(out Player player) || collision.TryGetComponent(out Enemy enemy)) return;
        else if (collision.TryGetComponent(out PoolObject pool)) collision.gameObject.SetActive(false);
        else Destroy(collision.gameObject);
    }
}
