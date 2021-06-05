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

    private GameStateNetwork _lastState;

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
        NetworkController.Instance.OnGameState += HandleGameState;
    }

    private void Update()
    {
        UpdateState();
    }

    private void UpdateState()
    {
        if (_lastState == null) return;
        foreach (var player in _lastState.players)
        {
            PlayerNetworkSync playerData = null;
            if (!_players.ContainsKey(player.id))
            {
                playerData = Instantiate(_playerPrefab);
                _players.Add(player.id, playerData);
            }
            else
            {
                playerData = _players[player.id];
            }
            playerData.Sync(player);
        }
        
        foreach (var building in _lastState.buildings)
        {
            BuildingNetworkSync buildingData = null;
            if (!_buildings.ContainsKey(building.id))
            {
                buildingData = Instantiate(_buildingPrefab);
                _buildings.Add(building.id, buildingData);
            }
            else
            {
                buildingData = _buildings[building.id];
            }
            buildingData.Sync(building);
        }
        
        foreach (var tool in _lastState.tools)
        {
            ToolNetworkSync toolData = null;
            if (!_tools.ContainsKey(tool.id))
            {
                toolData = Instantiate(_toolPrefab);
                _tools.Add(tool.id, toolData);
            }
            else
            {
                toolData = _tools[tool.id];
            }
            toolData.Sync(tool);
        }
    }

    private void HandleGameState(GameStateNetwork state)
    {
        _lastState = state;
    }

}
