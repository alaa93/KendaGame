using UnityEngine;

public class PermissionManager : MonoBehaviour {
  //private const string WriteExternalStoragePermission = "android.permission.WRITE_EXTERNAL_STORAGE";
  private const int RequestCode = 1;

  void Start() {
    // Request the permission
    RequestWriteExternalStoragePermission();
  }

  private void RequestWriteExternalStoragePermission() {
    // Check if the permission is already granted
    if (!HasWriteExternalStoragePermission()) {
      Debug.Log("WRITE_EXTERNAL_STORAGE permission is not granted requesting it RN...");
      // Request the permission
    //   AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    //   AndroidJavaObject unityPlayerActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
    //   AndroidJavaObject packageManager = unityPlayerActivity.Call<AndroidJavaObject>("getPackageManager");
    //   string packageName = unityPlayerActivity.Call<string>("getPackageName");
    //   AndroidJavaObject permissionClass = new AndroidJavaObject("android.Manifest$permission");
    //   string permissionString = permissionClass.GetStatic<string>(WriteExternalStoragePermission);
    //   int permissionStatus = packageManager.Call<int>("checkPermission", permissionString, packageName);

    //   if (permissionStatus == 0) {
    //     // Permission is already granted
    //     Debug.Log("WRITE_EXTERNAL_STORAGE permission is now granted.");
    //   } else {
    //     // Permission is not granted, request it
    //     AndroidJavaClass ActivityCompatClass = new AndroidJavaClass("androidx.core.app.ActivityCompat");
    //     ActivityCompatClass.CallStatic("requestPermissions", unityPlayerActivity, new string[] { permissionString }, RequestCode);
    //   }
    // } else {
    //   // Permission is already granted
    //   Debug.Log("WRITE_EXTERNAL_STORAGE permission is already granted.");
     }
  }

  private bool HasWriteExternalStoragePermission() {
    return true;
    // AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    // AndroidJavaObject unityPlayerActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
    // AndroidJavaObject packageManager = unityPlayerActivity.Call<AndroidJavaObject>("getPackageManager");
    // string packageName = unityPlayerActivity.Call<string>("getPackageName");
    // AndroidJavaObject permissionClass = new AndroidJavaObject("android.Manifest$permission");
    // string permissionString = permissionClass.GetStatic<string>(WriteExternalStoragePermission);
    // int permissionStatus = packageManager.Call<int>("checkPermission", permissionString, packageName);

    // return permissionStatus == 0;
  }

  // Handle the permission request result
  void OnRequestPermissionsResult(string permission, string[] grantResults) {
    return;
    // if (permission.Equals(WriteExternalStoragePermission)) {
    //   if (grantResults.Length > 0 && grantResults[0].Equals("android.content.pm.PackageManager+PERMISSION_GRANTED")) {
    //     // Permission is granted
    //     Debug.Log("WRITE_EXTERNAL_STORAGE permission is granted.");
    //   } else {
    //     // Permission is denied
    //     Debug.Log("WRITE_EXTERNAL_STORAGE permission is denied.");
    //   }
    // }
  }
}