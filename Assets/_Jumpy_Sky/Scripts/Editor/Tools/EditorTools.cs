using UnityEngine;
using UnityEditor;
using System.IO;

public class EditorTools : EditorWindow {


    [MenuItem("Tools/Reset PlayerPrefs")]
    public static void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("*************** PlayerPrefs Was Deleted ***************");
    }


    [MenuItem("Tools/Capture Screenshot")]
    public static void CaptureScreenshot()
    {
        string path = "C:/Users/Nguyen Quang Tien/Desktop/icon.png";
        ScreenCapture.CaptureScreenshot(path);
    }


    //[MenuItem("Tools/Create Domino Pack")]
    //public static void CreateDominoPack()
    //{
    //    FindObjectOfType<IngameManager>().CreateTiles();
    //}
}
