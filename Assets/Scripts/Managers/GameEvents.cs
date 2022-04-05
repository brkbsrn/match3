using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents Instance;

    //Fuction trigger
    public event Action<BoardPiece[][]> OnBoardCreated;

    public event Action<BoardPiece, Direction> OnStoneSwitchRequested;

    private void Awake()
    {
        Instance = this;
    }

    public void BoardCreated(BoardPiece[][] boardData) 
    {
        OnBoardCreated?.Invoke(boardData);
    }

    
    public void StoneSwitchRequested(BoardPiece boardPiece, Direction direction) 
    {
        OnStoneSwitchRequested?.Invoke(boardPiece, direction);
    }
}
