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

    private bool bCanYell;

    private float fDeltaTime;
    private float fYellCooldown = 2f;

    private void Awake()
    {
        _controller = FindObjectOfType<GameController>();
        _player = GetComponent<PlayerNetworkSync>();
        _tool = GetComponent<PlayerTool>();
        _playerMovement = GetComponent<PlayerMovement>();
        bCanYell = true;
        fDeltaTime = 0;
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
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
        {
            _use = true;
            Use();
        }

        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
        {
            PickTool();
        }

        _playerMovement.Move(_horizontal, _vertical);


        if(!bCanYell)
        {
            fDeltaTime += Time.deltaTime;
            if(fDeltaTime >= fYellCooldown)
            {
                bCanYell = true;
                fDeltaTime = 0f;
            }
        }

    }

    private void Use()
    {
        if (GameController.Instance.IsGameEnded) return;
        if (_playerMovement.InsideBuilding)
        {
            if (_tool.HasTool)
            {
                _tool.Use(_playerMovement.InsideBuilding);
            }
            else if(bCanYell)
            {
                bCanYell = false;
                var building = GameController.Instance.GetBuilding(_playerMovement, false);
                building.Yell();
                GameController.Instance.UpdateAllChallenges();
            }
        }
    }

    private void PickTool()
    {
        if (GameController.Instance.IsGameEnded) return;
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
        if (GameController.Instance.IsGameEnded) return;
        if (_playerMovement.InsideBuilding) return;
        _tool.DropTool();
    }

}
