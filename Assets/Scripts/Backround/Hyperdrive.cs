using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hyperdrive : MonoBehaviour
{
    private Animator _animator;

    public void Animate() => _animator.SetBool("isHyperdrive", false);
    private void Start() => _animator = GetComponent<Animator>();
}
