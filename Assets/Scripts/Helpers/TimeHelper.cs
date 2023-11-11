using KendaGame.Helpers;
using UnityEngine;

public class TimeHelper : MonoBehaviour
{
  private float startTime;
  private float endTime;

  void Start() {
    startTime = Time.realtimeSinceStartup;
    if (int.Parse(StorageHelper.ReadStorage(StorageHelper.TIMESPENTINMINUTES, "-1")) > 0) {
      HTTPRequestHandler.Instance.UserUpdateTimeInterface(int.Parse(StorageHelper.ReadStorage(StorageHelper.TIMESPENTINMINUTES)));
      Debug.Log(StorageHelper.ReadStorage(StorageHelper.TIMESPENTINMINUTES));
    }
  }
  private void OnApplicationPause(bool pause) {
    if (pause) {
      Debug.Log("Time is Work");
      endTime = Time.realtimeSinceStartup;
      CalculateTimeSpent();
    }
  }

  private void CalculateTimeSpent() {
    float timeSpentInSeconds = endTime - startTime;
    float timeSpentInMinutes = timeSpentInSeconds / 60f;
    int timeSpentInMinutesInt = Mathf.FloorToInt(timeSpentInMinutes);
    StorageHelper.WriteStorage(StorageHelper.TIMESPENTINMINUTES,$"{timeSpentInMinutesInt}");
    Debug.Log("Time"+StorageHelper.ReadStorage(StorageHelper.TIMESPENTINMINUTES));
  }

}