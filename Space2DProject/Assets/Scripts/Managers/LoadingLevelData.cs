using System;
using UnityEngine;

public class LoadingLevelData : MonoBehaviour
{
    public int seed;
    public int numberOfRooms;
    public int maxFloors;
    public int score;
    public bool hasLaunchedGame = false;

    #region Singleton Don't Destroy On Load
    public static LoadingLevelData Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
    }
    #endregion

    void Start()
    {
        ResetData();
    }

    public void ResetData()
    {
        DateTime dateTime = DateTime.Now;
        int seconds = dateTime.Second;
        int minutes = 100*dateTime.Minute;
        int hours = 10000*dateTime.Hour;
        int days = 1000000*dateTime.Day;
        int months = 100000000*dateTime.Month;
        seed = seconds+minutes+hours+days+months;
    }
}
