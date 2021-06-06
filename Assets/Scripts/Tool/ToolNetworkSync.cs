using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ToolType
{
    Hammer = 0,
    Paint = 1,
    Seed = 2,
    AirHorn = 3,
}

public class ToolNetworkSync : MonoBehaviour
{

    [SerializeField] private GenericDictionary<ToolType, AudioClip> _toolSfx = default;
    private ToolPosition _position;
    public ToolPosition Position => _position;

    private ToolSprite _toolSprite;
    private ToolSprite ToolSprite => _toolSprite;

    private int _uses;
    public int Uses => _uses;

    public bool IsUsable => _uses > 0;

    private AudioSource _source;

    private void Awake()
    {
        _position = GetComponent<ToolPosition>();
        _toolSprite = GetComponent<ToolSprite>();
        _source = GetComponent<AudioSource>();
    }
    
    public void Setup(ToolNetwork network)
    {
        _toolSprite.Construct(network.type);
        gameObject.name = network.type.ToString();
        _uses = network.uses;
        _position.Setup(network.horizontalPosition, network.verticalPosition);
    }

    public bool UseTool(BuildingController building)
    {
        if (building.PeopleInBuilding > 0 && _toolSprite.Type != ToolType.AirHorn) return true;
        _uses -= 1;
        if (_toolSprite.Type == ToolType.Hammer)
        {
            building.DealDamageFloor();
        }
        else if (_toolSprite.Type == ToolType.Seed)
        {
            building.NaturalizeFloor();
        }
        else if (_toolSprite.Type == ToolType.Paint)
        {
            building.GraffitiFloor();
        }
        else if (_toolSprite.Type == ToolType.AirHorn)
        {
            building.AirHorn();
        }

        // Play a sound
        if (_toolSfx.ContainsKey(_toolSprite.Type))
        {
            _source.PlayOneShot(_toolSfx[_toolSprite.Type]);
        }

        if (_uses <= 0)
        {
            gameObject.SetActive(false);
            return false;
        }
        GameController.Instance.UpdateAllChallenges();
        return true;
    }
}
