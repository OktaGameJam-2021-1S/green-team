using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    
    [SerializeField] private PlayerNetworkSync _playerPrefab = default;
    [SerializeField] private BuildingNetworkSync _buildingPrefab = default;

    private bool _isFirstGameState;
    private Dictionary<int, PlayerNetworkSync> _players;
    private Dictionary<int, BuildingNetworkSync> _buildings;

    private void Awake()
    {
        _isFirstGameState = true;
        _players = new Dictionary<int, PlayerNetworkSync>();
        _buildings = new Dictionary<int, BuildingNetworkSync>();
    }

    private void Start()
    {
        NetworkController.Instance.OnGameState += HandleGameState;
    }

    private void HandleGameState(GameStateNetwork state)
    {
        foreach (var player in state.players)
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
        
        foreach (var building in state.buildings)
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
    }

}