using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    Text text;
    bool timerActive = false;
    float currentTime;
    int seconds;


    // Start is called before the first frame update
    void Awake()
    {
        text = this.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timerActive)
        {
            currentTime = currentTime + Time.deltaTime;
            if (currentTime>9999f)
            {
                seconds = 9999;
            }
            else
            {
                seconds = (int)currentTime;
            }
            text.text = seconds.ToString();
        }
    }

    public void StartTimer()
    {
        currentTime = 0;
        seconds = 0;
        timerActive = true;
    }

    public int StopTimer()
    {
        timerActive = false;
        return seconds;
    }



}
