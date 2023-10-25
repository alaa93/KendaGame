using KendaGame.Helpers;
using UnityEngine;

public class TimeHelper : MonoBehaviour
{
  private float startTime;
  private float endTime;

  void Start() {
    startTime = Time.realtimeSinceStartup;
    HTTPRequestHandler.Instance.UserUpdateTimeInterface(int.Parse(StorageHelper.ReadStorage(StorageHelper.TIMESPENTINMINUTES, "0")));
    Debug.Log(StorageHelper.ReadStorage(StorageHelper.TIMESPENTINMINUTES));

  }
  private void OnDisable() {
    Debug.Log("Time is Work");
    endTime = Time.realtimeSinceStartup;
    CalculateTimeSpent();
  }

  private void CalculateTimeSpent() {
    float timeSpentInSeconds = endTime - startTime;
    float timeSpentInMinutes = timeSpentInSeconds / 60f;
    int timeSpentInMinutesInt = Mathf.FloorToInt(timeSpentInMinutes);
    StorageHelper.WriteStorage(StorageHelper.TIMESPENTINMINUTES,$"{timeSpentInMinutesInt}");
    Debug.Log(StorageHelper.ReadStorage(StorageHelper.TIMESPENTINMINUTES));
  }
}