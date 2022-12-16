using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        //string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();
        //Debug.Log(time);
        Debug.Log($"Welcome from server: {_myId}");
        Client.instance.myId = _myId;
        ClientSend.WelcomeReceived();

        //Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void SpawnPlayer(Packet _packet)
    {
        int total_players = _packet.ReadInt();
        float time = _packet.ReadFloat();
        float RTT = _packet.ReadFloat();
        int water = _packet.ReadInt();
        int coal = _packet.ReadInt();
        int metal = _packet.ReadInt();
        int me = Client.instance.myId;

        for (int i = 0; i < total_players; i++)
        {
            int _id = _packet.ReadInt(); //id
            Vector2 _position = _packet.ReadVector2(); //pos
            int status = _packet.ReadInt();
            Debug.Log("currid&pos= " + _id);
            GameManager.instance.SpawnPlayer(_id, $"player{_id}", _position, status);
            if (_id == me)
            {
                countdownTimer.instance.update_time(time - RTT);
                if (_id % 2 == 1)
                {
                    actionAni.instance.updateResource(water, coal, metal);
                }
                else
                {
                    BlueAni.instance.updateResource(water, coal, metal);
                }
            }
        }
    }

    public static void PlayerPosition(Packet _packet)
    {
        int total_players = _packet.ReadInt();
        Debug.Log("total =" + total_players);
        Debug.Log("who is me =" + Client.instance.myId);
        for(int i = 0; i < total_players; i++)
        {
            int _id = _packet.ReadInt();
            Vector2 _position = _packet.ReadVector2();
            int status = _packet.ReadInt();
            Debug.Log("currid =" + _id);
            if (_id != Client.instance.myId)
            {       
                float default_z = 0f;
                Vector3 vector3 = new Vector3(_position.x, _position.y, default_z);
                GameManager.players[_id].pos = vector3;
                GameManager.players[_id].status = status;
            }
        }
    }

    public static void UpdateProgressBar(Packet _packet)
    {
        int _id = _packet.ReadInt();
        int _water = _packet.ReadInt();
        int _coal = _packet.ReadInt();
        int _metal = _packet.ReadInt();
        if(Client.instance.myId % 2 == 1)
        {
            actionAni.instance.updateResource(_water, _coal, _metal);
        }
        else
        {
            BlueAni.instance.updateResource(_water, _coal, _metal);
        }
        GameManager.players[_id].GetComponent<Animator>().SetBool("develop", false);
    }

    public static void RenderBomb(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector2 bombPosition = _packet.ReadVector2();
        GameManager.players[_id].GetComponent<droppingBomb_team>().RenderOtherBomb(bombPosition);
    }

    public static void RenderGunPos(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 diff = _packet.ReadVector3();
        float _rotz = _packet.ReadFloat();
        //GameManager.players[_id].GetComponentInChildren<Weapon>()._id = _id;
        Weapon obj = GameManager.players[_id].GetComponentInChildren<Weapon>();
        if (obj != null)
        {
            GameManager.players[_id].GetComponentInChildren<Weapon>()._difference = diff;
            GameManager.players[_id].GetComponentInChildren<Weapon>()._rotZ = _rotz;
        } 
        else
        {
            GameManager.players[_id].status = 7;
        }
        
        //GameManager.players[_id].GetComponentInChildren<Weapon>().isMygun = false;
    }

    public static void RenderShot(Packet _packet)
    {
        Vector2 _position = _packet.ReadVector2();
        float default_z = 0f;
        Vector3 firePosition = new Vector3(_position.x, _position.y, default_z);
        Quaternion temp_quat = _packet.ReadQuaternion();

        Weapon_team.instance.RenderShot(firePosition, temp_quat);
    }



    public static void PlayerDisconnect(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Debug.Log($"player{_id} is disconnect...");
        GameManager.instance.disconnect(_id);
    }

    /*
    public static void curr_timeLeft(Packet _packet)
    {
        float timeLeft = _packet.ReadFloat();
        Debug.Log(timeLeft);
        countdownTimer.instance.timeRemaining = timeLeft;
    }
    */

    /*
    public static void PlayerRotation(Packet _packet)
    {　　
        int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.players[_id].transform.rotation = _rotation;
    }
    */
}
