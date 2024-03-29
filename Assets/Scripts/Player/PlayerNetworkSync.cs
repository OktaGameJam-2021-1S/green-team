using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetworkSync : MonoBehaviour
{

    private int _id;
    public int ID => _id;

    private PlayerMovement _movement;
    public PlayerMovement Movement => _movement;
    
    private PlayerTool _tool;
    public PlayerTool Tool => _tool;

    private bool _hasTool;
    public bool HasTool => _hasTool;

    private int _toolId;
    public int ToolId => _toolId;

    private float _moveSpeed;
    private float _speed;
    public float SpeedNormalized => _speed / _moveSpeed;

    private GameController _controller;

    private void Awake()
    {
        _controller = GameController.FindObjectOfType<GameController>();
        _movement = GetComponent<PlayerMovement>();
        _tool = GetComponent<PlayerTool>();
    }

}
