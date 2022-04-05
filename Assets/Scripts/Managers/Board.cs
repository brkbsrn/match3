using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
    public BoardPiece boardPiecePrefab;
    public int boardX = 8;
    public int boardY = 8;
    public float xStart = -3.5f;
    public float YStart = -2.5f;
    public BoardPiece[][] BoardData;


    public StoneScript stoneScriptPrefab;
    private Pool<StoneScript> stonePool;

    private void Start()
    {
        stonePool = new Pool<StoneScript>(stoneScriptPrefab, transform, boardY * boardX);
        CreateBoard();
        StartCoroutine(CreateStones());
        GameEvents.Instance.OnStoneSwitchRequested += OnStoneSwitchRequested;
    }

    private void OnDestroy()
    {
        GameEvents.Instance.OnStoneSwitchRequested -= OnStoneSwitchRequested;
    }

    //This function creates board by BoardX and BoardY
    private void CreateBoard()
    {
        BoardData = new BoardPiece[boardX][];
        for (int x = 0; x < boardX; x++)
        {
            BoardData[x] = new BoardPiece[boardY];
            for (int y = 0; y < boardY; y++)
            {
                BoardData[x][y] = Instantiate(boardPiecePrefab, new Vector2(xStart + x, YStart + y), Quaternion.identity);
                BoardData[x][y].name = x + "-" + y;
                BoardData[x][y].SetData(x, y);
            }
        }

        GameEvents.Instance.BoardCreated(BoardData);
    }

    
    private IEnumerator CreateStones()
    {
        for (int x = 0; x < boardX; x++)
        {
            for (int y = 0; y < boardY; y++)
            {
                CreateRandomStone(x, y, true);
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    //  stone embed without a match
    private void CreateRandomStone(int x, int y, bool checkDontMatch)
    {
        StoneScript instance = CreateStone(x, y);
        List<int> stoneTypes = new List<int>() { 0, 1, 2, 3 };
        stoneTypes.Shuffle();

        if (!checkDontMatch)
        {
            instance.SetStoneType(stoneTypes[0]);
            return;
        }

        for (int i = 0; i < stoneTypes.Count; i++)
        {
            instance.SetStoneType(stoneTypes[i]);
            if (!IsMatchExist())
            {
                break;
            }
        }        
    }

    //Create Stone
    private StoneScript CreateStone(int x, int y)
    {
        BoardPiece boardPiece = BoardData[x][y];
        StoneScript instance = stonePool.GetInstance();
        boardPiece.StoneSpawned(instance);

        return instance;
    }

    //Match Control
    private bool IsMatchExist()
    {
        for (int x = 0; x < boardX; x++)
        {
            for (int y = 0; y < boardY; y++)
            {
                BoardPiece piece = BoardData[x][y];
                if (piece.StonePiece == null)
                {
                    continue;
                }
                int stoneType = piece.StonePiece.stoneType;
                int yuMatches = 1;
                for (int yu = y + 1; yu < boardY; yu++)
                {
                    BoardPiece controlPiece = BoardData[x][yu];
                    if (controlPiece.StonePiece == null)
                    {
                        break;
                    }
                    if (controlPiece.StonePiece.stoneType == stoneType)
                    {
                        yuMatches++;
                    }
                    else
                    {
                        break;
                    }
                }
                if (yuMatches >= 3)
                {
                    return true;
                }

                int ydMatches = 1;
                for (int yd = y - 1; yd >= 0; yd--)
                {
                    BoardPiece controlPiece = BoardData[x][yd];
                    if (controlPiece.StonePiece == null)
                    {
                        break;
                    }
                    if (controlPiece.StonePiece.stoneType == stoneType)
                    {
                        ydMatches++;
                    }
                    else
                    {
                        break;
                    }
                }
                if (ydMatches >= 3)
                {
                    return true;
                }

                int xuMatches = 1;
                for (int xu = x + 1; xu < boardX; xu++)
                {
                    BoardPiece controlPiece = BoardData[xu][y];
                    if (controlPiece.StonePiece == null)
                    {
                        break;
                    }
                    if (controlPiece.StonePiece.stoneType == stoneType)
                    {
                        xuMatches++;
                    }
                    else
                    {
                        break;
                    }
                }
                if (xuMatches >= 3)
                {
                    return true;
                }

                int xdMatches = 1;
                for (int xd = x - 1; xd >= 0; xd--)
                {
                    BoardPiece controlPiece = BoardData[xd][y];
                    if (controlPiece.StonePiece == null)
                    {
                        break;
                    }
                    if (controlPiece.StonePiece.stoneType == stoneType)
                    {
                        xdMatches++;
                    }
                    else
                    {
                        break;
                    }

                }
                if (xdMatches >= 3)
                {
                    return true;
                }
            }
        }

        return false;
    }

    //Match List
    public List<BoardPiece> GetMatchingPieces()
    {
        List<BoardPiece> matchingStones = new List<BoardPiece>();
        List<BoardPiece> tempList = new List<BoardPiece>();
        for (int x = 0; x < boardX; x++)
        {
            for (int y = 0; y < boardY; y++)
            {

                BoardPiece piece = BoardData[x][y];
                if (piece.StonePiece == null)
                {
                    continue;
                }
                int stoneType = piece.StonePiece.stoneType;
                int yuMatches = 1;
                for (int yu = y + 1; yu < boardY; yu++)
                {
                    BoardPiece controlPiece = BoardData[x][yu];
                    if (controlPiece.StonePiece == null)
                    {
                        break;
                    }
                    if (controlPiece.StonePiece.stoneType == stoneType)
                    {
                        tempList.Add(controlPiece);
                        yuMatches++;
                    }
                    else
                    {
                        break;
                    }
                }
                if (yuMatches >= 3)
                {
                    for (int i = 0; i < tempList.Count; i++)
                    {
                        if (!matchingStones.Contains(tempList[i]))
                        {
                            matchingStones.Add(tempList[i]);
                        }
                    }
                }

                tempList.Clear();
                int ydMatches = 1;
                for (int yd = y - 1; yd >= 0; yd--)
                {
                    BoardPiece controlPiece = BoardData[x][yd];
                    if (controlPiece.StonePiece == null)
                    {
                        break;
                    }
                    if (controlPiece.StonePiece.stoneType == stoneType)
                    {
                        ydMatches++;
                        tempList.Add(controlPiece);
                    }
                    else
                    {
                        break;
                    }
                }
                if (ydMatches >= 3)
                {
                    for (int i = 0; i < tempList.Count; i++)
                    {
                        if (!matchingStones.Contains(tempList[i]))
                        {
                            matchingStones.Add(tempList[i]);
                        }
                    }
                }

                tempList.Clear();
                int xuMatches = 1;
                for (int xu = x + 1; xu < boardX; xu++)
                {
                    BoardPiece controlPiece = BoardData[xu][y];
                    if (controlPiece.StonePiece == null)
                    {
                        break;
                    }
                    if (controlPiece.StonePiece.stoneType == stoneType)
                    {
                        xuMatches++;
                        tempList.Add(controlPiece);
                    }
                    else
                    {
                        break;
                    }
                }
                if (xuMatches >= 3)
                {
                    for (int i = 0; i < tempList.Count; i++)
                    {
                        if (!matchingStones.Contains(tempList[i]))
                        {
                            matchingStones.Add(tempList[i]);
                        }
                    }
                }

                tempList.Clear();
                int xdMatches = 1;
                for (int xd = x - 1; xd >= 0; xd--)
                {
                    BoardPiece controlPiece = BoardData[xd][y];
                    if (controlPiece.StonePiece == null)
                    {
                        break;
                    }
                    if (controlPiece.StonePiece.stoneType == stoneType)
                    {
                        xdMatches++;
                        tempList.Add(controlPiece);
                    }
                    else
                    {
                        break;
                    }

                }
                if (xdMatches >= 3)
                {
                    for (int i = 0; i < tempList.Count; i++)
                    {
                        if (!matchingStones.Contains(tempList[i]))
                        {
                            matchingStones.Add(tempList[i]);
                        }
                    }
                }

                tempList.Clear();
            }
        }

        Debug.Log("matchingStones : " +matchingStones.Count);
        return matchingStones;
    }

    //Stone Fall
    public void FallStonesToEmptyPieces()
    {
        List<BoardPiece> falledPieces = new List<BoardPiece>();

        for (int x = 0; x < boardX; x++)
        {
            for (int y = 0; y < boardY; y++)
            {
                if (BoardData[x][y].StonePiece == null)
                {
                    for (int yu = y + 1; yu < boardY; yu++)
                    {
                        BoardPiece controlPiece = BoardData[x][yu];
                        if (controlPiece.StonePiece != null)
                        {
                            BoardData[x][y].ReplaceStones(controlPiece);
                            falledPieces.Add(BoardData[x][y]);
                            break;
                        }
                    }
                }
            }
        }

        for (int i = 0; i < falledPieces.Count; i++)
        { 
            falledPieces[i].AnimateStone();
        }
    }

    
    public void SpawnNewStones()
    {
        for (int x = 0; x < boardX; x++)
        {
            for (int y = 0; y < boardY; y++)
            {
                if (BoardData[x][y].StonePiece == null)
                {
                    CreateRandomStone(x, y, false);
                }
            }
        }
    }

    //Stone function Move 
    private void OnStoneSwitchRequested(BoardPiece holdingPiece, Direction direction)
    {
        if (GetBoardID(holdingPiece.BoardID, direction, out BoardID target))
        {
            StartCoroutine(SwitchStonesRoutine(holdingPiece, BoardData[target.X][target.Y]));
        }
        else
        {
            InputManager.isInputLocked = false;
        }
    }

    //Switch stones routine
    private IEnumerator SwitchStonesRoutine(BoardPiece holdingPiece, BoardPiece targetPiece)
    {
        float actionDuration = holdingPiece.StoneMoveDuration;
        holdingPiece.ReplaceStones(targetPiece);
        holdingPiece.AnimateStone();
        targetPiece.AnimateStone();
        yield return new WaitForSeconds(actionDuration);
        if (IsMatchExist())
        {
            yield return StartCoroutine(ExplodeMatches(actionDuration));
            InputManager.isInputLocked = false;
        }
        else
        {
            holdingPiece.ReplaceStones(targetPiece);
            holdingPiece.AnimateStone();
            targetPiece.AnimateStone();
            yield return new WaitForSeconds(actionDuration);
            InputManager.isInputLocked = false;
        }
    }

    //blast matching tiles
    private IEnumerator ExplodeMatches(float actionDuration)
    {
        List<BoardPiece> matchingPieces = GetMatchingPieces();
        for (int i = 0; i < matchingPieces.Count; i++)
        {
            matchingPieces[i].StonePiece.gameObject.SetActive(false);
            matchingPieces[i].StonePiece = null;
        }
        FallStonesToEmptyPieces();
        yield return new WaitForSeconds(actionDuration);
        SpawnNewStones();
        yield return new WaitForSeconds(actionDuration);
        if (IsMatchExist())
        {
            yield return StartCoroutine(ExplodeMatches(actionDuration));
        }
    }

    //Environmental control
    public bool GetBoardID(BoardID boardID, Direction direction, out BoardID target)
    {
        switch (direction)
        {
            case Direction.Right:
                if (boardID.X + 1 < boardX)
                {
                    target = BoardData[boardID.X + 1][boardID.Y].BoardID;
                    return true;
                }
                break;
            case Direction.Left:
                if (boardID.X - 1 > 0)
                {
                    target = BoardData[boardID.X - 1][boardID.Y].BoardID;
                    return true;
                }
                break;
            case Direction.Down:
                if (boardID.Y - 1 > 0)
                {
                    target = BoardData[boardID.X][boardID.Y - 1].BoardID;
                    return true;
                }
                break;
            case Direction.Up:
                if (boardID.Y + 1 < boardY)
                {
                    target = BoardData[boardID.X][boardID.Y + 1].BoardID;
                    return true;
                }
                break;
            default:
                break;
        }

        target = null;
        return false;
    }
}

public enum Direction
{
    Right,
    Left,
    Down,
    Up
}

public class BoardID
{
    public int X;
    public int Y;
}