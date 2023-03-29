using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achivements : MonoBehaviour
{
    public static bool bananaIsRescued { get; set; }
    public static bool inversionIsCompleted { get; set; }
    public static bool isMeetInvertedPlayer { get; set; }

    [SerializeField] private GameObject[] _cheakers;

    private void Start()
    {
        bool[] achivements = {
            bananaIsRescued = GetData("RescueBanana"),
            inversionIsCompleted = GetData("InversionCompleted"),
            isMeetInvertedPlayer = GetData("MeetInvertedPlayer")
        };
        for(int i = 0; i < _cheakers.Length; i++)
        {
            _cheakers[i].SetActive(achivements[i]);
        }
    }

    private bool GetData(string key) => PlayerPrefs.HasKey(key) ? System.Convert.ToBoolean(PlayerPrefs.GetInt(key)) : false;
}
