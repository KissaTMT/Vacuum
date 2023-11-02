using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PauseListener : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UnityAction<bool> PointExit;

    private bool _isTouch;

    public void OnPointerEnter(PointerEventData eventData) => _isTouch = true;
    public void OnPointerExit(PointerEventData eventData) => StartCoroutine(PointerExitRoutine());

    private IEnumerator PointerExitRoutine()
    {
        var timer = 0f;
        _isTouch = false;
        while (!_isTouch)
        {
            timer += Time.unscaledDeltaTime;
            yield return null;
            if (timer > 0.15f)
            {
                PointExit?.Invoke(true);
                break;
            }
        }
    }
}