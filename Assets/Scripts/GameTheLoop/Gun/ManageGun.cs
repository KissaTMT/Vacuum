using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageGun : MonoBehaviour
{
    public bool EnableShot { get; set; }
    [SerializeField] private SimpleGun[] _gun;
    private void Start()
    {
        EnableShot = true;
        StartCoroutine(ActivateGun());
    }

    private IEnumerator ActivateGun()
    {
        var len = _gun.Length;
        var halfLen = len / 2;
        while (true){
            yield return new WaitForSeconds(0.5f);

            int a = Random.Range(0.0f, 1f) > 0.5f ? 0 : halfLen;
            if (EnableShot)
            {
                for (int i = a; i < (a == 0 ? halfLen : len); i++)
                {
                    _gun[i].EnableShoot = true;
                }
                int b = (a == 0 ? halfLen : len);
                for (int i = (b == halfLen ? halfLen : 0); i < (b == halfLen ? len : halfLen); i++)
                {
                    _gun[i].EnableShoot = false;
                }
            }
            else
            {
                for(var i = 0; i < _gun.Length; i++)
                {
                    _gun[i].EnableShoot = false;
                }
            }
        }
    }
}
