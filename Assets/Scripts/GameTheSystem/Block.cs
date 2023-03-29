using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private BonusesManager _bonuses;
    [SerializeField] private BlackHoleWay _hole;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_bonuses.TimeOfBonus[(int)Bonuses.Hyperdrive] > 0) HyperdriveDestroy(collision);
        if (_hole.Timer > 0) BlackHoleDestroy(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_bonuses.TimeOfBonus[(int)Bonuses.Hyperdrive] > 0) HyperdriveDestroy(collision);
        if (_hole.Timer > 0) BlackHoleDestroy(collision);
    }

    private void HyperdriveDestroy(Collider2D other)
    {
        if (other.TryGetComponent(out Drop bonus))
        {
            if (ShopOfBonuses.timeOfBonus[(int)Bonuses.Hyperdrive] - _bonuses.HyperdriveTime > 1) return;
        }

        if (other.TryGetComponent(out PlayerInteractive player)) return;
        else
        {
            Destroy(other.gameObject);
        }
    }

    private void BlackHoleDestroy(Collider2D other)
    {

        if (other.TryGetComponent(out BackroundObject backround)) return;

        if (other.TryGetComponent(out PlayerInteractive player)) return;
        else
        {
            Destroy(other.gameObject);
        }
    }
}
