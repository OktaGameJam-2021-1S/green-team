using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameController : MonoBehaviour
{
    
    [SerializeField] private PlayerNetworkSync _playerPrefab = default;
    [SerializeField] private BuildingController _buildingPrefab = default;
    [SerializeField] private ToolNetworkSync _toolPrefab = default;

    [SerializeField] private GameConfiguration _gameConfig;
    [SerializeField] private float _buildingDistance;

    [SerializeField] private ScoreController _scoreController;

    public float BuildingDistance => _buildingDistance;

    private CinemachineVirtualCamera cinemachineVirtualCamera;

    private bool _isFirstGameState;
    private Dictionary<int, PlayerNetworkSync> _players;
    private Dictionary<int, BuildingController> _buildings;
    private List<ToolNetworkSync> _tools;

    public static GameController Instance { get; private set; }

    public Dictionary<int, PlayerNetworkSync> Players => _players;
    public List<ToolNetworkSync> Tools => _tools;
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
        _tools = new List<ToolNetworkSync>();
    }

    private void Start()
    {
        cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();

        var player = Instantiate(_playerPrefab);
        _players.Add(player.ID, player);

        cinemachineVirtualCamera.Follow = player.transform;

        BuildingController building;
        List<BuildingNetwork> lBuildingDatas = _gameConfig.BuildingData;
        for (int i = 0; i < lBuildingDatas.Count; i++)
        {
            building = Instantiate(_buildingPrefab);
            building.transform.position = new Vector3(0 + _buildingDistance * i, 0, 0f);
            building.Setup(lBuildingDatas[i]);
            _buildings.Add(lBuildingDatas[i].id, building);
        }

        LockCameraY pos = cinemachineVirtualCamera.GetComponent<LockCameraY>();
        pos.m_MinXPosition = -(_buildingDistance / 2f);
        pos.m_MaxXPosition = (lBuildingDatas.Count * _buildingDistance) - (_buildingDistance / 2f);

        for (int i = 0; i < Random.Range(7, 20); i++)
        {
            var toolData = Instantiate(_toolPrefab);
            ToolType type = (ToolType) (i % 4);
            if (type == ToolType.Paint) type = i % 2 == 0 ? ToolType.Hammer : ToolType.Seed;

            toolData.Setup(new ToolNetwork()
            {
                horizontalPosition = Random.Range(
                    pos.m_MinXPosition + 2,
                    pos.m_MaxXPosition - 2
                ),
                verticalPosition = (LayerHeight) Random.Range(0, 2),
                type = type,
                uses = 5
            });
            _tools.Add(toolData);
        }
    }

    public BuildingController GetBuilding(PlayerMovement movement, bool getDemolished)
    {
        if (movement.VerticalPosition == LayerHeight.Street) return null;
        var building = Buildings.Find(building => {
            if (!getDemolished && building.Demolished) return false;
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


    public void GameEnd()
    {
        int i = _scoreController.CalculateFinalScore();
    }

    public void UpdateAllChallenges()
    {
        _scoreController.UpdateChallenges();
    }
}
