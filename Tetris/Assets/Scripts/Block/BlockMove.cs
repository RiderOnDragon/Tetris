using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BlockMove : MonoBehaviour
{
    private enum State
    {
        FALL,
        STOP_FALL
    }

    [SerializeField] private Block _block;
    [SerializeField] private Transform _rotationAxis;

    [Header("Stats")]
    [SerializeField] private float _fallTime = 1f;
    [SerializeField] private float _turboFallTime = 0.08f;
    [SerializeField] private float _moveTime = 0.08f;

    private float _currentFallTime = 0;
    private float _currentTurboFallTime = 0;
    private float _currentMoveTime = 0;

    private State _state = State.FALL;

    private const int ANGLE = -90;

    public static event Action<BlockPiece[]> EndedFallEvent;


    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (_state != State.FALL)
            return;

        InputPlayer();

        if (_currentFallTime <= 0)
        {
            Fall();
        }

        UptadeTimer();
    }

    private void Init()
    {
        _block = GetComponent<Block>();
        _currentFallTime = _fallTime;
    }

    private void UptadeTimer()
    {
        _currentMoveTime -= Time.deltaTime;
        _currentFallTime -= Time.deltaTime;
        _currentTurboFallTime -= Time.deltaTime;
    }

    private void InputPlayer()
    {
        if (PlayerInput.RightMove())
            Move(Vector2Int.right);
        else if (PlayerInput.LeftMove())
            Move(Vector2Int.left);


        if (PlayerInput.Rotate())
        {
            Rotate();
        }


        if (PlayerInput.TurboFall())
        {
            if (_currentTurboFallTime <= 0)
            {
                Fall();
                _currentTurboFallTime = _turboFallTime;
            }
        }
    }

    private void Move(Vector2Int direction)
    {
        if (_currentMoveTime > 0)
            return;

        if (CheckMove(direction) == false)
        {
            _block.AudioPlayer.PlayOneShot("Wall");
            return;
        }

        transform.position += (Vector3Int)direction;

        _block.AudioPlayer.PlayOneShot("Move");

        _currentMoveTime = _moveTime;
    }

    private void Fall()
    {
        if (CheckFall() == false)
        {
            StopFall();
            return;
        }

        transform.position += (Vector3Int)Vector2Int.down;

        _block.AudioPlayer.PlayOneShot("Fall");

        _currentFallTime = _fallTime;
    }

    private void Rotate()
    {
        transform.RotateAround(_rotationAxis.position, new Vector3Int(0, 0, ANGLE), 90);

        foreach (var block in _block.BlockPieces)
        {
            if (block.ChechRotation(out Vector2Int pushDirection, out bool isAnotherBlock) == false)
            {
                if (isAnotherBlock == false)
                {
                    transform.position += (Vector3Int)pushDirection;
                    return;
                }
                else
                {
                    transform.RotateAround(_rotationAxis.position, new Vector3Int(0, 0, -ANGLE), ANGLE);
                    return;
                }
            }
        }
    }

    private bool CheckFall()
    {
        foreach (var block in _block.BlockPieces)
        {
            if (block.CheckFall() == false)
            {
                return false;
            }
        }

        return true;
    }

    private bool CheckMove(Vector2Int direction)
    {
        foreach (var block in _block.BlockPieces)
        {
            if (block.CheckMove(direction) == false)
            {
                return false;
            }
        }

        return true;
    }

    private void StopFall()
    {
        _state = State.STOP_FALL;
        EndedFallEvent?.Invoke(_block.BlockPieces);
    }
}
