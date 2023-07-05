using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBlocks : MonoBehaviour
{
    [SerializeField] private int _levelWidth = 10;
    [SerializeField] private int _levelHeight = 25;
    [SerializeField] private Transform _blockContainer;

    private BlockPiece[,] _blockOnLevel;
    public BlockPiece[,] BlockOnLevel => _blockOnLevel;

    private int _descroyLineCount = 0;

    public static event Action EndedAddBlockEvent;
    public static event Action<int> DestroedLine;
    public static event Action LosedEvent;

    private void Awake()
    {
        BlockMove.EndedFallEvent += AddBlockToLevel;
    }

    private void OnDestroy()
    {
        BlockMove.EndedFallEvent += AddBlockToLevel;
    }

    private void Start()
    {
        _blockOnLevel = new BlockPiece[_levelWidth, _levelHeight];
    }

    private void AddBlockToLevel(BlockPiece[] blockPieces)
    {
        foreach (var block in blockPieces)
        {
            int positionX = Mathf.RoundToInt(block.transform.position.x);
            int positionY = Mathf.RoundToInt(block.transform.position.y);

            if (positionY > _levelHeight - 1)
            {
                LosedEvent?.Invoke();
                return;
            }

            _blockOnLevel[positionX, positionY] = block;
        }

        _descroyLineCount = 0;

        foreach (var block in blockPieces)
        {
            int positionY = Mathf.RoundToInt(block.transform.position.y);

            CheckLine(positionY);
        }

        if (_descroyLineCount > 0)
            DestroedLine?.Invoke(_descroyLineCount);

        DeleteEmptyBlockParent();

        EndedAddBlockEvent?.Invoke();
    }

    private void CheckLine(int lineID)
    {
        for (int i = 0; i < _blockOnLevel.GetLength(0); i++)
        {
            if (_blockOnLevel[i, lineID] == null)
            {
                return;
            }
        }

        DestroyLine(lineID);
    }

    private void DestroyLine(int lineID)
    {
        _descroyLineCount++;

        for (int i = 0; i < _blockOnLevel.GetLength(0); i++)
        {
            Destroy(_blockOnLevel[i, lineID].gameObject);
            _blockOnLevel[i, lineID] = null;
        }

        DropAllUpLine(lineID);
    }

    private void DropAllUpLine(int lineID)
    {
        for (int i = 0; i < _blockOnLevel.GetLength(0); i++)
        {
            for (int j = lineID + 1; j < _blockOnLevel.GetLength(1); j++)
            {
                if (_blockOnLevel[i, j] != null)
                {
                    _blockOnLevel[i, j].transform.position += (Vector3)Vector2.down;
                    _blockOnLevel[i, j - 1] = _blockOnLevel[i, j];
                    _blockOnLevel[i, j] = null;

                }
            }
        }
    }

    private void DeleteEmptyBlockParent()
    {
        for (int i = 0; i < _blockContainer.childCount; i++)
        {
            var container = _blockContainer.GetChild(i);
            if (container.childCount == 1)
            {
                Destroy(container.gameObject);
            }
        }
    }
}
