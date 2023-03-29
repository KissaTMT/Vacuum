using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveGun : QueueGun
{
    [SerializeField] private int _countWaves;
    [SerializeField] private float _delayWaves;
    protected override IEnumerator ShootingManage()
    {
        var delayWaves = new WaitForSeconds(_delayWaves);
        var delayQueue = new WaitForSeconds(b_queueDelay);
        var delay = new WaitForSeconds(b_delay);

        while (true)
        {
            yield return delayWaves;
            if (EnableShoot)
            {
                for (int i = 0; i < _countWaves; i++)
                {
                    yield return delayQueue;

                    for (int j = 0; j < b_countShoots; j++)
                    {
                        Shoot(b_direction);
                        yield return delay;
                    }
                }
            }
            else
            {
                yield return null;
                yield return null;
            }
        }
        
    }
}
