using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class GameController : MonoBehaviour
{
    
    [SerializeField] private PlayerNetworkSync _playerPrefab = default;
    [SerializeField] private BuildingController _buildingPrefab = default;
    [SerializeField] private ToolNetworkSync _toolPrefab = default;

    [SerializeField] private GameConfiguration _gameConfig;
    [SerializeField] private float _buildingDistance;

    [SerializeField] private ScoreController _scoreController;

    [SerializeField] private GameObject _resultPanel;
    [SerializeField] private TextMeshProUGUI _resultText;
    [SerializeField] private TextMeshProUGUI _resultScore;

    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Transform _enemyRootSpawn;

    public float MinWorldBounds;
    public float MaxWorldBounds;

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

    public bool IsGameEnded { get; private set; }

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
        _resultPanel.SetActive(false);

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
        MinWorldBounds = pos.m_MinXPosition;
        MaxWorldBounds = pos.m_MaxXPosition;

        for (int i = 0; i < Random.Range(18, 22); i++)
        {
            var toolData = Instantiate(_toolPrefab);
            ToolType type = (ToolType) (i % 2);
            if (type == ToolType.Paint) type = ToolType.Seed;

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

        for (int i = 0; i < Random.Range(2, 4); i++)
        {
            var toolData = Instantiate(_toolPrefab);

            toolData.Setup(new ToolNetwork()
            {
                horizontalPosition = Random.Range(
                    pos.m_MinXPosition + 2,
                    pos.m_MaxXPosition - 2
                ),
                verticalPosition = (LayerHeight) Random.Range(0, 2),
                type = ToolType.AirHorn,
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
        int pTotalScore = _scoreController.CalculateFinalScore();
        
        
        if(pTotalScore >= _gameConfig.WinThreshold)
        {
            _resultText.text = "Win";
            
        }
        else
        {
            _resultText.text = "Lose";
        }
        _resultPanel.SetActive(true);
        _resultScore.text = pTotalScore.ToString();

        foreach(var item in Players)
        {
            PlayerMovement pMovement = item.Value.GetComponent<PlayerMovement>();
            pMovement.LeaveBuilding();
            IsGameEnded = true;
        }

        Instantiate(_enemyPrefab).transform.position = _enemyRootSpawn.position;

    }

    public void UpdateAllChallenges()
    {
        _scoreController.UpdateChallenges();
    }
}
