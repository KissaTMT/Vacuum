using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusAnimator : MonoBehaviour
{
    private Animator _animator;
    private RectTransform _rectTransform;

    public void Animate(bool flag)
    {
        _animator.SetBool("disableBonus", flag);
        if (!flag) Reload();
    }
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rectTransform = GetComponent<RectTransform>();
    }
    private void OnDisable() => Reload();
    private void Reload()
    {
        _rectTransform.localScale = new Vector3(1, 1, 1);
        _animator.SetBool("disableBonus", false);
    }
}
