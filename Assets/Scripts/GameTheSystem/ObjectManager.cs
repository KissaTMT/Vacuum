using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager instance { get; private set; }

    [System.NonSerialized] public LinkedList<GameObject> ActiveGameObjectList = new LinkedList<GameObject>();
    [System.NonSerialized] public LinkedList<GameObject> SpaceObjectList = new LinkedList<GameObject>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
}
