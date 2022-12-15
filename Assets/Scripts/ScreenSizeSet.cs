using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ScreenSizeSet : MonoBehaviour
{
    ToggleGroup toggleGroup;

    public Texture2D shovel_x1;
    public Texture2D shovel_x2;

    bool ready = false;

    private void Awake()
    {
        InitialScreenSize();
        toggleGroup = GetComponent<ToggleGroup>();
        CheckToggle(PlayerPrefs.GetInt("ScreenSize"));
    }
    // Start is called before the first frame update
    void Start()
    {
        ready = true;
        SetScreenSize();
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    void InitialScreenSize()
    {
        if (!PlayerPrefs.HasKey("ScreenSize"))
        {
            PlayerPrefs.SetInt("ScreenSize", 1);
        }
    }

    void CheckToggle(int sizeSetting)
    {
        toggleGroup.SetAllTogglesOff();
        string togg = "Toggle -x" + sizeSetting.ToString();
        GameObject.Find(togg).GetComponent<Toggle>().isOn = true;
    }

    public void ApplySize()
    {
        if (ready)
        {
            Toggle toggle = toggleGroup.ActiveToggles().FirstOrDefault();
            int a;
            switch (toggle.name)
            {
                case "Toggle -x1":
                    a = 1;
                    break;
                case "Toggle -x2":
                    a = 2;
                    break;
                default:
                    a = 1;
                    break;
            }
            PlayerPrefs.SetInt("ScreenSize", a);
            SetScreenSize();
        }
        
    }

    void SetScreenSize()
    {
        switch (PlayerPrefs.GetInt("ScreenSize"))
        {   
            case 2:
                Screen.SetResolution(1280, 720, false);
                //FindObjectOfType<Camera>().GetComponent<Camera>().orthographicSize = 22.5f;
                Cursor.SetCursor(shovel_x2, Vector2.zero, CursorMode.Auto);
                break;
            default:
                Screen.SetResolution(640, 360, false);
                //FindObjectOfType<Camera>().GetComponent<Camera>().orthographicSize = 11.25f;
                Cursor.SetCursor(shovel_x1, Vector2.zero, CursorMode.Auto);
                break;

        }
    }

}
