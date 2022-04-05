using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    //List shuffle 
    public static void Shuffle(this List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int i1 = Random.Range(0, list.Count);
            int i2 = Random.Range(0, list.Count);

            int t = list[i1];
            list[i1] = list[i2];
            list[i2] = t;
        }
    }

    //Replace stone
    public static void ReplaceStones(this BoardPiece bp, BoardPiece boardPiece)
    {
        StoneScript t = bp.StonePiece;
        bp.SetStone(boardPiece.StonePiece);
        boardPiece.SetStone(t);
    }
}
