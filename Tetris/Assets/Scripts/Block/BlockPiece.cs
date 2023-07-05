using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPiece : MonoBehaviour
{
    private LevelBlocks _level;

    public void Init(LevelBlocks level, Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
        _level = level;
    }

    public bool CheckFall()
    {
        int positionX = Mathf.RoundToInt(transform.position.x) + Vector2Int.down.x;
        int positionY = Mathf.RoundToInt(transform.position.y) + Vector2Int.down.y;

        if (positionY < 0)
        {
            return false;
        }

        if (positionY < _level.BlockOnLevel.GetLength(1))
        {
            if (_level.BlockOnLevel[positionX, positionY] != null)
            {
                return false;
            }
        }

        return true;
    }

    public bool CheckMove(Vector2Int direction)
    {
        int positionX = Mathf.RoundToInt(transform.position.x) + direction.x;
        int positionY = Mathf.RoundToInt(transform.position.y) + direction.y;

        if (positionX < 0 || positionX > _level.BlockOnLevel.GetLength(0) - 1)
        {
            return false;
        }

        if (positionY < 0)
        {
            return false;
        }

        if (positionY < _level.BlockOnLevel.GetLength(1) - 1)
        {
            if (_level.BlockOnLevel[positionX, positionY] != null)
            {
                return false;
            }
        }

        return true;
    }

    public bool ChechRotation(out Vector2Int pushDirection, out bool isAnotherBlock)
    {
        int positionX = Mathf.RoundToInt(transform.position.x);
        int positionY = Mathf.RoundToInt(transform.position.y);

        if (positionX < 0)
        {
            pushDirection = Vector2Int.right;
            isAnotherBlock = false;
            return false;
        }

        if (positionX > _level.BlockOnLevel.GetLength(0) - 1)
        {
            pushDirection = Vector2Int.left;
            isAnotherBlock = false;
            return false;
        }

        if (positionY < 0)
        {
            pushDirection = Vector2Int.zero;
            isAnotherBlock = true;
            return false;
        }

        if (positionY < _level.BlockOnLevel.GetLength(1) - 1)
        {
            if (_level.BlockOnLevel[positionX, positionY] != null)
            {
                pushDirection = Vector2Int.zero;
                isAnotherBlock = true;
                return false;
            }
        }

        pushDirection = Vector2Int.zero;
        isAnotherBlock = false;
        return true;
    }
}
