using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI; //Button
using UnityEngine.SceneManagement; //SceneManager
using UnityEngine;
using System.Net;
using System.Net.Sockets;
#if Unity_Editor
using UnityEditor;
#endif

public class start : MonoBehaviour
{
    //public static Client client;
    public int sceneIndex = 1; //要載入的Scene
    public static start instance;
    //public InputField IPaddress;
    public InputField IPaddress;
    // public Client client;
    //private Socket client_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    //private byte[] rcv_buffer = new byte[2048];
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }
    /*
    public void Connect(string IP, int port)
    {
        try
        {
            client_socket.Connect(new IPEndPoint(IPAddress.Parse(IP), port));
            Debug.Log("Connected");
        }

        catch (SocketException ex)
        {
            Debug.Log(ex.Message);
        }
    }
    */
    public void ClickEvent()
    {
        string IPaddr = IPaddress.text;
        // client = new Client();
        Client.instance.ConnectToServer(IPaddr);
        //切換Scene
        Debug.Log("change scene...");
        SceneManager.LoadScene(sceneIndex);

    }

}

