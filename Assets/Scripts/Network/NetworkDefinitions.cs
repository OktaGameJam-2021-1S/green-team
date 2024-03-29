using System;
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

[Serializable]
public class BuildingNetwork
{
    public int id;

    public int width;
    public int height;
    public float x;
    
    public string color;

    public int damage;
    public int plant;
    public int graffiti;

    public int maxDamage;
    public int maxPlant;

    public int people;
}

public class ToolNetwork
{
    public float horizontalPosition;
    public LayerHeight verticalPosition;
    
    public ToolType type;
    public int uses;
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