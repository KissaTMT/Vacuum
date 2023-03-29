using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    [SerializeField] private float _lifeTime;
    private void OnEnable() => StartCoroutine(Destroy());
    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(_lifeTime + 1);
        Destroy(gameObject);
    }
}
