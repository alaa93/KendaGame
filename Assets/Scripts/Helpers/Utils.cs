using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace SteamTech.Utils
{
    public static class CollectionsUtils
    {
        /// <summary>
        /// Randomly shuffle any provided list of items, no need for return type since its a list (by reference)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objects"></param>
        public static void Shuffle<T>(List<T> objects)
        {
            for (var i = objects.Count - 1; i > 0; i--)
            {
                var r = UnityEngine.Random.Range(0, i);
                var tmp = objects[i];
                objects[i] = objects[r];
                objects[r] = tmp;
            }
        }

        /// <summary>
        /// Randomly shuffle any provided array of items, and return the shuffled array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objects"></param>
        /// <returns></returns>
        public static T [] ShuffleArray<T>( T [] objects)
        {
            for (var i = objects.Length - 1; i > 0; i--)
            {
                var r = UnityEngine.Random.Range(0, i);
                var tmp = objects[i];
                objects[i] = objects[r];
                objects[r] = tmp;
            }

            return objects;
        }

        /// <summary>
        /// Swap (A) and (B) items indecies inside a given list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="indexA"></param>
        /// <param name="indexB"></param>
        public static void Swap<T>(List<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
        }

        /// <summary>
        /// Shift a given list to left by providing the number of shifts
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lst"></param>
        /// <param name="shifts"></param>
        public static void ShiftLeft<T>(List<T> lst, int shifts)
        {
            for (int i = shifts; i < lst.Count; i++)
            {
                lst[i - shifts] = lst[i];
            }

            for (int i = lst.Count - shifts; i < lst.Count; i++)
            {
                lst[i] = default;
            }
        }

        /// <summary>
        /// Shift a given list to right by providing the number of shifts
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lst"></param>
        /// <param name="shifts"></param>
        public static void ShiftRight<T>(List<T> lst, int shifts)
        {
            for (int i = lst.Count - shifts - 1; i >= 0; i--)
            {
                lst[i + shifts] = lst[i];
            }

            for (int i = 0; i < shifts; i++)
            {
                lst[i] = default;
            }
        }

    }

    public static class SafetyCheck
    {
        public static bool IsNull(object obj)
        {
            #pragma warning disable IDE0041 // Use 'is null' check
            return  System.Object.ReferenceEquals(obj, null);
            #pragma warning restore IDE0041 // Use 'is null' check
        }

        public static bool IsNotNull(object obj)
        {
            return !System.Object.ReferenceEquals(obj, null);
        }

        public static bool IsSameOrSubclass(Type potentialBase, Type potentialDescendant)
        {
            return potentialDescendant.IsSubclassOf(potentialBase)
                   || potentialDescendant == potentialBase;
        }
    }

    public static class CameraUtils
    {
        public static float GetCameraAspectRatio()
        {
            return (Mathf.Round(Camera.main.aspect * 100f) / 100f);
        }

        /// <summary>
        /// Is it 1280 : 800 (odd landscape) aspect screen
        /// </summary>
        /// <returns></returns>
        public static bool IsNarrowWidthTablet()
        {
            return Mathf.Abs((Mathf.Round(Camera.main.aspect * 100f) / 100f) - 1.6f) <= 0.2f;
        }
        /// <summary>
        /// Is it 2.06 aspect screen
        /// </summary>
        /// <returns> Boolan </returns>
        public static bool IsWideWidthScreen()
        {
            return (Mathf.Round(Camera.main.aspect * 100f) / 100f) >= 2.0f;
        }
        /// <summary>
        /// Is it 2.30 aspect screen (Longest landscape)
        /// </summary>
        /// <returns></returns>
        public static bool IsWidestScreen()
        {
            return Mathf.Abs((Mathf.Round(Camera.main.aspect * 100f) / 100f) - 2.30f) <= 0.2f;
        }
        /// <summary>
        /// Is it 1920 : 1080 aspect screen
        /// </summary>
        /// <returns> Boolan </returns>
        public static bool IsHD()
        {
            return Mathf.Abs((Mathf.Round(Camera.main.aspect * 100f) / 100f) - 1.78f) <= 0.2f;
        }

        public static bool Is4By3Screen()
        {
            return Mathf.Abs((Mathf.Round(Camera.main.aspect * 100f) / 100f) - 1.33f) <= 0.2f;
        }

        /// <summary>
        /// Is it 2048 : 1536 aspect screen
        /// </summary>
        /// <returns> Boolan </returns>
        public static bool IsNarrowWidthTablet_2()
        {
            return Mathf.Abs((Mathf.Round(Camera.main.aspect * 100f) / 100f) - 1.32f) <= 0.2f;
        }

        /// <summary>
        /// Return a screen position for a given world vector parameter
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <returns> Vector3 </returns>
        public static Vector3 GetViewportPosition(Vector3 worldPosition)
        {
            return Camera.main.WorldToViewportPoint(worldPosition);
        }

        /// <summary>
        /// Return a screen position for a given world vector parameter
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <returns> Vector3 </returns>
        public static Vector3 GetScreenPosition(Vector3 worldPosition)
        {
            return Camera.main.WorldToScreenPoint(worldPosition);
        }

        /// <summary>
        /// Return a screen bounds based on the center of the world
        /// </summary>
        /// <returns> Vector3 </returns>
        public static Vector3 GetScreenBounds()
        {
            return Camera.main.WorldToViewportPoint(Vector3.zero);
        }

        /// <summary>
        /// Return left screen edge based on the center of the world
        /// </summary>
        /// <returns></returns>
        public static Vector3 GetLeftScreenBound()
        {
            Vector3 screenBound = GetScreenBounds();
            screenBound.x = 0;
            screenBound.y = 0;
            return screenBound;
        }

        /// <summary>
        /// Return right screen edge based on the center of the world
        /// </summary>
        /// <returns></returns>
        public static Vector3 GetRightScreenBound()
        {
            Vector3 screenBound = GetScreenBounds();
            screenBound.x = 1;
            screenBound.y = 0;
            return screenBound;
        }

        /// <summary>
        /// Return the world position for a given viewport vector position parameter
        /// </summary>
        /// <param name="screenPosition"></param>
        /// <returns> Vector3 </returns>
        public static Vector3 GetWorldPosition(Vector3 screenPosition)
        {
            return Camera.main.ViewportToWorldPoint(screenPosition);
        }

        /// <summary>
        /// Return the world position for a given screen vector position parameter
        /// </summary>
        /// <param name="screenPosition"></param>
        /// <returns> Vector3 </returns>
        public static Vector3 GetWorldPositionFromScreenPos(Vector3 screenPosition)
        {
            return Camera.main.ScreenToWorldPoint(screenPosition); 
        }

        public static Vector3 GetViewportFromScreenPos(Vector3 screenPosition) 
        {
            return Camera.main.ScreenToViewportPoint(screenPosition);
        }

        public static Vector3 GetViewportFromWorldPos(Vector3 worldPosition)
        {
            return Camera.main.WorldToViewportPoint(worldPosition);
        }
    }

    public static class Printer
    {
        public enum PrintColor
        {
            black,green,red
        }

        public static void Print(string message, bool bold = false, PrintColor color = PrintColor.black, UnityEngine.Object context = null)
        {
#if UNITY_EDITOR

            string debugMessage = "";

            if (bold)
                debugMessage = "<b>" + message + "</b>";

            debugMessage = "<color=" + color.ToString() + ">" + debugMessage + "</color>";

            //Debug.Assert((debugMessage.Length > 0), debugMessage, context);
            Debug.Log(debugMessage,context);
#endif
        }
    }

    public static class ExtensionMethods
    {
        /// <summary>
        /// C# extension function which can be called from any string to get the first character in upper case
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string FirstCharToUpper(this string s)
        {
            // Check for empty string.  
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty; 
            }
            // Return char and concat substring.  
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        public static Color ToColor(this string color)
        {
            return (Color)typeof(Color).GetProperty(color.ToLowerInvariant()).GetValue(null, null);
        }

        public static bool Approximately(this Vector3 start, Vector3 target, float percentage)
        {
            var dx = start.x - target.x;
            if (Mathf.Abs(dx) > start.x * percentage)
                return false;

            var dy = start.y - target.y;
            if (Mathf.Abs(dy) > start.y * percentage)
                return false;

            var dz = start.z - target.z;

            return Mathf.Abs(dz) >= start.z * percentage;
        }

        public static bool Approximately(this Vector2 start, Vector2 target, float percentage)
        {
            var dx = start.x - target.x;
            if (Mathf.Abs(dx) > start.x * percentage)
                return false;

            var dy = start.y - target.y;
            if (Mathf.Abs(dy) > start.y * percentage)
                return false;

            return true;
        }

        public static float Remap(this float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
    }

    public static class RandomUtils
    {
        /// <summary>
        /// Generate Random Integer number between provided min and max values
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int GenerateRandom(int min, int max)
        {
            return UnityEngine.Random.Range(min,max);
        }

        /// <summary>
        /// Generate Random Float number between provided min and max values
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float GenerateRandom(float min, float max)
        {
            return UnityEngine.Random.Range(min, max);
        }
    }

    public static class AudioUtils
    {
        public static void FadeOutAudio(AudioSource audioSource, float speed = 1f, Action callback = null)
        {
            audioSource.DOFade(0f, speed).SetEase(Ease.Linear).OnComplete(()=> {

                callback?.Invoke();
            });
        }

        public static void FadeInAudio(AudioSource audioSource, float speed = 1f, Action callback = null)
        {
            audioSource.DOFade(1f, speed).SetEase(Ease.Linear).OnComplete(() => {

                callback?.Invoke();
            });
        }
    }
}
