using KendaGame.Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeHelper : MonoBehaviour
{
  private float startTime;
  private float endTime;

  void Start() {
    startTime = Time.realtimeSinceStartup;
    HTTPRequestHandler.Instance.UserUpdateTimeInterface(int.Parse(StorageHelper.ReadStorage(StorageHelper.TIMESPENTINMINUTES, "0")));

  }

  private void OnApplicationQuit() {
    endTime = Time.realtimeSinceStartup;
    CalculateTimeSpent();
  }

  private void CalculateTimeSpent() {
    float timeSpentInSeconds = endTime - startTime;
    float timeSpentInMinutes = timeSpentInSeconds / 60f;
    int timeSpentInMinutesInt = Mathf.FloorToInt(timeSpentInMinutes);
    int lastTimeSpentInMinutesInt=int.Parse(StorageHelper.ReadStorage(StorageHelper.TIMESPENTINMINUTES, "0"));
    int totalTimeSpendInMinutes = lastTimeSpentInMinutesInt + timeSpentInMinutesInt;

    StorageHelper.WriteStorage(StorageHelper.TIMESPENTINMINUTES,$"{totalTimeSpendInMinutes}");
    Debug.Log(StorageHelper.ReadStorage(StorageHelper.TIMESPENTINMINUTES));
  }
}