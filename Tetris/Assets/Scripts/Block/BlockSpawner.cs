using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] private Block[] _blockPrefab;
    [SerializeField] private Transform _container;
    [SerializeField] private LevelBlocks _level;

    [SerializeField] private Transform _nextBlockPreviewPosition;

    private Block _nextBlock;

    private void Awake()
    {
        LevelBlocks.EndedAddBlockEvent += SpawnBlock;
    }

    private void OnDestroy()
    {
        LevelBlocks.EndedAddBlockEvent -= SpawnBlock;
    }

    private void Start()
    {
        int blockID = Random.Range(0, _blockPrefab.Length);
        _nextBlock = Instantiate(_blockPrefab[blockID], _container);
        _nextBlock.Init(_level);

        SpawnBlock();
    }

    private void SpawnBlock()
    {
        _nextBlock.transform.position = transform.position;
        _nextBlock.GetComponent<BlockMove>().enabled = true;

        SpawnNextBlock();
    }

    private void SpawnNextBlock()
    {
        int blockID = Random.Range(0, _blockPrefab.Length);

        _nextBlock = Instantiate(_blockPrefab[blockID], _nextBlockPreviewPosition.position, Quaternion.identity, _container);
        _nextBlock.Init(_level);
        _nextBlock.GetComponent<BlockMove>().enabled = false;
    }
}
