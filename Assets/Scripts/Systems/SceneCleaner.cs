using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SceneCleaner
{
    public static void Clear(LinkedList<GameObject> list)
    {
        while (list.Count != 0)
        {
            var item = list.Last.Value;
            item.SetActive(false);
            DestroyAtAsync(item);
        }
    }
    private static async void DestroyAtAsync(GameObject item)
    {
        await Task.Delay(Random.Range(0, 5) * 1000);
        if (item != null) Object.Destroy(item);
    }
}
