using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
        //Debug.Log("haha");
    }

    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.udp.SendData(_packet);
        //Debug.Log("really send");
    }

    #region Packets
    public static void WelcomeReceived()
    {
        //Debug.Log("hihihihi");
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write("success");

            SendTCPData(_packet);
        }
    }
    
    /*
    public static void PlayerMovement(float[] _inputs)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerMovement))
        {
            _packet.Write(_inputs.Length);
            foreach (float _input in _inputs)
            {
                _packet.Write(_input);
            }
            _packet.Write(GameManager.players[Client.instance.myId].transform.rotation);

            SendUDPData(_packet);
        }
    }
    */

    public static void PlayerMovement(Vector2 _inputs, int status = 0)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerMovement))
        {
            _packet.Write(_inputs);
            _packet.Write(status);
            SendUDPData(_packet);
        }
    }

    public static void sendResource(int _inputs)
    {
        using (Packet _packet = new Packet((int)ClientPackets.sendResource))
        {
            _packet.Write(_inputs);
            SendTCPData(_packet);
        }
    }

    public static void sendBomb(Vector2 _inputs)
    {
        using (Packet _packet = new Packet((int)ClientPackets.sendBomb))
        {
            _packet.Write(_inputs);
            SendTCPData(_packet);
        }
    }

    public static void gunPos(Vector3 diff, float _rotZ)
    {
        using (Packet _packet = new Packet((int)ClientPackets.gunPos))
        {
            _packet.Write(diff);
            _packet.Write(_rotZ);
            SendUDPData(_packet);
        }
    }

    public static void fireShot(Vector2 bullet_pos, Quaternion _inputs)
    {
        using (Packet _packet = new Packet((int)ClientPackets.fireShot))
        {
            _packet.Write(bullet_pos);
            _packet.Write(_inputs);
            SendTCPData(_packet);
        }
    }
    #endregion
}
