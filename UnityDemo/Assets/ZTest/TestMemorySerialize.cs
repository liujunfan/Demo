using UnityEngine;
using System.Collections;
using System.IO;
public class TestMemorySerialize : MonoBehaviour 
{
    void Awake()
    {
    }
    void OnGUI()
    {
        if (GUILayout.Button("Import"))
        {
            string writePath = Application.streamingAssetsPath + "/TestMemorySerialize";

            WriteBytes(writePath);
        }
        if (GUILayout.Button("Export"))
        {
            string readPath = Utility.PathURL + "TestMemorySerialize";
            StartCoroutine(Load(readPath));
        }
    }
    void WriteBytes(string path)
    {
        CMemorySerialize memory = new CMemorySerialize();
        memory.SetFloat(this.transform.localPosition.x);
        memory.SetFloat(this.transform.localPosition.y);
        memory.SetFloat(this.transform.localPosition.z);
        File.WriteAllBytes(path, memory.GetBuffer());
    }
    void ReadBytes(byte[] bytes)
    {
        CMemorySerialize readMemory = new CMemorySerialize(bytes);
        float x = readMemory.GetFloat;
        float y = readMemory.GetFloat;
        float z = readMemory.GetFloat;
        Debug.Log(x + "," + y + "," + z);
    }

    public IEnumerator Load(string path)
    {
        WWW assetsWWW = new WWW(path);
        yield return assetsWWW;
        if (assetsWWW.error != null)
        {
            Debug.LogError(" WWW Server not Conection " + assetsWWW.url);
            yield break;
        }
        ReadBytes(assetsWWW.bytes);

        assetsWWW.Dispose();
        assetsWWW = null;
    }
}
