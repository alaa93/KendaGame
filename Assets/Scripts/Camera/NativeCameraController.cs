//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.SceneManagement;
//using System.Collections;

//namespace KendaGame.UI {
//  public class NativeCameraController : MonoBehaviour {

//    private WebCamTexture webCamTexture;
//    public RawImage displayImage;

//    void Start() {
//      // Check if the device has a camera
//      if (WebCamTexture.devices.Length > 0) {
//        // Get the first available camera
//        WebCamDevice cameraDevice = WebCamTexture.devices[0];

//        // Create a new instance of WebCamTexture
//        webCamTexture = new WebCamTexture(cameraDevice.name);

//        // Assign the WebCamTexture to the RawImage component
//        displayImage.texture = webCamTexture;

//        // Start the camera feed
//        webCamTexture.Play();
//      }
//    }

//    void Update() {
//      // Toggle camera feed on/off when space bar is pressed
//      if (Input.GetKeyDown(KeyCode.Space)) {
//        if (webCamTexture.isPlaying) {
//          webCamTexture.Pause();
//        } else {
//          webCamTexture.Play();
//        }
//      }

//      if (Input.GetMouseButtonDown(0)) {
//        if (webCamTexture.isPlaying) {
//          webCamTexture.Pause();
//        } else {
//          webCamTexture.Play();
//        }
//      }
//    }

//  }
//}


using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class NativeCameraController : MonoBehaviour {
  private WebCamTexture webCamTexture;
  public RawImage displayImage;
  public Button btn;


  public void CaptureCameraScreen() {
    if (webCamTexture != null && webCamTexture.isPlaying) {
      // Create a unique filename using timestamp
      string timestamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
      string filename = "CameraScreenshot_" + timestamp + ".png";

      // Capture the current frame from the camera texture
      Texture2D screenshot = new Texture2D(webCamTexture.width, webCamTexture.height);
      screenshot.SetPixels(webCamTexture.GetPixels());
      screenshot.Apply();

      // Convert the texture to a PNG image byte array
      byte[] bytes = screenshot.EncodeToPNG();

      // Save the PNG image to a file
      string filePath = Path.Combine(Application.persistentDataPath, filename);
      File.WriteAllBytes(filePath, bytes);

      // Save the screenshot to the device's gallery (camera roll)
      //SaveToGallery(filePath);
      SaveToGalleryAdvanced(filePath);
      Debug.Log("Camera screenshot captured and saved to gallery.");
    }
  }


  private void SaveToGallery(string filePath) {
    Debug.Log("calling SaveToGallery...");
    // Check if the platform is Android
    if (Application.platform == RuntimePlatform.Android) {
      Debug.Log("RuntimePlatform.Android");
      // Get the Android Java class for UnityPlayer
      AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
      AndroidJavaObject unityPlayerActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");

      // Get the Android Java class for MediaScannerConnection
      AndroidJavaClass mediaScannerClass = new AndroidJavaClass("android.media.MediaScannerConnection");

      // Trigger the media scanner to scan the saved image file
      string[] filePaths = { filePath };
      string[] mimeTypes = { "image/png" };
      mediaScannerClass.CallStatic("scanFile", unityPlayerActivity, filePaths, mimeTypes, null);
      Debug.Log("after calling static [scanFile] fucntion");
    }
    // Check if the platform is iOS
    else if (Application.platform == RuntimePlatform.IPhonePlayer) {
      // Call the iOS native plugin or use the Unity-iPhone UnitySendMessage function to save the image to the device's gallery
      // You'll need to implement the iOS native code or UnitySendMessage function separately
    }
  }

  private void SaveToGalleryAdvanced(string filePath) {
    Debug.Log("SaveToGalleryAdvanced");
    if (Application.platform == RuntimePlatform.Android) {
      Debug.Log("RuntimePlatform.Android");
      // Get the Android Java class for UnityPlayer
      AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
      AndroidJavaObject unityPlayerActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");

      // Create a new Android Intent to insert the image into the Media Store
      AndroidJavaObject mediaScanIntent = new AndroidJavaObject("android.content.Intent", "android.intent.action.MEDIA_SCANNER_SCAN_FILE");

      // Create a new Android File object for the captured screenshot
      AndroidJavaObject file = new AndroidJavaObject("java.io.File", filePath);

      // Get the Android Uri for the File object
      // Get the Android Uri for the File object
      AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
      AndroidJavaObject uri = uriClass.CallStatic<AndroidJavaObject>("fromFile", file);

      // Set the data Uri of the Intent
      mediaScanIntent.Call<AndroidJavaObject>("setData", uri);

      // Call the Android Activity's sendBroadcast method to send the media scan intent
      unityPlayerActivity.Call("sendBroadcast", mediaScanIntent);
      Debug.Log("calling sendBroadcast method to send the media scan intent");
      Debug.Log("Final file path "+ filePath);
    } else if (Application.platform == RuntimePlatform.IPhonePlayer) {
      // Call the iOS native plugin or use the Unity-iPhone UnitySendMessage function to save the image to the device's gallery
      // You'll need to implement the iOS native code or UnitySendMessage function separately
    }
  }



  void Start() {
    // Check if the device has a camera
    if (WebCamTexture.devices.Length > 0) {
      // Get the first available camera
      WebCamDevice cameraDevice = WebCamTexture.devices[0];

      // Create a new instance of WebCamTexture
      webCamTexture = new WebCamTexture(cameraDevice.name);

      // Assign the WebCamTexture to the RawImage component
      displayImage.texture = webCamTexture;

      // Start the camera feed
      webCamTexture.Play();
    }

    btn.onClick.AddListener(CaptureCameraScreen);
  }

  void OnDestroy() {
    if (webCamTexture != null && webCamTexture.isPlaying) {
      // Stop the camera feed when the script is destroyed
      webCamTexture.Stop();
    }
  }
}
