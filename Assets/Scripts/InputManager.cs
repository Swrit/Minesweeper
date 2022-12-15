using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputTimer
{
    public int index;
    public float time;
    public int waitfor;

    public InputTimer(int index, int waitfor, float time = 0)
    {
        this.index = index;
        this.waitfor = waitfor;
        this.time = time;
        Debug.Log(index.ToString() + " waits for " + waitfor.ToString());
    }
}

public class InputManager : MonoBehaviour
{

    public Minefield minefield;
    List<InputTimer> timers = new List<InputTimer>();

    // Start is called before the first frame update
    void Start()
    {
        minefield = FindObjectOfType<Camera>().GetComponent<Minefield>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = timers.Count-1; i>=0; i--)
        {
            timers[i].time += Time.deltaTime;
            if (timers[i].time > 0.1)
            {
                minefield.MiddleOff(timers[i].index);
                Debug.Log("Timer over: " + timers[i].index.ToString());
                timers.RemoveAt(i);
                
            }
        }
    }

    public void LeftClick(int index)
    {
        MainClick(index, 0, 1);
    }

    public void RightClick(int index)
    {
        MainClick(index, 1, 0);
    }

    public void MiddleClick(int index)
    {
        minefield.MiddleClick(index);
    }

    public void MiddleOff(int index)
    {
        minefield.MiddleOff(index);
    }

    public void MiddleHold(int index)
    {
        minefield.MiddleHold(index);
    }

    void MainClick(int index, int click, int other)
    {
        int timer = timers.FindIndex(t => t.index == index);
        if (timer>=0)
        {
            if (timers[timer].waitfor == click)
            {
                timers.RemoveAt(timer);
                minefield.MiddleClick(index);
            }
            else if (Input.GetMouseButton(other))
            {
                timers[timer].time = 0;
            }    
        }    
        else if (Input.GetMouseButton(other))
        {
            timers.Add(new InputTimer(index, other));
        }
        else if (click == 0)
        {
            minefield.LeftClick(index);
        }
        else if (click == 1)
        {
            minefield.RightClick(index);
        }

    }


}
