using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    private void OnEnable() => _enemy.ChangeStateShield += (flag) => gameObject.GetComponent<SpriteRenderer>().enabled = flag;
    private void OnDisable() => _enemy.ChangeStateShield -= (flag) => gameObject.GetComponent<SpriteRenderer>().enabled = flag;
}
