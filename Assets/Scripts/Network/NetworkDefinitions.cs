using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetwork
{
    public int id;
    public float x;
    public int y;
    public int tool;
}

public class BuildingNetwork
{
    public int id;

    public int width;
    public int height;
    public float x;
    
    public string color;

    public int damage;
    public int plant;
    public bool graffiti;
}

public class GameStateNetwork
{
    public int gameId;
    public string hash;
    public List<PlayerNetwork> players;
    public List<BuildingNetwork> buildings;
}

public class PlayerInputNetwork
{
    public int vertical;
    public int horizontal;
    public bool use;

    public void Clear()
    {
        vertical = 0;
        use = false;
    }
}

public class DamageBuildingNetwork
{
    public int id;
}