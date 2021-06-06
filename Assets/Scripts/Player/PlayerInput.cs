using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    private int _horizontal;
    private int _vertical;
    private bool _use;

    private int _lastVertical;
    
    private GameController _controller;
    private PlayerMovement _playerMovement;
    private PlayerTool _tool;
    private PlayerNetworkSync _player;

    private void Awake()
    {
        _controller = FindObjectOfType<GameController>();
        _player = GetComponent<PlayerNetworkSync>();
        _tool = GetComponent<PlayerTool>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {

        _horizontal = (int) Input.GetAxisRaw("Horizontal");
        
        _vertical = 0;
        int vertical = (int) Input.GetAxisRaw("Vertical");
        if (vertical != _lastVertical)
        {
            _lastVertical = vertical;
            _vertical = vertical;
        }

        _use = false;
        if (Input.GetKeyDown(KeyCode.E))
        {
            _use = true;
            Use();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            PickTool();
        }

        _playerMovement.Move(_horizontal, _vertical);
    }

    private void Use()
    {
        if (_player.HasTool && _controller.Tools.ContainsKey(_player.ToolId))
        {
            _controller.Tools[_player.ToolId].UseTool(_player.Movement);
        }
    }

    private void PickTool()
    {
        if (_player.Tool.HasTool)
        {
            DropTool();
            return;
        }

        foreach (var toolKeyPair in _controller.Tools)
        {
            var toolSync = toolKeyPair.Value;
            if (toolSync.Movement.VerticalPosition != _player.Movement.VerticalPosition) continue;
            if (Vector3.Distance(toolSync.transform.position, _player.transform.position) < .5f)
            {
                toolSync.PickUpTool();
                _tool.HoldTool(toolSync.transform);
                break;
            }
        }
    }

    public void DropTool()
    {
    }

}
