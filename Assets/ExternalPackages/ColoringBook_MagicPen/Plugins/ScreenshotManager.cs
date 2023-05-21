#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.

using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.Networking;
using System.Text;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScreenshotManager : MonoBehaviour {
  public static event Action ScreenshotFinishedSaving;
  public static string screenshotFilename;

#if UNITY_IPHONE
	[DllImport("__Internal")]
    private static extern bool saveToGallery( string path );
#elif UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern void ImageDownloader(string str, string fn);

    private static void DownloadScreenshot(byte[] imageData, string imageFilename)
    {
        ImageDownloader(System.Convert.ToBase64String(imageData), imageFilename);
    }
#endif

  public static IEnumerator Upload(string filePath) {
    WWWForm form = new WWWForm();
    form.AddBinaryData("Imageurl", File.ReadAllBytes(filePath)); // , "imageName.jpg"
    form.AddField("RequiredClass", "alef");

    UnityWebRequest www = UnityWebRequest.Post("http://137.184.184.223:8000/ClientPredict/", form);
    //www.SetRequestHeader("Content-Type", "application/json");
    www.SetRequestHeader("Authorization", "8b38c163cdd104d13ca887f5c17fd4c3324c4a13");

    yield return www.SendWebRequest();

    if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError) {
      Debug.Log(www.error);
    } else {
      Debug.Log("photo upload complete! " + www.downloadHandler.text);

    }
  }
  public static IEnumerator SaveForPaint(Rect rect, string fileName, string albumName = "MyScreenshots", bool callback = false, Action<string> onComplete = null) {
    bool photoSaved = false;
    string share = "I drew this painting! ^_^";
    share += '\n' + "Do you like drawing too? Then install Kids Coloring Book With Magic Pen!";
    string date = System.DateTime.Now.ToString("dd-MM-yy");

    ScreenshotManager.ScreenShotNumber++;

    screenshotFilename = fileName + "_" + ScreenshotManager.ScreenShotNumber + "_" + date + ".png";

    //Rect rect = new Rect(0, Screen.height * 0.2f, Screen.width * 0.8f, Screen.height * 0.8f);

    Debug.Log("Save screenshot " + screenshotFilename);

#if UNITY_IPHONE
		
			if(Application.platform == RuntimePlatform.IPhonePlayer) 
			{
				Debug.Log("iOS platform detected");

      //string iosPath = Application.persistentDataPath + "/" + screenshotFilename;
      string iosPath = Application.persistentDataPath + "/" + screenshotFilename;

      //Application.CaptureScreenshot(screenshotFilename);

      //////////////
      yield return new WaitForEndOfFrame();

                Texture2D texture = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
                texture.ReadPixels(rect, 0, 0);

                texture.Apply();

                yield return 0;

                byte[] bytes = texture.EncodeToJPG();
                File.WriteAllBytes(iosPath, bytes);

                Destroy(texture);
                //////////////
				
				while(!photoSaved) 
				{
					photoSaved = saveToGallery( iosPath );
					
					yield return new WaitForSeconds(.5f);
				}
			
				UnityEngine.iOS.Device.SetNoBackupFlag( iosPath );
			
                new NativeShare().AddFile(iosPath).SetSubject(albumName).SetText(share).Share();
			} 
            else
            {
                //Application.CaptureScreenshot(screenshotFilename);

                //////////////
                yield return new WaitForEndOfFrame();

                Texture2D texture = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
                texture.ReadPixels(rect, 0, 0);

                texture.Apply();

                yield return 0;

                byte[] bytes = texture.EncodeToJPG();
                File.WriteAllBytes("/Users/alaaabuhadbah/workspace/MyPicture_15_06-05-23.png", bytes);

                Destroy(texture);
                yield return new WaitForSeconds(0.1f);
                using (UnityWebRequest www = UnityWebRequest.Post("http://137.184.184.223:8000/ClientPredict/", new WWWForm())) {
                  WWWForm form = new WWWForm();
                  byte[] pictureBytes = File.ReadAllBytes("/Users/alaaabuhadbah/workspace/MyPicture_15_06-05-23.png");
                  Debug.Log("path "+ Application.persistentDataPath + "/" + screenshotFilename);
                  //form.AddBinaryData("Imageurl", pictureBytes, "picture.png");
                  form.AddField("Imageurl", System.Convert.ToBase64String(pictureBytes));
                  form.AddField("RequiredClass", "alef");
                  www.uploadHandler = new UploadHandlerRaw(form.data);
                  www.downloadHandler = new DownloadHandlerBuffer();
                  www.SetRequestHeader("AUTHORIZATION", "Token " + "8b38c163cdd104d13ca887f5c17fd4c3324c4a13");
                  www.SetRequestHeader("Content-Type", "multipart/form-data");
                  yield return www.SendWebRequest();

                  if (www.result != UnityWebRequest.Result.Success) {
                    Debug.Log("Error updating picture: " + www.error);
                  } else {
                    Debug.Log("Picture updated successfully!");
                  }
                }

      //////////////
    }
			
#elif UNITY_ANDROID

    if (Application.platform == RuntimePlatform.Android) {
      Debug.Log("Android platform detected");

      yield return new WaitForEndOfFrame();

      Texture2D texture = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
      texture.ReadPixels(rect, 0, 0);
      texture.Apply();

      yield return 0;

      byte[] bytes = texture.EncodeToJPG();

      string path = Path.Combine(Application.temporaryCachePath, screenshotFilename);
      File.WriteAllBytes(path, bytes);

      NativeGallery.SaveImageToGallery(bytes, albumName, screenshotFilename);

      Destroy(texture);

      new NativeShare().AddFile(path).SetSubject(albumName).SetText(share).Share();
    } else {
      //////////////
      yield return new WaitForEndOfFrame();
      Debug.Log("uploading screenshot to server...");
      Texture2D texture = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
      texture.ReadPixels(rect, 0, 0);

      texture.Apply();

      yield return 0;

      byte[] bytes = texture.EncodeToPNG();
      //string base64Image = $"data:image/png;base64,{System.Convert.ToBase64String(bytes)}";
      //File.WriteAllBytes(Application.persistentDataPath + "/" + screenshotFilename, bytes);
      //Destroy(texture);
      GameObject gameObject1 = new GameObject("RowImage");
      gameObject1.AddComponent<RectTransform>();
      gameObject1.transform.parent = GameObject.FindObjectOfType<Canvas>().transform;
      gameObject1.transform.localScale = Vector3.one;
      RawImage image = gameObject1.AddComponent<RawImage>();
      image.texture = texture;
      
      string _headerValue = "Token 8b38c163cdd104d13ca887f5c17fd4c3324c4a13";

      
      WWWForm formrequest = new WWWForm();
      formrequest.AddBinaryData("Imageurl", bytes, "test.jpeg", "image/jpeg");
      formrequest.AddField("RequiredClass", "alef");

      UnityWebRequest www = UnityWebRequest.Post("http://137.184.184.223:8000/ClientPredict/", formrequest);

      www.SetRequestHeader("Authorization", _headerValue);

      yield return www.SendWebRequest();
      if (www.downloadHandler != null && www.downloadHandler.error != null)
        Debug.Log(www.downloadHandler.error);
      if (www.downloadHandler != null && www.downloadHandler.text != null)
        Debug.Log(www.downloadHandler.text);

    }



#elif UNITY_WEBGL

        yield return new WaitForEndOfFrame();

        Texture2D texture = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
        texture.ReadPixels(rect, 0, 0);

        texture.Apply();

        yield return 0;

        byte[] bytes = texture.EncodeToJPG();

        DownloadScreenshot(bytes, screenshotFilename);

        Destroy(texture);

#else
        while (!photoSaved)
        {
            yield return new WaitForSeconds(.5f);

            photoSaved = true;
        }
#endif
    if (callback)
      ScreenshotFinishedSaving();
  }

  IEnumerator Uploadimage(string url, string _headerKey, byte [] imageBytes) {

    string _headerValue = "Token 8b38c163cdd104d13ca887f5c17fd4c3324c4a13";

    byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes("");
    string dirPath = Application.persistentDataPath + "/saveImages/";
    System.IO.DirectoryInfo info = new System.IO.DirectoryInfo(dirPath);
    System.IO.FileInfo[] fileInfo = info.GetFiles();
    if (fileInfo[fileInfo.Length - 1].Extension.ToString() == ".jpg" && fileInfo[fileInfo.Length - 1].Name.ToString().Substring(0, 5) == "Image") {
      byte[] byteArray = System.IO.File.ReadAllBytes(dirPath + fileInfo[fileInfo.Length - 1].Name.ToString());
      bodyRaw = byteArray;
    }

    WWWForm formrequest = new WWWForm();
    formrequest.AddBinaryData("Imageurl", imageBytes, "test.jpeg", "image/jpeg");
    formrequest.AddField("RequiredClass", "alef");

    UnityWebRequest www = UnityWebRequest.Post(url, formrequest);


    //set jwt
    if (_headerKey != null)
      www.SetRequestHeader(_headerKey, _headerValue);

    yield return www.SendWebRequest();
    if (www.downloadHandler != null && www.downloadHandler.error != null)
      Debug.Log(www.downloadHandler.error);
    if (www.downloadHandler != null && www.downloadHandler.text != null)
      Debug.Log(www.downloadHandler.text);
  }

  static string Authenticate() {
    string auth = "Token" + " " + "8b38c163cdd104d13ca887f5c17fd4c3324c4a13";
    auth = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(auth));
    //auth = "Basic " + auth;
    return auth;
  }

  public static int ScreenShotNumber {
    set { PlayerPrefs.SetInt("screenShotNumber", value); }

    get { return PlayerPrefs.GetInt("screenShotNumber"); }
  }
}
