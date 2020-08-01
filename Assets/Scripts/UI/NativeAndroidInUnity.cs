using UnityEngine.UI;
using UnityEngine;

public class NativeAndroidInUnity : MonoBehaviour
{
    public static void ShowToast(string toastText)
    {
#if UNITY_ANDROID

        AndroidJavaClass toastClass =
                   new AndroidJavaClass("android.widget.Toast");

        object[] toastParams = new object[3];
        AndroidJavaClass unityActivity =
          new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        toastParams[0] =
                     unityActivity.GetStatic<AndroidJavaObject>
                               ("currentActivity");
        toastParams[1] = toastText;
        toastParams[2] = toastClass.GetStatic<int>
                               ("LENGTH_SHORT");

        AndroidJavaObject toastObject =
                        toastClass.CallStatic<AndroidJavaObject>
                                      ("makeText", toastParams);
        toastObject.Call("show");
#else
		Debug.LogError("No Toast setup for this platform.");
#endif
    }
}
