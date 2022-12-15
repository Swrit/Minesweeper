using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    //Sprites
    [SerializeField]
    Sprite spriteLock;
    [SerializeField]
    Sprite spriteMine;
    [SerializeField]
    Sprite spriteFlag;
    [SerializeField]
    Sprite spriteExplode;
    [SerializeField]
    Sprite spriteHigh;
    [SerializeField]
    List<Sprite> spritesVal;

    //State prop
    public enum State
    {
        Lock,
        Open,
        Flag,
        Explode
    }
    public State state = State.Lock;

    bool highlight = false;
    public bool externalHighlight = false;

    //GameObject's sprite renderer, assigned in Start
    SpriteRenderer sprite;

    //InputManager script, assigned in start
    //public Minefield minefield;
    public InputManager inputManager;
    public bool blockInput = false;

    //cell's index in the list, assigned at creation
    public int index;

    //Cell's value, -1=mine
    public int value = 0;

    //Surrounding cells indexes
    public List<int> surroundings = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        sprite = this.GetComponent<SpriteRenderer>();
        //minefield = FindObjectOfType<Camera>().GetComponent<Minefield>();
        inputManager = FindObjectOfType<Camera>().GetComponent<InputManager>();
    }

    // Update is called once per frame
    //void Update()
    //{

    //}

    //private void OnMouseOver()
    //{
    //    if (!(minefield.victory | minefield.loss))
    //    {
    //        if ((Input.GetMouseButton(2)) | Input.GetMouseButton(0) & Input.GetMouseButton(1))
    //        {
    //            minefield.MiddleHold(index);
    //        }
    //        else
    //        {
    //            if ((Input.GetMouseButton(0)) & (state == State.Lock))
    //            {
    //                highlight = true;
    //                UpdateSprite();
    //            }
    //        }
    //    }



    //    if ((Input.GetMouseButtonUp(2)) | ((Input.GetMouseButtonUp(0)) & (Input.GetMouseButtonUp(1))))
    //    {
    //        minefield.MiddleClick(index);
    //    }
    //    else
    //    {
    //        if (Input.GetMouseButtonUp(0))
    //        {
    //            minefield.LeftClick(index);
    //        }
    //        if (Input.GetMouseButtonUp(1))
    //        {
    //            minefield.RightClick(index);
    //        }
    //    }



    //}

    private void OnMouseOver()
    {
        if (!blockInput)
        {
            if ((Input.GetMouseButton(2)) | Input.GetMouseButton(0) & Input.GetMouseButton(1))
            {
                inputManager.MiddleHold(index);
            }
            else
            {
                if ((Input.GetMouseButton(0)) & (state == State.Lock))
                {
                    highlight = true;
                    UpdateSprite();
                }
                else
                {
                    highlight = false;
                    UpdatedSprite();
                }
            }

            if ((Input.GetMouseButtonUp(2)) | (Input.GetMouseButtonUp(0) & Input.GetMouseButtonUp(1)))
            {
                inputManager.MiddleClick(index);
            }
            else
            {
                if (Input.GetMouseButtonUp(0))
                {
                    inputManager.LeftClick(index);
                }
                if (Input.GetMouseButtonUp(1))
                {
                    inputManager.RightClick(index);
                }
            }

        }
    }


    private void OnMouseExit()
    {
        if (highlight)
        {
            highlight = false;
            UpdateSprite();
        }
            inputManager.MiddleOff(index);
    }


    //if ((Input.GetMouseButton(0)) & (state == State.Lock))
    //{
    //    highlight = true;
    //    UpdateSprite();
    //}
    //if (Input.GetMouseButton(2))
    //{
    //    minefield.MiddleHold(index);
    //}
    //if (Input.GetMouseButtonUp(0))
    //{
    //    minefield.LeftClick(index);
    //}
    //if (Input.GetMouseButtonUp(1))
    //{
    //    minefield.RightClick(index);
    //}
    //if (Input.GetMouseButtonUp(2))
    //{
    //    minefield.MiddleClick(index);
    //}

    //private void OnMouseExit()
    //{
    //    if (highlight)
    //    {
    //        highlight = false;
    //        UpdateSprite();
    //    }
    //    if (Input.GetMouseButton(2))
    //    {
    //        minefield.MiddleOff(index);
    //    }
    //}

    public void UpdateSprite()
    {
        sprite.sprite = UpdatedSprite();
    }

    Sprite UpdatedSprite()
    {
        switch (state)
        {
            case State.Lock:
                if (highlight | externalHighlight)
                {
                    return spriteHigh;
                }
                else
                {
                    return spriteLock;
                }
            case State.Flag:
                return spriteFlag;
            case State.Explode:
                return spriteExplode;
            case State.Open:
                if (value == -1)
                {
                    return spriteMine;
                }
                else
                {
                    return spritesVal[value];
                }
            default:
                return spriteLock;
        }
    }


}