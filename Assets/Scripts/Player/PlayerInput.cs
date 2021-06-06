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
        if (_tool.HasTool && _playerMovement.InsideBuilding)
        {
            _tool.Use(_playerMovement.InsideBuilding);
        }
    }

    private void PickTool()
    {
        if (_tool.HasTool)
        {
            DropTool();
            return;
        }

        foreach (var toolSync in _controller.Tools)
        {
            if (!toolSync.IsUsable) continue;
            if (toolSync.Position.VerticalPosition != _playerMovement.VerticalPosition) continue;
            if (Vector3.Distance(toolSync.transform.position, transform.position) < .5f)
            {
                _tool.HoldTool(toolSync);
                break;
            }
        }
    }

    public void DropTool()
    {
        _tool.DropTool();
    }

}
