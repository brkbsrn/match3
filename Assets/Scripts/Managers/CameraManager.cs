using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraManager : MonoBehaviour
{
    private Camera mainCamera;
    public float sizeMultiplier;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    private void Start()
    {
        GameEvents.Instance.OnBoardCreated += OnBoardCreated;
    }

    private void OnDestroy()
    {
        GameEvents.Instance.OnBoardCreated -= OnBoardCreated;
    }

    //Camera setting
    public void OnBoardCreated(BoardPiece[][] boardData)
    {
        int x = boardData[0].Length - 1;
        int y = boardData.Length - 1;
        BoardPiece leftBottomPiece = boardData[0][0];
        BoardPiece rightTopPiece = boardData[x][y];

        Vector3 leftBottom = leftBottomPiece.transform.position;
        Vector3 rightTop = rightTopPiece.transform.position;
        Vector3 middle = (leftBottom + rightTop) / 2f;

        Vector3 position = middle;
        position.z = mainCamera.transform.position.z;
        mainCamera.transform.position = position;

        int biggerAxisAmount = x > y ? x : y;

        mainCamera.orthographicSize = sizeMultiplier * biggerAxisAmount;
    }
}
