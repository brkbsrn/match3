using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Board Board;
    Vector3 startPos;
    Camera mainCamera;
    private bool isHolding;
    private BoardPiece holdingPiece;
    public static bool isInputLocked;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!isInputLocked)
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit2D = Physics2D.Raycast(ray.origin, ray.direction, 100, 1 << 6);
                if (hit2D.collider != null)
                {
                    BoardPiece bp = hit2D.collider.GetComponent<BoardPiece>();
                    holdingPiece = bp;
                    isHolding = true;
                }
            }
        
        }
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            if (isHolding)
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit2D = Physics2D.Raycast(ray.origin, ray.direction, 100, 1 << 6);
                if (hit2D.collider != null)
                {
                    BoardPiece bp = hit2D.collider.GetComponent<BoardPiece>();
                    if (bp != holdingPiece)
                    {
                        SwitchStones(holdingPiece, bp);
                        isHolding = false;
                    }
                }
            }
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            isHolding = false;
        }
    }

    private void SwitchStones(BoardPiece holdingPiece, BoardPiece bp)
    {
        isInputLocked = true;
        Direction direction = Direction.Down;
        if (holdingPiece.BoardID.X < bp.BoardID.X)
        {
            direction = Direction.Right;
        }
        else if (holdingPiece.BoardID.X > bp.BoardID.X)
        {
            direction = Direction.Left;
        }
        else if (holdingPiece.BoardID.Y < bp.BoardID.Y)
        {
            direction = Direction.Up;
        }
        else if (holdingPiece.BoardID.Y > bp.BoardID.Y)
        {
            direction = Direction.Down;
        }

        GameEvents.Instance.StoneSwitchRequested(holdingPiece, direction);
    }
}
