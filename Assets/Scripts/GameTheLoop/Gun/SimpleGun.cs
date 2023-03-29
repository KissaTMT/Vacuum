using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleGun : MonoBehaviour
{
    public bool EnableShoot;
    public int CountShoots => b_countOfShoots;
    public float Delay => b_delay;

    [SerializeField] protected LazerBase b_lazerPrefab;
    [SerializeField] protected Vector2 b_direction;
    [SerializeField] protected float b_delay;
    [SerializeField] protected int b_angle;

    protected Transform b_transform;
    protected int b_countOfShoots;

    protected virtual IEnumerator ShootingManage()
    {
        var delay = new WaitForSeconds(b_delay);
        while (true)
        {
            if (EnableShoot)
            {
                yield return delay;
                Shoot(b_direction);
            }
            else
            {
                yield return null;
                yield return null;
            }
        }
    }
    private void Awake() => b_transform = GetComponent<Transform>();
    private void OnEnable() => StartCoroutine(ShootingManage());
    protected void Shoot(Vector2 direction) => Instantiate(b_lazerPrefab, b_transform.position, Quaternion.Euler(0, 0, b_angle)).SetDirectionFromGun(this, direction);
}
