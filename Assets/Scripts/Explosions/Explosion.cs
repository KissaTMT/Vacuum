using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PoolObject))]
public class Explosion : MonoBehaviour
{
    [SerializeField] protected ParticleSystem[] particleSystems;

    protected WaitForSeconds wait;

    protected int size;

    [SerializeField] private int _seconds;

    protected virtual void OnEnable() => StartCoroutine(GenerateExplosion());
    private IEnumerator GenerateExplosion()
    {
        for (var i = 0; i < size; i++)
        {
            yield return null;
            particleSystems[i].Play();
            yield return null;
            yield return null;
        }

        yield return wait;
        gameObject.SetActive(false);
    }
    private void Awake()
    {
        wait = new WaitForSeconds(_seconds);
        size = particleSystems.Length;
    }
}
