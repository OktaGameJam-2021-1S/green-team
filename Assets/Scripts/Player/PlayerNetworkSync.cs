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

    private GameController _controller;

    private void Awake()
    {
        _controller = GameController.FindObjectOfType<GameController>();
        _movement = GetComponent<PlayerMovement>();
        _tool = GetComponent<PlayerTool>();
    }
    
    public void Sync(PlayerNetwork network)
    {
        _id = network.id;
        _movement.MoveHorizontal(network.x);
        _movement.MoveVertical(network.y);
        if (!network.hasTool)
        {
            _tool.DropTool();
        }
        else if (_controller.Tools.ContainsKey(network.toolId))
        {
            _tool.HoldTool(_controller.Tools[network.toolId].transform);
        }
    }

}
