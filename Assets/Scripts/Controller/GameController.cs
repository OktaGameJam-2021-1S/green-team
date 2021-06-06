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

    public float BuildingDistance => _buildingDistance;

    private bool _isFirstGameState;
    private Dictionary<int, PlayerNetworkSync> _players;
    private Dictionary<int, BuildingController> _buildings;
    private Dictionary<int, ToolNetworkSync> _tools;

    public static GameController Instance { get; private set; }

    public Dictionary<int, PlayerNetworkSync> Players => _players;
    public Dictionary<int, ToolNetworkSync> Tools => _tools;
    public List<BuildingController> Buildings
    {
        get
        {
            List<BuildingController> items = new List<BuildingController>();
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
        _buildings = new Dictionary<int, BuildingController>();
        _tools = new Dictionary<int, ToolNetworkSync>();
    }

    private void Start()
    {
        var player = Instantiate(_playerPrefab);
        _players.Add(player.ID, player);

        BuildingController building;
        List<BuildingNetwork> lBuildingDatas = _gameConfig.BuildingData;
        int x = 0;
        for (int i = 0; i < lBuildingDatas.Count; i++)
        {
            building = Instantiate(_buildingPrefab);
            building.transform.position = new Vector3(0 + _buildingDistance * i, 0, 0f);
            building.Setup(lBuildingDatas[i]);
            _buildings.Add(lBuildingDatas[i].id, building);
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

        toolData = Instantiate(_toolPrefab);
        toolData.Sync(new ToolNetwork()
        {
            id = 1,
            x = 7,
            y = 1,
            type = (int)ToolSprite.Tool.Seed,
            uses = 5,
            isHold = false
        });
        _tools.Add(toolData.ID, toolData);

        toolData = Instantiate(_toolPrefab);
        toolData.Sync(new ToolNetwork()
        {
            id = 2,
            x = 8,
            y = 1,
            type = (int)ToolSprite.Tool.Paint,
            uses = 5,
            isHold = false
        });
        _tools.Add(toolData.ID, toolData);

        toolData = Instantiate(_toolPrefab);
        toolData.Sync(new ToolNetwork()
        {
            id = 3,
            x = 2,
            y = 0,
            type = (int)ToolSprite.Tool.AirHorn,
            uses = 5,
            isHold = false
        });
        _tools.Add(toolData.ID, toolData);
    }

    public BuildingController GetBuilding(PlayerMovement movement)
    {
        if (movement.VerticalPosition == LayerHeight.Street) return null;
        var building = Buildings.Find(building => {
            if ((building.transform.position.x - BuildingDistance/2) < movement.transform.position.x)
            {
                if (building.transform.position.x + BuildingDistance/2 > movement.transform.position.x)
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
