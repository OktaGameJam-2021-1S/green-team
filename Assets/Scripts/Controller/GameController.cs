using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    
    [SerializeField] private PlayerNetworkSync _playerPrefab = default;
    [SerializeField] private BuildingNetworkSync _buildingPrefab = default;
    [SerializeField] private ToolNetworkSync _toolPrefab = default;

    private bool _isFirstGameState;
    private Dictionary<int, PlayerNetworkSync> _players;
    private Dictionary<int, BuildingNetworkSync> _buildings;
    private Dictionary<int, ToolNetworkSync> _tools;

    public static GameController Instance { get; private set; }

    public Dictionary<int, PlayerNetworkSync> Players => _players;
    public Dictionary<int, ToolNetworkSync> Tools => _tools;
    public List<BuildingNetworkSync> Buildings
    {
        get
        {
            List<BuildingNetworkSync> items = new List<BuildingNetworkSync>();
            items.AddRange(_buildings.Values);
            return items;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;

        _isFirstGameState = true;
        _players = new Dictionary<int, PlayerNetworkSync>();
        _buildings = new Dictionary<int, BuildingNetworkSync>();
        _tools = new Dictionary<int, ToolNetworkSync>();
    }

    private void Start()
    {
        var player = Instantiate(_playerPrefab);
        _players.Add(player.ID, player);

        BuildingNetworkSync buildData;
        int x = 0;
        for (int i = 0; i < 10; i++)
        {
            buildData = Instantiate(_buildingPrefab);
            int width = Random.Range(1, 4);
            int height = Random.Range(1, 3);
            buildData.Sync(new BuildingNetwork()
            {
                id = i,
                x = x,
                width = width,
                height = height,
                color = "#ffcc00",
                damage = 0,
                plant = 0,
                graffiti = 0,
                maxDamage = 5,
                maxPlant = 5,
                people = 5,
            });
            x += width;
            _buildings.Add(buildData.ID, buildData);
        }

        var toolData = Instantiate(_toolPrefab);
        toolData.Sync(new ToolNetwork()
        {
            id = 0,
            x = 5,
            y = 1,
            type = (int)ToolSprite.Tool.Hammer,
            uses = 5,
            isHold = false
        });
        _tools.Add(toolData.ID, toolData);
    }

    public BuildingNetworkSync GetBuilding(PlayerMovement movement)
    {
        if (movement.VerticalPosition != LayerHeight.Sidewalk) return null;
        var building = Buildings.Find(building => {
            if (building.transform.position.x < movement.transform.position.x)
            {
                if (building.transform.position.x + building.Width > movement.transform.position.x)
                {
                    return true;   
                }
            }
            return false;
        });

        if (!building) return null;
        return building;
    }

}
