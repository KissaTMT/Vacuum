using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SafeArea : MonoBehaviour
{
    private List<RectTransform> _movingButtons = new List<RectTransform>();
    private RectTransform _rect;
    private Vector2 _anchorMin = new Vector2(0, 0);
    private Vector2 _anchorMax = new Vector2(1, 1);

    public void ToSafeArea(bool flag = false)
    {
        var anchorMin = _anchorMin;
        var anchorMax = _anchorMax;
        if (flag)
        {
            var safeArea = Screen.safeArea;

            anchorMin = safeArea.position;
            anchorMax = anchorMin + safeArea.size;
            anchorMin.x /= Screen.width;
            anchorMax.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.y /= Screen.height;
        }
        _rect.anchorMin = anchorMin;
        _rect.anchorMax = anchorMax;
        DataSaver.SaveBool($"{gameObject.name}",flag);
    }
    private void Start()
    {
        _rect = GetComponent<RectTransform>();
        if (BonusesManager.instance)
        {
            var rect = GetComponentsInChildren<RectTransform>();

            for(var i = 0; i < rect.Length; i++)
            {
                if (rect[i].GetComponent<Button>())
                {
                    _movingButtons.Add(rect[i]);
                }
            }
        }
        ToSafeArea(DataSaver.LoadBool($"{gameObject.name}"));
    }
}
