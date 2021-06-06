using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayerInput : MonoBehaviour
{

    // private int _playerId;
    // private int _horizontal;
    // private int _vertical;
    // private bool _use;

    // private int _lastVertical;

    // private NetworkController _networkController;
    // private GameController _controller;
    // private PlayerNetworkSync _player;

    // private void Awake()
    // {
    //     _playerId = 0;
    //     _networkController = GetComponent<NetworkController>();
    // }

    // public void Construct(PlayerNetwork playerNetwork)
    // {
    //     _playerId = playerNetwork.id;
    // }

    // private void Update()
    // {
    //     if (_networkController.CurrentGameStatus != NetworkController.GameStatus.Game) return;

    //     if (_controller == null)
    //     {
    //         _controller = GameController.FindObjectOfType<GameController>();
    //     }
    //     else if (_player == null && _controller.Players.ContainsKey(_playerId))
    //     {
    //         _player = _controller.Players[_playerId];
    //     }

    //     _horizontal = (int) Input.GetAxisRaw("Horizontal");
        
    //     _vertical = 0;
    //     int vertical = (int) Input.GetAxisRaw("Vertical");
    //     if (vertical != _lastVertical)
    //     {
    //         _lastVertical = vertical;
    //         _vertical = vertical;
    //     }

    //     _use = false;
    //     if (Input.GetKeyDown(KeyCode.E))
    //     {
    //         _use = true;
    //         Use();
    //     }

    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         PickTool();
    //     }

    //     SendInput();
    // }

    // private void Use()
    // {
    //     if (_player.HasTool && _controller.Tools.ContainsKey(_player.ToolId))
    //     {
    //         _controller.Tools[_player.ToolId].UseTool(_player.Movement);
    //     }
    // }

    // private void PickTool()
    // {
    //     if (_player.Tool.HasTool)
    //     {
    //         DropTool();
    //         return;
    //     }

    //     foreach (var toolKeyPair in _controller.Tools)
    //     {
    //         var toolSync = toolKeyPair.Value;
    //         if (toolSync.Movement.VerticalPosition != _player.Movement.VerticalPosition) continue;
    //         if (Vector3.Distance(toolSync.transform.position, _player.transform.position) < .5f)
    //         {
    //             toolSync.PickUpTool();
    //             break;
    //         }
    //     }
    // }

    // public void DropTool()
    // {
    //     NetworkController.Instance.SendDropTool(new DropToolNetwork());
    // }

    // private void SendInput()
    // {
        
    //     // Dispatch event with this 
    //     if (NetworkController.Instance)
    //     {
    //         var input = new PlayerInputNetwork()
    //         {
    //             horizontal = _horizontal,
    //             vertical = _vertical,
    //             use = _use
    //         };
    //         NetworkController.Instance.SendInput(input);
    //     }

    // }

}
