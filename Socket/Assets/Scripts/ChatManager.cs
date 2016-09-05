using UnityEngine;
using System.Collections;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class ChatManager : MonoBehaviour 
{
	public string IP = "127.0.0.1";
	public int port = 7788;
	public UILabel text;
	public UIInput input;
	public GameObject sendButton;
	private Socket clientSocket;
	private Thread t;
	private string str;

	void Start()
	{
		UIEventListener.Get (sendButton).onClick = OnSendButtonClick;
		ConnectToServer();
		t = new Thread(ReceiveMes);
		t.Start();
	}
	void Update()
	{
		if (str != null && !string.IsNullOrEmpty(str)) 
		{
			text.text = str;
			Debug.Log(str);
			str = string.Empty;
		}
	}
	void ConnectToServer()
	{
		clientSocket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		clientSocket.Connect (new IPEndPoint (IPAddress.Parse (IP), port));
	}
	void ReceiveMes()
	{
		while(true)
		{
			byte[] data = new byte[1024];
			int length = clientSocket.Receive(data);
			string message = Encoding.UTF8.GetString(data, 0, length);
			str = message;
		}
	}
	private void SendMes(string message)
	{
		byte[] data = Encoding.UTF8.GetBytes (message);
		clientSocket.Send (data);
	}
	private void OnSendButtonClick(GameObject go)
	{
		SendMes (input.value);
		input.value = string.Empty;
	}
	void OnDestroy()
	{
		clientSocket.Shutdown (SocketShutdown.Both);
		clientSocket.Close ();
	}
}
