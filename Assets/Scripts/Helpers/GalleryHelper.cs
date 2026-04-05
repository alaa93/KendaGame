// using System;
// using UnityEngine;

// namespace KendaGame.Helpers {
//   public static class GalleryHelper {

//     public static void PickImage(Action<bool,Texture2D> result,int maxSize = 256) {
//       Texture2D texture = null;
//       NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) => {
//         texture = NativeGallery.LoadImageAtPath(path, maxSize);
//         if (path != null) {
//           // Create Texture from selected image
//           texture = NativeGallery.LoadImageAtPath(path, maxSize);
//           if (texture == null) {
//             result.Invoke(false,null);
//           }
//           else{
//           result.Invoke(true,texture);
//           }
//         }
//       });
//       Debug.Log("Permission result: " + permission);
//     }

//     public static Texture2D GetReadableTexture(Texture2D source) {
//       RenderTexture renderTex = RenderTexture.GetTemporary(
//                             source.width,
//                             source.height,
//                             0,
//                             RenderTextureFormat.Default,
//                             RenderTextureReadWrite.Linear);

//       Graphics.Blit(source, renderTex);
//       RenderTexture previous = RenderTexture.active;
//       RenderTexture.active = renderTex;
//       Texture2D readableText = new Texture2D(source.width, source.height);
//       readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
//       readableText.Apply();
//       RenderTexture.active = previous;
//       RenderTexture.ReleaseTemporary(renderTex);
//       return readableText;
//     }

//   }
// }
