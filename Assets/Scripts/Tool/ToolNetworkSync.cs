using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolNetworkSync : MonoBehaviour
{

    private int _id;
    public int ID => _id;
    
    private PlayerMovement _movement;
    public PlayerMovement Movement => _movement;

    private ToolSprite _toolSprite;
    private ToolSprite ToolSprite => _toolSprite;

    private void Awake()
    {
        _movement = GetComponent<PlayerMovement>();
        _toolSprite = GetComponent<ToolSprite>();
    }
    
    public void Sync(ToolNetwork network)
    {
        _id = network.id;
        _toolSprite.Construct((ToolSprite.Tool)network.type);
        if (!network.isHold)
        {
            _movement.MoveHorizontal(network.x);
            _movement.MoveVertical(network.y);
        }
    }

    public void UseTool(PlayerMovement movement)
    {
        var building = GameController.Instance.GetBuilding(movement);
        if (building)
        {
            if (_toolSprite.Type == ToolSprite.Tool.Hammer)
            {
                building.DealDamageFloor();
            }
            else if (_toolSprite.Type == ToolSprite.Tool.Seed)
            {
                building.NaturalizeFloor();
            }
        }
    }
}
