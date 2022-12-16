using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();

    public GameObject teamCPrefab;
    public GameObject teamIPrefab;
    public GameObject teamateCPrefab;
    public GameObject teamateIPrefab;
    public GameObject rocket;

    public float default_z = -26.26636f;

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

    public void SpawnPlayer(int _id, string _username, Vector2 _position, int status)// Quaternion _rotation)
    {
        GameObject _player;
        float z = default_z;
        if (_id == Client.instance.myId)
        {
            if (_id % 2 != 0)
            {
                _player = Instantiate(teamCPrefab);
            }
            else
            {
                _player = Instantiate(teamIPrefab);
            }

            rocket.GetComponent<launchRocket>().progressBar = _player.GetComponentInChildren<Slider>();
        }
        else
        {
            if (_id % 2 != 0)
            {

                _player = Instantiate(teamateCPrefab);

            }
            else
            {
                _player = Instantiate(teamateIPrefab);

            }
            z = 0f;
            // _player.transform.position = new Vector2(_position.x, _position.y);
            //_player = Instantiate(playerPrefab, _position);//, _rotation);
        }
        
        _player.transform.position = new Vector3(_position.x, _position.y, 0f);
        _player.transform.rotation = new Quaternion(0, 0, 0, 0);
        _player.GetComponent<PlayerManager>().id = _id;
        _player.GetComponent<PlayerManager>().username = _username;
        _player.GetComponent<PlayerManager>().status = status;
        players.Add(_id, _player.GetComponent<PlayerManager>());
    }

    public void disconnect(int id)
    {
        Debug.Log($"destroy id{id}");
        Destroy(players[id].gameObject);
        players.Remove(id);
    }

}
