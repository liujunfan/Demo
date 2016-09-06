using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;
using System.Text;
using System.Xml;
using System.Security.Cryptography;
using System.Xml.Serialization;
using System.Reflection;
public class GameData
{
    public string playName;
    public byte btIndex;
    public GameData()
    {
    }
    public GameData(string name, byte index)
    {
        playName = name;
        btIndex = index;
    }
}

public class GameDataManager : MonoBehaviour
{
	public static GameDataManager Instance;
	public bool iscrypt = false;
    private string dataFileName = "data.text";
    private XmlSaver xs = new XmlSaver();
    public GameData[] gameDatas;
	Type type;
    void Awake()
    {
		Instance = this;
        gameDatas = new GameData[2]
        {
            new GameData("haha", 1),
            new GameData("hehe", 2)
        };  
		type = gameDatas.GetType();

        //Load();
    }
    void OnGUI()
    {
        if (GUILayout.Button("Save"))
        {
            gameDatas[0].btIndex = 10;
            gameDatas[0].playName = "pic";
            Save();
        }
        if (GUILayout.Button("Load"))
        {
            Load();
        }
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
        GameData[] gameDataFromXML = xs.DeserializeObject(assetsWWW.text, type) as GameData[];
        foreach (var item in gameDataFromXML)
        {
            Debug.Log(item.playName + "," + item.btIndex);
        }
        assetsWWW.Dispose();
        assetsWWW = null;
    }
    /// <summary>
    /// 读取
    /// </summary>
    public void Load()
    {
        string gameDataFile = Utility.PathURL + dataFileName;
        StartCoroutine(Load(gameDataFile));
    }
    /// <summary>
    /// 保存
    /// </summary>
    public void Save()
    {
        string gameDataFile = Application.streamingAssetsPath + "/" + dataFileName;
		string dataString = xs.SerializeObject(gameDatas, type);
        xs.CreateXML(gameDataFile, dataString);
    }
    /// <summary>
    /// 获取路径
    /// </summary>
    /// <returns></returns>
    private static string GetDataPath()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            string path = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
            path = path.Substring(0, path.LastIndexOf('/'));
            return path + "/Documents";
        }
        else
            return Application.dataPath;
    }
}
public class XmlSaver
{
    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="toE"></param>
    /// <returns></returns>
    public string Encrypt(string toE)
    {
        byte[] keyArray = UTF8Encoding.UTF8.GetBytes("52013141592613145201314159261314");
        RijndaelManaged rDel = new RijndaelManaged();
        rDel.Key = keyArray;
        rDel.Mode = CipherMode.ECB;
        rDel.Padding = PaddingMode.PKCS7;
        ICryptoTransform cTransform = rDel.CreateEncryptor();
        byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toE);
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }
    /// <summary>
    /// 解密
    /// </summary>
    /// <param name="toD"></param>
    /// <returns></returns>
    public string Decrypt(string toD)
    {
        byte[] keyArray = UTF8Encoding.UTF8.GetBytes("52013141592613145201314159261314");
        RijndaelManaged rDel = new RijndaelManaged();
        rDel.Key = keyArray;
        rDel.Mode = CipherMode.ECB;
        rDel.Padding = PaddingMode.PKCS7;
        ICryptoTransform cTransform = rDel.CreateEncryptor();
        byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toD);
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        return UTF8Encoding.UTF8.GetString(resultArray);
    }
    /// <summary>
    /// 序列化
    /// </summary>
    /// <param name="pObject"></param>
    /// <param name="ty"></param>
    /// <returns></returns>
    public string SerializeObject(object pObject, System.Type ty)
    {
        MemoryStream memoryStream = new MemoryStream();
        XmlSerializer xs = new XmlSerializer(ty);
        XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
        xs.Serialize(xmlTextWriter, pObject);
        memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
        return UTF8ByteArrayToString(memoryStream.ToArray());
    }
    /// <summary>
    /// 反序列化
    /// </summary>
    /// <param name="pXmlizedString"></param>
    /// <param name="ty"></param>
    /// <returns></returns>
    public object DeserializeObject(string pXmlizedString, System.Type ty)
    {
        XmlSerializer xs = new XmlSerializer(ty);
        MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
        XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
        return xs.Deserialize(memoryStream);
    }
    /// <summary>
    /// 创建XML
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="thisData"></param>
    public void CreateXML(string fileName, string thisData)
    {
		string xxx = GameDataManager.Instance.iscrypt ? Encrypt(thisData) : thisData ;
        StreamWriter writer;
        writer = File.CreateText(fileName);
		writer.Write(xxx);
        writer.Close();
    }
    /// <summary>
    /// 读取XML
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public string LoadXML(string fileName)
    {
        StreamReader sReader = File.OpenText(fileName);
        string dataString = sReader.ReadToEnd();
        sReader.Close();
		return GameDataManager.Instance.iscrypt ? Decrypt(dataString) : dataString;
    }
    /// <summary>
    /// 路径是否存在
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public bool hasFile(string fileName)
    {
        return File.Exists(fileName);
    }
    /// <summary>
    /// byte[]转string
    /// </summary>
    /// <param name="characters"></param>
    /// <returns></returns>
    public string UTF8ByteArrayToString(byte[] characters)
    {
        UTF8Encoding encoding = new UTF8Encoding();
        string constructedString = encoding.GetString(characters);
        return (constructedString);
    }
    /// <summary>
    /// string转byte[]
    /// </summary>
    /// <param name="pXmlString"></param>
    /// <returns></returns>
    public byte[] StringToUTF8ByteArray(String pXmlString)
    {
        UTF8Encoding encoding = new UTF8Encoding();
        byte[] byteArray = encoding.GetBytes(pXmlString);
        return byteArray;
    }
}