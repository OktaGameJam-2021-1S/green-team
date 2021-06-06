using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetwork
{
    public int id;
    public float x;
    public int y;

    public float moveSpeed;
    public float speed;

    public bool hasTool;
    public int toolId;
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

public class ToolNetwork
{
    public int id;
    public float x;
    public int y;
    
    public int type;
    public int uses;
    public bool isHold;
}

public class GameStateNetwork
{
    public int gameId;
    public string hash;
    public List<PlayerNetwork> players;
    public List<BuildingNetwork> buildings;
    public List<ToolNetwork> tools;
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

public class SeedBuildingNetwork
{
    public int id;
}

public class PickUpToolNetwork
{
    public int id;
}

public class DropToolNetwork
{
    public int id;
}