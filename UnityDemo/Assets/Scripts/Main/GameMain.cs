using UnityEngine;
using System.Collections;

public class GameMain : MonoBehaviour 
{
    public static GameMain Instance;
    GameObject UIRoot;
	void Awake()
    {
        Instance = this;
        Init();
    }
    void Init()
    {
        UIRoot = Utility.GetObj(null, "UI Root");
        Debug.Log(UIRoot);
        Debug.Log(Utility.GetCompt<Camera>(UIRoot, "Camera"));
        Utility.InstanceGameObject(Resources.Load("Cube"), new Vector3(10,10,10));
    }
}
