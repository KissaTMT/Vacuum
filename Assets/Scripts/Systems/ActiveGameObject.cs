using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveGameObject : MonoBehaviour
{
    private void OnEnable() => ObjectManager.instance.ActiveGameObjects.AddLast(gameObject);
    private void OnDisable() => ObjectManager.instance.ActiveGameObjects.Remove(gameObject);
}