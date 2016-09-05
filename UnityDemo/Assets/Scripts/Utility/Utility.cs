using UnityEngine;
using System.Collections;
using System.IO;

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
    #region Members
    static public GameObject TempObj;
    #endregion
    #region Function
    public static string GetPathName(GameObject go)
    {
        if (go == null)
        {
            return string.Empty;
        }
        if (go.transform.parent != null)
        {
            return GetPathName(go.transform.parent.gameObject) + "/" + go.name;
        }else
            return go.name;
    }

    public static GameObject GetObj(GameObject objParent, string strPath)
    {
        if (string.IsNullOrEmpty(strPath))
        {
            return null;
        }
        GameObject go = null;
        if (objParent != null && objParent.transform != null)
        {
            go = objParent.transform.Find(strPath).gameObject;
        }
        else
            go = GameObject.Find(strPath);
        return go;
    }
    public static T GetCompt<T>(GameObject objParent, string strPath) where T : Component
    {
        T comp = default(T);
        GameObject go = GetObj(objParent, strPath);
        if (go != null)
        {
            comp = go.GetComponent<T>();
        }
        return comp;
    }
    static public T AddCompt<T>(GameObject parent) where T : Component
    {
        T comp = parent.GetComponent<T>();
        if (comp == null)
        {
            parent.AddComponent<T>();
            comp = parent.GetComponent<T>();
        }
        return comp;
    }
    #region NGUI
    static public UISprite AddSprite(GameObject go, UIAtlas atlas, string spriteName,int depth = 0, int width = 0, int height = 0)
    {
        UISpriteData sp = (atlas != null) ? atlas.GetSprite(spriteName) : null;
        UISprite sprite = AddCompt<UISprite>(go);
        SetWidget<UISprite>(ref sprite, depth, width, height);
        sprite.type = (sp == null || !sp.hasBorder) ? UISprite.Type.Simple : UISprite.Type.Sliced;
        sprite.atlas = atlas;
        sprite.spriteName = spriteName;
        if (width == 0 && height == 0)
            sprite.MakePixelPerfect();
        return sprite;
    }
    static void SetWidget<T>(ref T widget, int depth, int width, int height)where T : UIWidget
    {
        widget.depth = depth;
        widget.width = width;
        widget.height = height;
    }
    #endregion
    public static GameObject InstanceGameObject(Object obj)
    {
        return GameObject.Instantiate(obj) as GameObject;
    }
    static public GameObject InstanceGameObject(Object obj, Vector3 Position)
    {
        return GameObject.Instantiate(obj, Position, Quaternion.identity) as GameObject;
    }

    static public GameObject InstanceGameObject(Object obj, Vector3 Position, Quaternion rotation)
    {
        return GameObject.Instantiate(obj, Position, rotation) as GameObject;
    }
    static public GameObject InstanceGameObject(Transform parent, Object obj, Vector3 localPosition, Vector3 localEulerAngles, Vector3 localScale)
    {
        GameObject go = GameObject.Instantiate(obj) as GameObject;
        Transform t = go.transform;
        SetParent(parent, t);
        t.localPosition = localPosition;
        t.localEulerAngles = localEulerAngles;
        t.localScale = localScale;
        return go;
    }
    static public GameObject SetParent(Transform parent, Transform child)
    {
        child.parent = parent;
        return child.gameObject;
    }

    static public void SetChildLayer(Transform t, int layer)
	{
        Transform[] childs = t.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < childs.Length; i++)
        {
            childs[i].gameObject.layer = layer;
        }
	}
    static public void SetChildLayer(Transform t, string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        SetChildLayer(t, layer);
    }
    static bool Save(string fileName, byte[] bytes)
    {
        string path = WritePlatformPath + fileName;
        if (bytes == null)
        {
            if (File.Exists(path)) File.Delete(path);
            return true;
        }
        FileStream file = null;
        try
        {
            file = File.Create(path);
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.Message);
            return false;
        }
        file.Write(bytes, 0, bytes.Length);
        file.Close();
        return true;
    }
    static byte[] Load(string fileName)
    {
        string path = ReadPlatformPath + fileName;
        if (File.Exists(path))
        {
            return File.ReadAllBytes(path);
        }
        return null;
    }
    #endregion

}
