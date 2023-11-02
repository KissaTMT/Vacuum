using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Panel : MonoBehaviour
{
    [SerializeField] private Settings _settings;
    [SerializeField] private UnityEvent _action;
    private void OnEnable() => _settings.ReturnBack += () => _action.Invoke();
    private void OnDisable() => _settings.ReturnBack -= () => _action.Invoke();
}
