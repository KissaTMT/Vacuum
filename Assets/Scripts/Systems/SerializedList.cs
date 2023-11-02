using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class SerializedList<T>
{
    public List<T> List;
    public int Count => List.Count;

    public T this[int index] => List[index];
}
