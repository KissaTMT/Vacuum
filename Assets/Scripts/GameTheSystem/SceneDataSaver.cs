using System.Collections.Generic;

public class SceneDataSaver
{
    private static Dictionary<string, int> _dataInt = new Dictionary<string, int>();

    public static void SaveInt(string key, int value)
    {
        if (_dataInt.ContainsKey(key)) _dataInt[key] = value;
        else _dataInt.Add(key, value);
    }
    public static int LoadInt(string key, int def)
    {
        var item = def;
        if(_dataInt.TryGetValue(key, out int value)){
            item = value;
            _dataInt.Remove(key);
        }
        return item;
    }
    public static void Clear() => _dataInt.Clear();
}
