using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBackround : MonoBehaviour
{
    [SerializeField] private GameObject _startHyperdrive, _white, _ternsiteHole;
    private int _counterLoads;
    private void Awake()
    {
        _counterLoads = SceneDataSaver.LoadInt(nameof(_counterLoads), 0);
        if(_counterLoads == 0) _startHyperdrive.SetActive(true);
        else
        {
            _white.SetActive(true);
            _ternsiteHole.SetActive(true);
        }
        _counterLoads++;
        SceneDataSaver.SaveInt(nameof(_counterLoads), _counterLoads);
    }
}
