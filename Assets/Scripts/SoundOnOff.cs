using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SoundOnOff : MonoBehaviour
{
    ToggleGroup toggleGroup;
    Text text;
    bool isToggle = false;
    bool isButton = false;
    int buttonState = 0;
    bool ready = false;


    private void Awake()
    {
        InitialSoundSetting();
        if (TryGetComponent<ToggleGroup>(out var tg))
        {
            toggleGroup = tg;
            isToggle = true;
            CheckToggle(PlayerPrefs.GetInt("SoundOnOff"));

        }
        if (TryGetComponent<Button>(out var b))
        {
            text = this.GetComponentInChildren<Text>();
            isButton = true;
            if (!(buttonState==PlayerPrefs.GetInt("SoundOnOff")))
            {
                ButtonChangeState();
            }

        }
        //toggleGroup = GetComponent<ToggleGroup>();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        ready = true;
        SetSound();
    }

    // Update is called once per frame
    //void Update()
    //{

    //}

    void InitialSoundSetting()
    {
        if (!PlayerPrefs.HasKey("SoundOnOff"))
        {
            PlayerPrefs.SetInt("SoundOnOff", 1);
        }
    }

    void CheckToggle(int sizeSetting)
    {
        toggleGroup.SetAllTogglesOff();
        string togg = "Toggle -Sound" + sizeSetting.ToString();
        GameObject.Find(togg).GetComponent<Toggle>().isOn = true;
    }

    public void ApplySound()
    {
        if (ready)
        {
            int a = 1;
            if (isToggle)
            {
                Toggle toggle = toggleGroup.ActiveToggles().FirstOrDefault();
                switch (toggle.name)
                {
                    case "Toggle -Sound1":
                        a = 1;
                        break;
                    case "Toggle -Sound0":
                        a = 0;
                        break;
                    default:
                        a = 1;
                        break;
                }
            }
            if (isButton)
            {
                a = buttonState;
            }
            PlayerPrefs.SetInt("SoundOnOff", a);
            SetSound();
        }
        
    }

    void ButtonChangeState()
    {
        if (buttonState == 1)
        {
            buttonState = 0;
            text.text = "Sound: off";

        }
        else
        {
            buttonState = 1;
            text.text = "Sound: on";
        }
    }

    public void ButtonClick()
    {
        ButtonChangeState();
        ApplySound();
    }

    void SetSound()
    {
        switch (PlayerPrefs.GetInt("SoundOnOff"))
        {
            case 0:
                AudioListener.volume = 0;
                break;
            default:
                AudioListener.volume = 0.5f;
                break;

        }
    }

}
