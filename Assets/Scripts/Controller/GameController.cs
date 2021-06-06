using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    
    [SerializeField] private PlayerNetworkSync _playerPrefab = default;
    [SerializeField] private BuildingController _buildingPrefab = default;
    [SerializeField] private ToolNetworkSync _toolPrefab = default;

    [SerializeField] private GameConfiguration _gameConfig;

    [SerializeField] private float _buildingDistance;

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

        BuildingController building;
        List<BuildingNetwork> _buildings = _gameConfig.BuildingData;
        int x = 0;
        for (int i = 0; i < _buildings.Count; i++)
        {
            building = Instantiate(_buildingPrefab);
            building.transform.position = new Vector3(0 + _buildingDistance * i, 0, 0f);
            building.Setup(_buildings[i]);
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
