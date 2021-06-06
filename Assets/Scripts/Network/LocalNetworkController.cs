using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalNetworkController : NetworkController
{
    [SerializeField] ToolSprite.Tool tool;
    private GameStateNetwork _gameState;

    protected override void Awake()
    {
        base.Awake();
        _gameState = new GameStateNetwork();
        _gameState.players = new List<PlayerNetwork>();
        _gameState.players.Add(new PlayerNetwork()
        {
            id = 0,
            x = 0,
            y = 0,
            toolId = 0,
            hasTool = true
        }); ;

        _gameState.buildings = new List<BuildingNetwork>();
        float x = 0;
        for (int i = 0; i < 7; i++)
        {
            int height = Random.Range(1, 4);
            int width = Random.Range(1, 4);
            _gameState.buildings.Add(new BuildingNetwork()
            {
                id = i,
                x = x,
                width = width,
                height = height,
                color = "#ffcc00",
                damage = 0,
                plant = 0,
                graffiti = 0
            });
            x += width;
        }

        _gameState.tools = new List<ToolNetwork>();
        _gameState.tools.Add(new ToolNetwork()
        {
            id = 0,
            x = 1,
            y = 1,
            type = (int) tool,
            uses = 2,
            isHold = true
        });
    }

    protected override void Start()
    {

    }

    public override void SendInput(PlayerInputNetwork inputNetwork)
    {
        _gameState.players[0].moveSpeed = 5;
        _gameState.players[0].speed = inputNetwork.horizontal * _gameState.players[0].moveSpeed;
        _gameState.players[0].x += _gameState.players[0].speed * Time.deltaTime;
        _gameState.players[0].y += inputNetwork.vertical;

    }

    public override void SendDamageBuilding(DamageBuildingNetwork damageBuildingNetwork)
    {
        _gameState.buildings[damageBuildingNetwork.id].damage += 1;
    }

    public override void SendSeedBuilding(SeedBuildingNetwork seedBuildingNetwork)
    {
        _gameState.buildings[seedBuildingNetwork.id].plant += 1;
    }
    
    private void Update()
    {
        DispatchGameState(_gameState);
    }

}
