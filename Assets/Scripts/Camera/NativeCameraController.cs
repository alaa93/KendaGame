using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class NativeCameraController : MonoBehaviour {
  private static WebCamTexture webCamTexture;
  public RawImage displayImage;
  //public Button btn;

  public void StartCamera() {
    if (WebCamTexture.devices.Length > 0) {
      // Get the first available camera
      WebCamDevice cameraDevice = WebCamTexture.devices[0];
      // Create a new instance of WebCamTexture
      webCamTexture = new WebCamTexture(cameraDevice.name);
      // Assign the WebCamTexture to the RawImage component
      if (displayImage != null) {
        displayImage.texture = webCamTexture;
      }
      // Start the camera feed
      webCamTexture.Play();
    }
  }

  private void OnDestroy() {
    if (webCamTexture != null && webCamTexture.isPlaying) {
      // Stop the camera feed when the script is destroyed
      webCamTexture.Stop();
    }
  }

  //private async void OnScreenshotBtnPressed() {
  //  Texture2D capturePhoto = await CaptureCameraScreen();
  //  if (capturePhoto != null) {
  //    Debug.Log("capturePhoto " + capturePhoto.texelSize);
  //  } else {
  //    Debug.Log("capturePhoto is null");
  //  }
  //}

  public async Task<Texture2D> Capture() {
    return await CaptureCameraScreen();
  }


  public async Task<Texture2D> CaptureCameraScreen() {
    if (webCamTexture != null && webCamTexture.isPlaying) {
      // Capture the current frame from the camera texture
      Texture2D screenshot = new Texture2D(webCamTexture.width, webCamTexture.height, TextureFormat.RGB24, false);
      screenshot.SetPixels(webCamTexture.GetPixels());
      //screenshot.Apply();
      await Task.Delay(300);
      return screenshot;
    }
    return null;
  }
}
