using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueGun : SimpleGun
{
    [SerializeField] protected int b_countShoots;
    [SerializeField] protected float b_queueDelay;
    protected override IEnumerator ShootingManage()
    {
        var delay = new WaitForSeconds(b_delay);
        var queueDelay = new WaitForSeconds(b_queueDelay);
        yield return delay;
        while (true)
        {
            if (EnableShoot)
            {
                for (var i = 0; i < b_countShoots; i++)
                {
                    yield return delay;
                    Shoot(b_direction);
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
