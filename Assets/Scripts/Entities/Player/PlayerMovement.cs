using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MorningCache, IRun
{
    private const int FLIGHT_SPEED = 30;

    private const float MIN_X_LIMIT = -2.2f;
    private const float MAX_X_LIMIT = 2.2f;
    private const float MIN_Y_LIMIT = -4.8f;
    private const float MAX_Y_LIMIT = 4.8f;

    private Camera _cameraMain;

    public void Run() => Movement(GetPosition());
    protected override void CacheInit()
    {
        base.CacheInit();
        _cameraMain = Camera.main;
    }
    private Vector3 GetPosition()
    {
        var position = _cameraMain.ScreenToWorldPoint(Input.mousePosition);
        return new Vector3(Mathf.Clamp(position.x, MIN_X_LIMIT, MAX_X_LIMIT), Mathf.Clamp(position.y, MIN_Y_LIMIT, MAX_Y_LIMIT));
    }
    private void Movement(Vector3 targetPosition) => transform.position += (targetPosition - transform.position) * FLIGHT_SPEED * Time.deltaTime;
}
