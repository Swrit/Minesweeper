using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class DifficultySelection : MonoBehaviour
{
    ToggleGroup toggleGroup;

    List<Text> hiScores = new List<Text>();

    private void Awake()
    {
        InitialDifficulty();
        hiScores.Add(GameObject.Find("HiScore - easy").GetComponent<Text>());
        hiScores.Add(GameObject.Find("HiScore - normal").GetComponent<Text>());
        hiScores.Add(GameObject.Find("HiScore - hard").GetComponent<Text>());
        toggleGroup = GetComponent<ToggleGroup>();
        CheckToggle(PlayerPrefs.GetInt("Difficulty"));
    }


    // Start is called before the first frame update
    void Start()
    {
        CheckToggle(PlayerPrefs.GetInt("Difficulty"));
        SetHiScores();
        
    }

    void InitialDifficulty()
    {
        if (!PlayerPrefs.HasKey("Difficulty"))
        {
            PlayerPrefs.SetInt("Difficulty", 1);
        }
    }

    void CheckToggle(int diffSetting)
    {
        toggleGroup.SetAllTogglesOff();
        string togg = "Toggle - diff" + diffSetting.ToString();
        GameObject.Find(togg).GetComponent<Toggle>().isOn = true;
    }

    void SetHiScores()
    {
        int i = 1;
        foreach(Text text in hiScores)
        {
            string pref = "HighScore" + i.ToString();
            if (!PlayerPrefs.HasKey(pref))
            {
                PlayerPrefs.SetInt(pref, 9999);
            }
            text.text = PlayerPrefs.GetInt(pref).ToString();
            i++;
        }
    }

    public void StartGame()
    {
        Toggle toggle = toggleGroup.ActiveToggles().FirstOrDefault();
        Debug.Log(toggle.name + " " + toggle.GetComponentInChildren<Text>().text);
        int a;
        switch (toggle.name)
        {
            case "Toggle - diff1":
                a = 1;
                break;
            case "Toggle - diff2":
                a = 2;
                break;
            case "Toggle - diff3":
                a = 3;
                break;
            default:
                a = 1;
                break;
        }
        PlayerPrefs.SetInt("Difficulty", a);
        SceneManager.LoadScene("MinesweeperGame");
    }

    public void ExitGame()
    {
        Debug.Log("Game Exit");
        Application.Quit();
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}




}
