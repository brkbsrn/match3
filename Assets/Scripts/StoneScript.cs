using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneScript : MonoBehaviour
{
    public int stoneType;
    public Sprite[] icons;
    public SpriteRenderer renderer;

    //Stone  type and icon set
    public void SetStoneType(int stoneType)
    {
        this.stoneType = stoneType;
        renderer.sprite = icons[stoneType];
    }

}