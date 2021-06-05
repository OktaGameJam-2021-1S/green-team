using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalNetworkController : NetworkController
{
    [SerializeField] PlayerTool.Tool tool;
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
            tool = (int)tool
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
                graffiti = false
            });
            x += width;
        }
    }

    public override void SendInput(PlayerInputNetwork inputNetwork)
    {
        _gameState.players[0].x += 5f * inputNetwork.horizontal * Time.deltaTime;
        _gameState.players[0].y += inputNetwork.vertical;

        if (inputNetwork.use && _gameState.players[0].tool == (int)PlayerTool.Tool.Hammer)
        {
            _gameState.buildings[0].damage += 1;
        }

        if (inputNetwork.use && _gameState.players[0].tool == (int)PlayerTool.Tool.Seed)
        {
            _gameState.buildings[0].plant += 1;
        }

    }
    
    private void Update()
    {
        DispatchGameState(_gameState);
    }

}
