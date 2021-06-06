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

    private int _uses;
    public int Uses => _uses;

    private void Awake()
    {
        _movement = GetComponent<PlayerMovement>();
        _toolSprite = GetComponent<ToolSprite>();
    }
    
    public void Sync(ToolNetwork network)
    {
        _id = network.id;
        _toolSprite.Construct((ToolSprite.Tool)network.type);
        _uses = network.uses;
        if (!network.isHold)
        {
            _movement.MoveHorizontal(network.x);
            _movement.MoveVertical(network.y);
        }
    }

    public bool UseTool(PlayerMovement movement)
    {
        var building = GameController.Instance.GetBuilding(movement);
        if (building)
        {
            _uses -= 1;
            if (_toolSprite.Type == ToolSprite.Tool.Hammer)
            {
                building.DealDamageFloor();
            }
            else if (_toolSprite.Type == ToolSprite.Tool.Seed)
            {
                building.NaturalizeFloor();
            }
            else if (_toolSprite.Type == ToolSprite.Tool.Paint)
            {
                building.GraffitiFloor();
            }
            else if (_toolSprite.Type == ToolSprite.Tool.AirHorn)
            {
                building.AirHorn();
            }

            if (_uses <= 0)
            {
                gameObject.SetActive(false);
                return false;
            }
            
        }
        return true;
    }
}
