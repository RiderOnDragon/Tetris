using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private BlockPiece[] _blockPieces;
    [SerializeField] private Color[] _piecesColor;
    [SerializeField] private AudioPlayer _audioPlayer;

    private LevelBlocks _level;

    public BlockPiece[] BlockPieces => _blockPieces;
    public AudioPlayer AudioPlayer => _audioPlayer;
    public Color[] PiecesColor => _piecesColor;
    public LevelBlocks Level => _level;

    public void Init(LevelBlocks level)
    {
        _level = level;

        Color piecesColor = _piecesColor[Random.Range(0, _piecesColor.Length)];
        foreach (var block in _blockPieces)
        {
            block.Init(_level, piecesColor);
        }
    }
}
