using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueGunWithTarget : SimpleGun
{
    [SerializeField] private Transform _target;
    [SerializeField] protected int b_countShoots;
    [SerializeField] protected float b_queueDelay;
    protected override IEnumerator ShootingManage()
    {
        var delay = new WaitForSeconds(b_delay);
        var queueDelay = new WaitForSeconds(b_queueDelay);
        while (true)
        {
            if (EnableShoot)
            {
                for (var i = 0; i < b_countShoots; i++)
                {
                    yield return delay;
                    Shoot(_target.position);
                    b_countOfShoots++;
                }
                yield return queueDelay;
            }
            else
            {
                yield return null;
                yield return null;
            }
        }
    }
}
