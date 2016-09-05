using UnityEngine;
using System.Collections;

public class Utility
{
    #region Path
    //本地读取的路径
    static public readonly string ReadPlatformPath =
        "file:///" + Application.temporaryCachePath + "/";

    //资源写入的路径
    static public readonly string WritePlatformPath =
        Application.temporaryCachePath + "/";

    //streamingAssetss的路径
    public static readonly string PathURL =
    #if UNITY_ANDROID && !UNITY_EDITOR
        "jar:file://" + Application.dataPath + "!/assets/";
    #elif UNITY_IPHONE
        "file:///" + Application.streamingAssetsPath+"/";
    #elif UNITY_STANDALONE_WIN || UNITY_EDITOR
        "file:///" + Application.dataPath + "/StreamingAssets/";
    #else
        string.Empty;
    #endif
    #endregion
    #region Function
    public static string GetPathName(GameObject go)
    {
        return "";
    }
    #endregion


}
