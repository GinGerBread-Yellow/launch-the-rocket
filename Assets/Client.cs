using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;

public class Client : MonoBehaviour
{
    public static Client instance;
    public static int dataBufferSize = 4096;

    public string ip = "10.0.2.15";
    public int port = 26950;
    public int myId = 0;
    public TCP tcp;
    public UDP udp;
    private bool isConnected = false;
    private delegate void PacketHandler(Packet _packet);
    private static Dictionary<int, PacketHandler> packetHandlers;

    /*
    public Client(string IPaddr)
    {
        ip = IPaddr;
    }
    */

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("this is Client class!");
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        tcp = new TCP();
        udp = new UDP();
    }

    private void OnApplicationQuit()
    {
        Disconnect();
    }
    public void ConnectToServer(string IPaddr)
    {
        InitializeClientData();
        isConnected = true;
        instance.ip = IPaddr;
        tcp.Connect(IPaddr);
        udp.Connect(26950, IPaddr);
    }

    public class TCP
    {
        public TcpClient socket;

        private NetworkStream stream;
        private Packet receivedData;
        private byte[] receiveBuffer;

        public void Connect(string IPaddr)
        {
            socket = new TcpClient
            {
                ReceiveBufferSize = dataBufferSize,
                SendBufferSize = dataBufferSize
            };
            receiveBuffer = new byte[dataBufferSize];
            
            socket.BeginConnect(IPaddr, instance.port, ConnectCallback, socket);
        }

        private void ConnectCallback(IAsyncResult _result)
        {
            socket.EndConnect(_result);

            if (!socket.Connected)
            {
                return;
            }

            stream = socket.GetStream();

            receivedData = new Packet();

            stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
        }

        public void SendData(Packet _packet)
        {
            try
            {
                if (socket != null)
                {
                    //Debug.Log("whyyy");
                    stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
                }
                else
                {
                    //Debug.Log("socket gg");
                }
            }
            catch (Exception _ex)
            {
                Debug.Log($"Error sending data to server via TCP: {_ex}");
            }
        }

        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                int _byteLength = stream.EndRead(_result);
                if (_byteLength <= 0)
                {
                    // TODO: disconnect
                    instance.Disconnect();
                    return;
                }

                byte[] _data = new byte[_byteLength];
                Array.Copy(receiveBuffer, _data, _byteLength);

                receivedData.Reset(HandleData(_data));
                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            }
            catch
            {
                // TODO: disconnect
                Disconnect();
            }
        }

        private bool HandleData(byte[] _data)
        {
            int _packetLength = 0;

            receivedData.SetBytes(_data);

            if (receivedData.UnreadLength() >= 4)
            {
                _packetLength = receivedData.ReadInt();
                if (_packetLength <= 0)
                {
                    return true;
                }
            }

            while (_packetLength > 0 && _packetLength <= receivedData.UnreadLength())
            {
                byte[] _packetBytes = receivedData.ReadBytes(_packetLength);
                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (Packet _packet = new Packet(_packetBytes))
                    {
                        int _packetId = _packet.ReadInt();
                        packetHandlers[_packetId](_packet);
                    }
                });

                _packetLength = 0;
                if (receivedData.UnreadLength() >= 4)
                {
                    _packetLength = receivedData.ReadInt();
                    if (_packetLength <= 0)
                    {
                        return true;
                    }
                }
            }

            if (_packetLength <= 1)
            {
                return true;
            }

            return false;
        }

        private void Disconnect()
        {
            instance.Disconnect();
            stream = null;
            receiveBuffer = null;
            receivedData = null;
            socket = null;
        }
    }

    
    public class UDP
    {
        public UdpClient socket;
        public IPEndPoint endPoint;

        public UDP()
        {
            // endPoint = new IPEndPoint(IPAddress.Parse(instance.ip), instance.port);
        }

        public void Connect(int _localPort, string ip)
        {
            socket = new UdpClient();
            endPoint = new IPEndPoint(IPAddress.Parse(ip), instance.port);
            socket.Connect(endPoint);
            socket.BeginReceive(ReceiveCallback, null);

            using (Packet _packet = new Packet())
            {
                SendData(_packet);
            }
        }

        public void SendData(Packet _packet)
        {
            try
            {
                _packet.InsertInt(instance.myId);
                if (socket != null)
                {
                    socket.BeginSend(_packet.ToArray(), _packet.Length(), null, null);
                }
            }
            catch (Exception _ex)
            {
                Debug.Log($"Error sending data to server via UDP: {_ex}");
            }
        }

        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                byte[] _data = socket.EndReceive(_result, ref endPoint);
                socket.BeginReceive(ReceiveCallback, null);

                if (_data.Length < 4)
                {
                    // TODO: disconnect
                    instance.Disconnect();

                    return;
                }

                HandleData(_data);
            }
            catch
            {
                // TODO: disconnect
                Disconnect();
            }
        }

        private void HandleData(byte[] _data)
        {
            using (Packet _packet = new Packet(_data))
            {
                int _packetLength = _packet.ReadInt();
                _data = _packet.ReadBytes(_packetLength);
            }

            ThreadManager.ExecuteOnMainThread(() =>
            {
                using (Packet _packet = new Packet(_data))
                {
                    int _packetId = _packet.ReadInt();
                    packetHandlers[_packetId](_packet);
                }
            });
        }

        private void Disconnect()
        {
            instance.Disconnect();
            endPoint = null;
            socket = null;
            
        }
    }
    
    

    private void InitializeClientData()
    {
        packetHandlers = new Dictionary<int, PacketHandler>()
        {
            { (int)ServerPackets.welcome, ClientHandle.Welcome },
            { (int)ServerPackets.spawnPlayer, ClientHandle.SpawnPlayer },
            { (int)ServerPackets.playerPosition, ClientHandle.PlayerPosition },
            { (int)ServerPackets.updateResource, ClientHandle.UpdateProgressBar },
            { (int)ServerPackets.disconnect, ClientHandle.PlayerDisconnect },
            { (int)ServerPackets.ReceivedBomb, ClientHandle.RenderBomb },
            { (int)ServerPackets.ReceivedShot, ClientHandle.RenderShot },
            { (int)ServerPackets.ReceivedGunPos, ClientHandle.RenderGunPos }
            //{ (int)ServerPackets.curr_timeLeft, ClientHandle.curr_timeLeft },
        };
        Debug.Log("Initialized packets.");
    }

    public void Disconnect()
    {
        if (isConnected)
        {
            isConnected = false;
            tcp.socket.Close();
            udp.socket.Close();

            Debug.Log("disconnect...");
        }
    }
}
