using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSaver
{
    public static void SaveInt(string key, int value) => PlayerPrefs.SetInt(key, value);
    public static void SaveFloat(string key, float value) => PlayerPrefs.SetFloat(key, value);
    public static void SaveString(string key, string value) => PlayerPrefs.SetString(key, value);
    public static void SaveBool(string key, bool value) => PlayerPrefs.SetInt(key, System.Convert.ToInt32(value));
    public static int LoadInt(string key, int returnValue = 0) => PlayerPrefs.HasKey(key) ? PlayerPrefs.GetInt(key) : returnValue;
    public static float LoadFloat(string key, float returnValue = 0) => PlayerPrefs.HasKey(key) ? PlayerPrefs.GetFloat(key) : returnValue;
    public static string LoadString(string key, string returnValue = default(string)) => PlayerPrefs.HasKey(key) ? PlayerPrefs.GetString(key) : returnValue;
    public static bool LoadBool(string key, bool returnValue = false) => PlayerPrefs.HasKey(key) ? System.Convert.ToBoolean(PlayerPrefs.GetInt(key)) : returnValue;
}
