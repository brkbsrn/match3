using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardPiece : MonoBehaviour
{
    public StoneScript StonePiece;
    public float StoneMoveDuration;
    public BoardID BoardID;

    
    public void SetData(int x, int y)
    {
        BoardID = new BoardID()
        {
            X = x,
            Y = y
        };
    }

    //Forms stone on top
    public void StoneSpawned(StoneScript stone)
    {
        SetStone(stone);
        StonePiece.transform.position = transform.position + Vector3.up * 10;
        AnimateStone();
    }

    //puts a stone on the piece
    public void SetStone(StoneScript stone)
    {
        StonePiece = stone;
        if (StonePiece)
        {
            StonePiece.transform.parent = transform;
        }
    }

    //brings a stone to the piece.
    public void AnimateStone()
    {
        StonePiece.transform.DOMove(transform.position, StoneMoveDuration);
    }   
    

}
