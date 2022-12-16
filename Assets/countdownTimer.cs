using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class countdownTimer : MonoBehaviour
{
    //public ClientHandle c_handle;

    public static countdownTimer instance;
    public float timeRemaining = 0;
    public bool timerIsRunning = false;
    public Text timeText;
    public float _time;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        timeRemaining = 300f;
        // Starts the timer automatically
        timerIsRunning = true;
    }

    public void update_time(float time)
    {    
        _time = time;
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (_time > 0)
            {
                _time -= Time.deltaTime;
                DisplayTime(_time);
            }
            else
            {
                _time = 0;
                timerIsRunning = false;
                Client.instance.Disconnect();
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }

}
