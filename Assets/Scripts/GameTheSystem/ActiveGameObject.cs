using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveGameObject : MonoBehaviour
{
    private void OnEnable() => ObjectManager.instance.ActiveGameObjectList.AddLast(gameObject);
    private void OnDisable() => ObjectManager.instance.ActiveGameObjectList.Remove(gameObject);
}