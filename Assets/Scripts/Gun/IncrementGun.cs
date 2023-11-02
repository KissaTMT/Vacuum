using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncrementGun : SimpleGun
{
    [SerializeField] protected float b_queueDelay;
    protected override IEnumerator ShootingManage()
    {
        var delay = new WaitForSeconds(b_delay);
        var queueDelay = new WaitForSeconds(b_queueDelay);
        var countShots = 2;
        yield return delay;
        while (true)
        {
            if (EnableShoot)
            {
                for (var i = 0; i < countShots; i++)
                {
                    yield return delay;
                    Shoot(b_direction);
                    b_countOfShoots++;
                }
                countShots += 2;
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
