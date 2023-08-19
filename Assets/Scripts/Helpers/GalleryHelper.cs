using System;
using UnityEngine;

namespace KendaGame.Helpers {
  public static class GalleryHelper {

    public static void PickImage(Action<bool,Texture2D> result,int maxSize = 256) {
      Texture2D texture = null;
      NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) => {
        Debug.Log("Image path: " + path);
        texture = NativeGallery.LoadImageAtPath(path, maxSize);
        if (path != null) {
          // Create Texture from selected image
          texture = NativeGallery.LoadImageAtPath(path, maxSize);
          if (texture == null) {
            Debug.Log("Couldn't load texture from " + path);
            result.Invoke(false,null);
          }
          else{
          result.Invoke(true,texture);
          }
        }
      });
      Debug.Log("Permission result: " + permission);
    }
  }
}
