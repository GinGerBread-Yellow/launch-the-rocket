using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform _transform;
    private void FixedUpdate()
    {
        //Debug.Log("send successfully");
        SendInputToServer();
    }
    private void Update()
    {
        //Debug.Log(_transform.position);
    }
    private void SendInputToServer()
    {
        /*
        bool[] _inputs = new bool[]
        {
            Input.GetKey(KeyCode.W),
            Input.GetKey(KeyCode.S),
            Input.GetKey(KeyCode.A),
            Input.GetKey(KeyCode.D),
        };
        */
        Vector2 inputs = new Vector2(_transform.position.x, _transform.position.y);
        if(Client.instance.myId % 2 == 1)
        {
            ClientSend.PlayerMovement(inputs,(int)actionAni.instance.cs);
        }
        else
        {
            //Debug.Log($"isUsinggun={(playerMovement.isUsingGun() ? 16 : 0)}");
            ClientSend.PlayerMovement(inputs, (int)BlueAni.instance.cs);
        }
    }
}
