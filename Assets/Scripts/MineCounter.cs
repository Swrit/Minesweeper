using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineCounter : MonoBehaviour
{
    Text text;
    bool blink = false;
    bool blinkColor = false;
    float blinkTime = 0;

    // Start is called before the first frame update
    void Awake()
    {
        text = this.GetComponent<Text>();
    }

    private void Update()
    {
        if (blink)
        {
            blinkTime += Time.deltaTime;
            if (blinkTime >= 0.75f)
            {
                if (blinkColor)
                {
                    text.color = Color.white;
                    blinkColor = false;
                }
                else
                {
                    text.color = Color.red;
                    blinkColor = true;
                }
                blinkTime = 0;
            }
        }
    }



    public void UpdateText(int num)
    {
        text.text = num.ToString();
    }

    public void UpdateText(string newtext)
    {
        text.text = newtext;
    }

    public void BlinkOn()
    {
        blink = true;
    }

    public void BlinkOff()
    {
        blink = false;
        blinkColor = false;
        blinkTime = 0;
        text.color = Color.white;
    }

}
