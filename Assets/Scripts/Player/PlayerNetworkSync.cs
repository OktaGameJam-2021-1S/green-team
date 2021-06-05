using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetworkSync : MonoBehaviour
{

    private int _id;
    public int ID => _id;

    private PlayerMovement _movement;
    private PlayerTool _tool;

    private void Awake()
    {
        _movement = GetComponent<PlayerMovement>();
        _tool = GetComponent<PlayerTool>();
    }
    
    public void Sync(PlayerNetwork network)
    {
        _id = network.id;
        _movement.MoveHorizontal(network.x);
        _movement.MoveVertical(network.y);
        switch (network.tool)
        {
            case 0:
                _tool.DropTool();
                break;
            case 1:
                _tool.HoldTool(PlayerTool.Tool.Hammer);
                break;
            case 2:
                _tool.HoldTool(PlayerTool.Tool.Paint);
                break;
        }
    }

}
