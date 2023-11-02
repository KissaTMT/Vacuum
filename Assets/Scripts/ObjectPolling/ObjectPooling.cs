using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    public static ObjectPooling instance { get; private set; }
    [NonSerialized] public LinkedList<PoolObject> ActiveObjects;
    [SerializeField] private List<GameObject> _prefabs;
    private Dictionary<GameObject, List<PoolObject>> _pool;
    private int[] _maxCapacities;
    public void Clear()
    {
        while (ActiveObjects.Count != 0)
        {
            var item = ActiveObjects.First.Value;
            item.gameObject.SetActive(false);
        }
    }
    public PoolObject GetItem(GameObject key, Vector2 position, Quaternion quaternion)
    {
        if (TryGetElement(key, out PoolObject element))
        {
            element.Transform.position = position;
            element.Transform.rotation = quaternion;
            ActiveObjects.AddLast(element);
            return element;
        }
        else return CreateObject(key, position, quaternion);
    }
    private bool TryGetElement(GameObject key, out PoolObject element)
    {
        _pool.TryGetValue(key, out List<PoolObject> list);
        var size = list.Count;
        for (var i = 0; i < size; i++)
        {
            if (!list[i].gameObject.activeInHierarchy)
            {
                list[i].gameObject.SetActive(true);
                element = list[i];
                return true;
            }
        }
        element = null;
        return false;
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            InitializePool();
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    private void InitializePool()
    {
        var size = _prefabs.Count;
        _pool = new Dictionary<GameObject, List<PoolObject>>(size);
        ActiveObjects = new LinkedList<PoolObject>();
        _maxCapacities = new int[size];

        for (var i = 0; i < size; i++)
        {
            _maxCapacities[i] = DataSaver.LoadInt(_prefabs[i].name + "Capasity", 1);
        }
        for (var i = 0; i < size; i++)
        {
            _pool.Add(_prefabs[i], new List<PoolObject>(_maxCapacities[i]));
            for(var j = 0; j < _maxCapacities[i]; j++)
            {
                CreateObject(_prefabs[i]);
            }
        }
    }
    private PoolObject CreateObject(GameObject element)
    {
        var prefab = Instantiate(element);
        DontDestroyOnLoad(prefab);
        prefab.SetActive(false);
        var item = prefab.GetComponent<PoolObject>();
        _pool.TryGetValue(element, out List<PoolObject> list);
        list.Add(item);
        return item;
    }
    private PoolObject CreateObject(GameObject element,Vector3 position,Quaternion quaternion)
    {
        var prefab = Instantiate(element, position, quaternion);
        DontDestroyOnLoad(prefab);
        var item = prefab.GetComponent<PoolObject>();
        ActiveObjects.AddLast(item);
        DataSaver.SaveInt(element.name + "Capasity", ++_maxCapacities[_prefabs.IndexOf(element)]);
        _pool.TryGetValue(element, out List<PoolObject> list);
        list.Add(item);
        return item;
    }
}
