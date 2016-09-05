using UnityEngine;
using UnityEditor;
using System.Collections;

public class Tools
{
    /// <summary>
    /// 返回选中下所有Object数组
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <returns></returns>
    public static T[] SelectAllObj<T>() where T : Object
    {
        //Unfiltered = 0, 			返回整个选择
		//TopLevel = 1,				仅返回最顶层选择的变化，其他选择的子对象将被过滤出
        //Deep = 2,					返回选择和所有选择的子变化
        //ExcludePrefab = 4,		从选择中排除任何预制
        //OnlyUserModifiable = 8,	
        //Editable = 8,				排除任何对象不得修改
        //Assets = 16,				仅返回资源目录中的资源对象
		//DeepAssets = 32,			如果选择包含文件夹，也包含所有资源和子目录
        object[] objs = Selection.GetFiltered(typeof(T), SelectionMode.Deep);
		if (objs.Length == 0)
		{
            Debug.Log("not find type");
			return null;
		}
        T[] ts = new T[objs.Length];
        int index = 0;
        foreach (var item in objs)
        {
            ts[index++] = item as T;
        }
        return ts;
    }
    [MenuItem("Tools/Test")]
    static void test()
    {
        MeshFilter[] ms = SelectAllObj<MeshFilter>();
        foreach (var item in ms)
        {
            Debug.Log(item.name);
        }
    }
	/// <summary>
	/// 是否激活AddChild
	/// </summary>
	[MenuItem ("Tools/AddChild", true)]  
	static bool IsSelect()
	{
		return Selection.activeTransform;
	}
    /// <summary>
    /// 添加子对象
    /// </summary>
	[MenuItem ("Tools/AddChild")]  
	static void AddChild()  
	{  
		Transform[] transforms = Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable);  
		foreach(Transform transform in transforms)  
		{  
			GameObject newChild = new GameObject("_Child");  
			newChild.transform.parent = transform;  
		}  
	} 
}
