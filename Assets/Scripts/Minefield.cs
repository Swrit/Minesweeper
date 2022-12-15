using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Minefield : MonoBehaviour
{

    int FieldWidth = 10;
    int FieldHeight = 10;
    int MinesNum = 20;
    float xOffset = 5;
    float yOffset = 5;
    //int flagsNum = 0;
    int MinesLeft;
    int difficulty;

    public bool victory;
    public bool loss;

    [SerializeField]
    GameObject prefabCell;
    [SerializeField]
    GameObject prefabFirework;

    GameObject Fireworks;

    List<Cell> cells = new List<Cell>();
    List<GameObject> cellObjs = new List<GameObject>();
    List<int> victoryList = new List<int>();

    MineCounter mineCounter;
    Timer timeCounter;
    //CamShake camShake;


    void Awake()
    {
        mineCounter = GameObject.Find("MineCounter").GetComponent<MineCounter>();
        timeCounter = GameObject.Find("Timer").GetComponent<Timer>();
        //camShake = GameObject.Find("Main Camera").GetComponent<CamShake>();

    }
    // Start is called before the first frame update
    void Start()
    {
        
        victory = false;
        loss = false;
        Debug.Log("Get difficulty based parameters");
        difficulty = PlayerPrefs.GetInt("Difficulty");
        GetStartParameters();
        Debug.Log("start");
        GenerateCellObjects(FieldWidth, FieldHeight);
        Debug.Log("objects generated");
        GenerateMines(MinesNum, FieldWidth, FieldHeight);
        Debug.Log("mines generated");
        GenerateValues();
        MinesLeft = MinesNum;
        mineCounter.UpdateText(MinesLeft);
        timeCounter.StartTimer();

    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    void GetStartParameters()
    {
        switch (difficulty)
        {
            case 1:
                FieldWidth = 10;
                FieldHeight = 10;
                MinesNum = 10;
                xOffset = -2.25f;
                yOffset = 4.5f;
                break;
            case 2:
                FieldWidth = 16;
                FieldHeight = 16;
                MinesNum = 40;
                xOffset = 0.75f;
                yOffset = 7.5f;
                break;
            case 3:
                FieldWidth = 24;
                FieldHeight = 20;
                MinesNum = 99;
                xOffset = 4.75f;
                yOffset = 9.5f;
                break;
        }
    }    

    void GenerateCellObjects(int width, int height)
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Vector3 Location = new Vector3(j - xOffset, i - yOffset, -Camera.main.transform.position.z);
                cellObjs.Add(Instantiate(prefabCell) as GameObject);
                cellObjs.Last().transform.position = Location;
                cells.Add(cellObjs.Last().GetComponent<Cell>());
                cells.Last().surroundings = GetSurroundings(j, i, width, height);
                cells.Last().index = XYtoIndex(j, i, width);
            }
        }
    }

    List<int> GetSurroundings(int x, int y, int width, int height)
    {
        List<int> result = new List<int>();
        List<int> scopeX = new List<int>();
        scopeX.Add(x);
        if (x > 0)
        {
            scopeX.Add(x - 1);
        }
        if (x < (width - 1))
        {
            scopeX.Add(x + 1);
        }
        List<int> scopeY = new List<int>();
        scopeY.Add(y);
        if (y > 0)
        {
            scopeY.Add(y - 1);
        }
        if (y < (height - 1))
        {
            scopeY.Add(y + 1);
        }
        foreach (int i in scopeY)
        {
            foreach (int j in scopeX)
            {
                if (!((i == y) & (j == x)))
                {
                    result.Add(XYtoIndex(j, i, width));
                }
            }
        }
        return result;

    }

    int XYtoIndex(int x, int y, int width)
    {
        return ((y * width) + x);
    }

    void GenerateMines(int minenum, int width, int height)
    {
        int minectrl = 0;
        for (int i = 0; i < minenum; i++)
        {
            int place = Random.Range(0, cells.Count);
            while (!SuitableForMine(place))
            {
                place = Random.Range(0, cells.Count);
            }
            cells[place].value = -1;
            minectrl++;
        }
        Debug.Log("Mines laid: " + minectrl.ToString());
    }

    bool SuitableForMine(int index)
    {
        if ((cells[index].value == -1)||(CountSurroundingMines(index) == cells[index].surroundings.Count))
            return false;
        foreach (int ind in cells[index].surroundings)
            {
            if (CountSurroundingMines(ind) >= (cells[ind].surroundings.Count - 1))
                return false;
            }
        return true;
    }

    void GenerateValues()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            if (cells[i].value != -1)
            {
                cells[i].value = CountSurroundingMines(i);
                victoryList.Add(cells[i].index);
            }
        }
    }

    int CountSurroundingMines(int index)
    {
        int result = 0;
        foreach (int i in cells[index].surroundings)
        {
            if (cells[i].value == -1)
            {
                result++;
            }
        }
        return result;
    }

    int CountSurroundingFlags(int index)
    {
        int result = 0;
        foreach (int i in cells[index].surroundings)
        {
            if (cells[i].state == Cell.State.Flag)
            {
                result++;
            }
        }
        return result;
    }


    public void LeftClick(int index)
    {
        Debug.Log("Left click");
        Debug.Log("Id = " + cells[index].index.ToString() + "State = " + cells[index].state.ToString() + ", val = " + cells[index].value.ToString());
        Cell cell = cells[index];
        if (cell.state == Cell.State.Lock)
        {
            if (cell.value == -1)
            {
                cell.state = Cell.State.Explode;
                cell.UpdateSprite();
                Explode();
                //LOSS
                loss = true;
                BlockCells();
                Debug.Log("EXPLODE");
            }
            else
            {
                OpenCell(cell, cascade: true);
            }
        }
    }

    public void RightClick(int index)
    {
        Debug.Log("Right click");
        Debug.Log("Id = " + cells[index].index.ToString() + "State = " + cells[index].state.ToString() + ", val = " + cells[index].value.ToString());
        Cell cell = cells[index];
        if (cell.state == Cell.State.Lock)
        {
            cell.state = Cell.State.Flag;
            cell.UpdateSprite();
            MinesLeft--;
            mineCounter.UpdateText(MinesLeft);
        }
        else if (cell.state == Cell.State.Flag)
        {
            cell.state = Cell.State.Lock;
            cell.UpdateSprite();
            MinesLeft++;
            mineCounter.UpdateText(MinesLeft);
        }
    }

    public void MiddleClick(int index)
    {
        Cell cell = cells[index];
        if ((cell.state == Cell.State.Open) & (CountSurroundingFlags(index) == cell.value))
        {
            foreach (int i in cell.surroundings)
            {
                if (cells[i].state == Cell.State.Lock)
                {
                    LeftClick(i);
                }
                if ((victory) | (loss))
                {
                    break;
                }
            }
        }
        else
        {
            MiddleOff(index);
        }
    }

    public void MiddleOff(int index)
    {
        foreach (Cell checkCell in cells)
        {
            if (checkCell.externalHighlight)
            {
                checkCell.externalHighlight = false;
                checkCell.UpdateSprite();
            }
        }
    }

    public void MiddleHold(int index)
    {
        List<int> highlights = new List<int>();
        highlights.Add(index);
        foreach (int i in cells[index].surroundings)
        {
            highlights.Add(i);
        }
        //foreach (Cell checkCell in cells)
        //{
        //    if ((!highlights.Contains(checkCell.index)) & (checkCell.externalHighlight))
        //    {
        //        checkCell.externalHighlight = false;
        //        checkCell.UpdateSprite();
        //    }
        //}
        foreach (int i in highlights)
        {
            cells[i].externalHighlight = true;
            cells[i].UpdateSprite();
        }
    }

    void Explode()
    {
        foreach (Cell cell in cells)
        {
            if ((cell.state == Cell.State.Flag) | (cell.state == Cell.State.Lock))
            {
                OpenCell(cell, loss: true);
            }
        }
        timeCounter.StopTimer();
        mineCounter.UpdateText("TOO BAD");
        mineCounter.BlinkOn();
        //transform.localPosition = new Vector3(5, 5, -10);
        PlaySound("loss_jingle");
        PlaySound("explosion");
        CamShake.instance.Shake();
    }
    void OpenCell(Cell cell, bool cascade = false, bool loss = false)
    {
        cell.state = Cell.State.Open;
        cell.UpdateSprite();
        victoryList.Remove(cell.index);
        Debug.Log("Remain = " + victoryList.Count.ToString());
        if ((cell.value == 0) & cascade)
        {
            Cascade(cell);
        }
        if ((victoryList.Count == 0) && !loss)
        {
            Debug.Log("VICTORY");
            victory = true;
            BlockCells();
            Victory();
            //VICTORY
            //CleanSlate();
            
        }
    }
    void Cascade(Cell cell)
    {
        List<int> zeros = new List<int>();
        zeros.Add(cell.index);
        while (zeros.Count > 0)
        {
            foreach (int i in cells[zeros[0]].surroundings)
            {
                if (cells[i].state != Cell.State.Open)
                {
                    OpenCell(cells[i]);
                    if (cells[i].value == 0)
                    {
                        zeros.Add(i);
                    }
                }

            }
            zeros.RemoveAt(0);
        }

    }

    void Victory()
    {
        int timeResult = timeCounter.StopTimer();
        string hiScorePref = "HighScore" + difficulty.ToString();
        int hiScore = PlayerPrefs.GetInt(hiScorePref);
        if (hiScore>timeResult)
        {
            PlayerPrefs.SetInt(hiScorePref, timeResult);
            Debug.Log("New record: " + timeResult.ToString());
            mineCounter.UpdateText("RECORD!");
            PlaySound("record_jingle");
        }
        else
        {
            mineCounter.UpdateText("VICTORY");
            PlaySound("victory_jingle");
        }
        mineCounter.BlinkOn();
        Fireworks = Instantiate(prefabFirework) as GameObject;
        Fireworks.transform.position = new Vector3 (6.75f, 0, 0);
    }

    void BlockCells()
    {
        foreach (Cell cell in cells)
        {
            cell.blockInput = true;
        }
    }

    void PlaySound(string soundname)
    {
        SoundEngine.instance.Play(soundname);
    }

    void RestartBGM()
    {
        SoundEngine.instance.RestartBGM();
    }

    public void CleanSlate()
    {
        foreach (GameObject cellObj in cellObjs)
        {
            Destroy(cellObj);
        }
        cells = new List<Cell>();
        cellObjs = new List<GameObject>();
        victoryList = new List<int>();
        mineCounter.BlinkOff();
        if (Fireworks)
        {
            Destroy(Fireworks);
        }
        RestartBGM();
        Start();
        //SceneManager.LoadScene("MinesweeperGame");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }


}